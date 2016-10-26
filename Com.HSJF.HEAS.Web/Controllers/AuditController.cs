using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.HEAS.BLL.Audit;
using Com.HSJF.HEAS.BLL.Other;
using Com.HSJF.HEAS.BLL.Other.Dto;
using Com.HSJF.HEAS.BLL.Sales;
using Com.HSJF.HEAS.Web.Helper;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Audit;
using Com.HSJF.HEAS.Web.Validations.Audit;
using Com.HSJF.Infrastructure.ExtendTools;
using Com.HSJF.Infrastructure.Extensions;
using Com.HSJF.Infrastructure.File;
using Com.HSJF.Infrastructure.Identity.Model;
using Com.HSJF.Infrastructure.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace Com.HSJF.HEAS.Web.Controllers
{
    [Authorize(Roles = "1Audit,2Audit")]
    public class AuditController : BaseController
    {
        private readonly AuditCasePush _auditCasePush;
        private readonly BaseAuditBll _baseAuditBll;
        private readonly BaseAuditDAL _baseAuditDal;
        private readonly SalesGroupBll _salesGroupBll;

        public AuditController()
        {
            _auditCasePush = new AuditCasePush();
            _baseAuditBll = new BaseAuditBll();
            _baseAuditDal = new BaseAuditDAL();
            _salesGroupBll = new SalesGroupBll();
        }

        public ActionResult AuditIndex()
        {
            var salesGroups = new List<SelectListItem>();
            salesGroups.Add(new SelectListItem()
            {
                Selected = true,
                Text = "",
                Value = "",
            });
            _salesGroupBll.GetAll().ForEach(p => salesGroups.Add(new SelectListItem
            {
                Text = p.Name,
                Value = p.ID
            }));
            ViewBag.SaleGroups = salesGroups;
            return View();
        }

        [HttpPost]
        public ActionResult GetPageIndex(BaseAuditListPageRequestViewModel request)
        {
            var baseAuditDal = new BaseAuditDAL();
            var modellist = baseAuditDal.GetAllAuthorizeAndSelf(CurrentUser);
            int total = 0;

            if (!string.IsNullOrEmpty(request.CaseStatus))
            {
                modellist = modellist.Where(t => t.CaseStatus == request.CaseStatus);
            }
            if (!string.IsNullOrEmpty(request.BorrowerName) && !string.IsNullOrWhiteSpace(request.BorrowerName))
            {
                modellist = modellist.Where(t => t.BorrowerName.Contains(request.BorrowerName));
            }
            if (!string.IsNullOrEmpty(request.CaseNum) && !string.IsNullOrWhiteSpace(request.CaseNum))
            {
                modellist = modellist.Where(t => t.NewCaseNum.Contains(request.CaseNum));
            }
            if (request.SalesGroupId.IsNotNullOrWhiteSpace() && request.SalesGroupId.IsNotNullOrEmpty())
            {
                modellist = modellist.Where(t => t.SalesGroupID == request.SalesGroupId);
            }

            var pageList = baseAuditDal.GetAllPage(modellist, out total, request.PageSize, request.PageIndex, request.Order,
                request.Sort);
            var newlist = pageList.ToList().Select(t => new BaseAuditViewModel().CastModel(t));

            var response = new BaseAuditListPageResponseViewModel();
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            response.Total = total;
            response.TotalPage = (int)Math.Ceiling((decimal)response.Total / response.PageSize);
            response.Data = newlist;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditAudit(string id)
        {
            ViewBag.ID = id;
            BaseAuditDAL bad = new BaseAuditDAL();

            var entity = bad.GetAuthorizeAndSelf(id, CurrentUser);
            if (entity == null)
            {
                return RedirectToAction("Failed", "Home");
            }

            var model = new BaseAuditViewModel().CastModel(entity);
            if (model == null)
            {
                return RedirectToAction("Failed", "Home");
            }
            return View(model);
        }

        /// <summary>
        /// 根据ID获取审核信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetBaseAudit(string id)
        {
            var response = new BaseResponse<BaseAuditViewModel>();
            var ch = new AuditHelper();

            var model = ch.FindByID(id, CurrentUser);
            if (model != null)
            {
                var ahp = new AuditHisHelper();
                var list = _baseAuditDal.GetListByCaseNum(model.CaseNum);
                model.AuditHistory = ahp.GetHistory(list);
                response.Status = StatusEnum.Success.ToString();
                response.Data = model;
            }
            else
            {
                response.Status = StatusEnum.Failed.ToString();
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditBaseAudit(BaseAuditViewModel model)
        {
            var response = new BaseResponse<string>();
            var bad = new BaseAuditDAL();
            var entity = bad.GetAuthorizeAndSelf(model.ID, CurrentUser);
            if (entity == null)
            {
                response.Status = "Failed";
                response.Message = new ErrorMessage[] { new ErrorMessage("权限", "权限不足") { } };
                return Json(response);
            }

            var result = new EditBaseAuditValidator().Validate(model);

            if (result.IsNotValid())
            {
                response.Status = "Failed";
                response.Message = result.GetErrors().ToArray();
                return Json(response);
            }
            else
            {
                var ch = new AuditHelper();
                var baseaudit = bad.Get(model.ID);

                int baseauditsum = 0;
                if (baseaudit.RelationPersonAudits != null)
                {
                    foreach (var item in baseaudit.RelationPersonAudits)
                    {
                        if (item.RelationType == DictionaryType.PersonType.JieKuanRenPeiOu)
                        {
                            baseauditsum = baseauditsum + 1;
                        }
                    }
                }
                int modelsum = 0;
                if (model.RelationPersonAudits != null)
                {
                    foreach (var item in model.RelationPersonAudits)
                    {
                        if (item.RelationType == DictionaryType.PersonType.JieKuanRenPeiOu)
                        {
                            modelsum = modelsum + 1;
                        }
                    }
                }
                if (modelsum > baseauditsum)
                {
                    response.Status = "Failed";
                    result.Add(new ErrorMessage("Data", "不能添加借款人配偶"));
                    response.Message = result.GetErrors().ToArray();
                    return Json(response);
                }

                #region 修改时限制不更新字段

                //审核时限制不更新字段
                //借款人姓名和证件类型，证件号码
                //抵押物编号
                if (baseaudit.BorrowerName != null)
                {
                    model.BorrowerName = baseaudit.BorrowerName;
                }
                //if (model.BorrowerPerson != null)
                //{
                //    var rela = baseaudit.RelationPersonAudits.FirstOrDefault(s => s.ID == model.BorrowerPerson.ID);

                //    if (rela != null)
                //    {
                //        if (rela.RelationType == "-PersonType-JieKuanRen")
                //        {
                //            model.BorrowerPerson.IdentificationNumber = rela.IdentificationNumber;
                //            model.BorrowerPerson.Name = rela.Name;
                //            model.BorrowerPerson.IdentificationType = rela.IdentificationType;
                //            model.BorrowerPerson.RelationType = rela.RelationType;
                //        }
                //    }
                //}
                if (model.RelationPersonAudits != null)
                {
                    foreach (var item in model.RelationPersonAudits)
                    {
                        var rela = baseaudit.RelationPersonAudits.FirstOrDefault(s => s.ID == item.ID);
                        if (rela != null)
                        {
                            if (rela.RelationType == "-PersonType-JieKuanRenPeiOu")
                            {
                                item.IdentificationNumber = rela.IdentificationNumber;
                                item.RelationType = rela.RelationType;
                                item.Name = rela.Name;
                                item.IdentificationType = rela.IdentificationType;
                            }
                        }
                    }
                }
                if (model.CollateralAudits != null)
                {
                    foreach (var item in model.CollateralAudits)
                    {
                        var Coll = baseaudit.CollateralAudits.FirstOrDefault(s => s.ID == item.ID);
                        if (Coll != null)
                        {
                            if (Coll.CollateralType == "-FacilityCategary-MainFacility")
                            {
                                item.CollateralType = Coll.CollateralType;
                                item.HouseNumber = Coll.HouseNumber;
                            }
                        }
                    }
                }

                #endregion 修改时限制不更新字段

                if (baseaudit == null || baseaudit.CaseStatus != CaseStatus.FirstAudit)
                {
                    response.Status = "Failed";
                    response.Message = new[] { new ErrorMessage("", "案件不存在或已处理") };
                }
                else
                {
                    model.Version = baseaudit.Version;
                    model.CaseStatus = baseaudit.CaseStatus;
                    model.CreateUser = CurrentUser.UserName;
                    model.CreateTime = DateTime.Now;
                    ch.UpdateBaseAudit(model, "update");

                    response.Status = "Success";
                }
            }
            return Json(response);
        }

        /// <summary>
        /// 一审通过(调查)
        /// </summary>
        /// <param name="model">案件数据</param>
        /// <returns>提交结果</returns>
        [HttpPost]
        public ActionResult SubmitBaseAudit(BaseAuditViewModel model)
        {
            var auditHelper = new AuditHelper();
            var baseAuditDal = new BaseAuditDAL();
            var response = new BaseResponse<string>();
            var entity = baseAuditDal.GetAuthorizeAndSelf(model.ID, CurrentUser);
            if (entity == null)
            {
                response.Status = "Failed";
                response.Message = new ErrorMessage[] { new ErrorMessage("权限", "权限不足") { } };
                return Json(response);
            }

            var validator = new SubmitBaseAuditValidator();

            var result = validator.Validate(model);

            if (model.OutboundCost == null || model.OutboundCost <= 0)
                result.Add(new ErrorMessage("OutboundCost", "外访费（下户费） 不能为空"));

            // 验证证件有效期
            var editResult = new EditBaseAuditValidator().Validate(model);
            result.Add(editResult.GetErrors());

            if (!ModelState.IsValid)
            {
                foreach (var e in ModelState.Keys)
                {
                    if (ModelState[e].Errors.Any())
                    {
                        if (e.Contains("FileName")) continue;
                        if (string.IsNullOrEmpty(ModelState[e].Errors[0].ErrorMessage)) continue;
                        if (e.Contains("BorrowerPerson.RelationType")) continue;
                        result.Add(new ErrorMessage(e, ModelState[e].Errors[0].ErrorMessage));
                    }
                }
            }

            if (result.IsNotValid())
            {
                response.Status = "Failed";
                response.Message = result.GetErrors().ToArray();
                response.Data = null;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            var baseAudit = _baseAuditBll.QueryLeatest(model.CaseNum);

            if (baseAudit == null || baseAudit.CaseStatus != CaseStatus.FirstAudit)
            {
                result.Add(new ErrorMessage("Data", "数据过期"));
                response.Status = "Failed";
                response.Message = result.GetErrors().ToArray();
                response.Data = null;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            // 验证抵押物

            //if (baseAudit.BorrowerName != null)
            //{
            //    model.BorrowerName = baseAudit.BorrowerName;
            //}
            //if (model.BorrowerPerson != null)
            //{
            //    var rela = baseAudit.RelationPersonAudits.FirstOrDefault(s => s.ID == model.BorrowerPerson.ID);

            //    if (rela != null)
            //    {
            //        if (rela.RelationType == DictionaryType.PersonType.JieKuanRen)
            //        {
            //            model.BorrowerPerson.IdentificationNumber = rela.IdentificationNumber;
            //            model.BorrowerPerson.Name = rela.Name;
            //            model.BorrowerPerson.IdentificationType = rela.IdentificationType;
            //            model.BorrowerPerson.RelationType = rela.RelationType;
            //        }
            //    }
            //}
            if (model.RelationPersonAudits != null)
            {
                foreach (var item in model.RelationPersonAudits)
                {
                    var rela = baseAudit.RelationPersonAudits.FirstOrDefault(s => s.ID == item.ID);
                    if (rela != null)
                    {
                        if (rela.RelationType == DictionaryType.PersonType.JieKuanRenPeiOu)
                        {
                            item.IdentificationNumber = rela.IdentificationNumber;
                            item.RelationType = rela.RelationType;
                            item.Name = rela.Name;
                            item.IdentificationType = rela.IdentificationType;
                        }
                    }
                }
            }
            if (model.CollateralAudits != null)
            {
                foreach (var item in model.CollateralAudits)
                {
                    var Coll = baseAudit.CollateralAudits.FirstOrDefault(s => s.ID == item.ID);
                    if (Coll != null)
                    {
                        if (Coll.CollateralType == "-FacilityCategary-MainFacility")
                        {
                            item.CollateralType = Coll.CollateralType;
                            item.HouseNumber = Coll.HouseNumber;
                        }
                    }
                }
            }

            // 更新指定字段
            baseAudit.CaseMode = model.CaseMode;
            baseAudit.ThirdParty = model.ThirdParty;
            baseAudit.MonthlyInterest = model.MonthlyInterest;
            baseAudit.LendingDate = model.LendingDate;
            baseAudit.PaymentDate = model.PaymentDate;
            baseAudit.ActualInterest = model.ActualInterest;
            baseAudit.AdvanceInterest = model.AdvanceInterest;
            baseAudit.Description = model.Description;
            //baseAudit.CreateTime = DateTime.Now;
            //baseAudit.CreateUser = CurrentUser.UserName;
            baseAuditDal.Update(baseAudit);
            baseAuditDal.AcceptAllChange();

            model.CreateTime = DateTime.Now;
            model.CreateUser = User.Identity.Name;
            model.CaseStatus = CaseStatus.SecondAudit;
            var baseAuditViewModel = auditHelper.AddBaseAudit(model, "add"); //保存信息

            response.Status = baseAuditViewModel != null ? "Success" : "Failed";

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除案件
        /// </summary>
        /// <param name="id">案件Id</param>
        /// <returns>删除结果</returns>
        [HttpPost]
        public ActionResult DeleteBaseAudit(string id)
        {
            var bad = new BaseAuditDAL();
            var entity = bad.GetAuthorizeAndSelf(id, CurrentUser);
            if (entity == null)
            {
                return GetBaseResponse<BaseAudit>(false);
            }
            bad.Delete(entity);
            bad.AcceptAllChange();
            return GetBaseResponse<BaseAudit>(true);
        }

        /// <summary>
        /// 重载审批拒绝
        /// </summary>
        /// <param name="model">案件信息</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RejectBaseAudit(BaseAuditViewModel model)
        {
            var response = new BaseResponse<string>();

            var baseAuditDal = new BaseAuditDAL();
            var entity = baseAuditDal.GetAuthorizeAndSelf(model.ID, CurrentUser);
            if (entity == null)
            {
                response.Status = "Failed";
                response.Message = new ErrorMessage[] { new ErrorMessage("权限", "权限不足") { } };
                return Json(response);
            }

            var ba = new BaseAuditDAL();
            var ch = new AuditHelper();
           // var relationstatebll = new RelationStateBLL();
            BaseAudit baudit = ba.Get(model.ID);
            if (baudit == null)
            {
                List<ErrorMessage> em = new List<ErrorMessage>();
                ErrorMessageAdd("Data", "数据过期", em);
                response.Status = "Failed";
                response.Message = em.ToArray();
                response.Data = null;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(model.RejectType))
            {
                List<ErrorMessage> em = new List<ErrorMessage>();
                ErrorMessageAdd("RejectType", "拒绝理由不能为空", em);
                response.Status = "Failed";
                response.Message = em.ToArray();
                response.Data = null;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            var pushResult = _auditCasePush.RejectPush(model.CaseNum);
            if (pushResult.IsSuccess == false)
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = new ErrorMessage[] { new ErrorMessage("", pushResult.Message) };
                return Json(response);
            }

            if (model.CaseStatus == CaseStatus.FirstAudit)
            {
                baudit.RejectType = model.RejectType;
                baudit.Description = model.Description;
                baudit.CreateTime = DateTime.Now;
                baudit.CreateUser = CurrentUser.UserName;
                ba.Update(baudit);
                ba.AcceptAllChange();

                model.CreateUser = CurrentUser.UserName;
                model.CreateTime = DateTime.Now;
                model.CaseStatus = CaseStatus.CloseCase;

                //if (model.BorrowerPerson != null)
                //{
                //    RelationStateBLLModel relationbllModel = new RelationStateBLLModel()
                //    {
                //        Number = model.BorrowerPerson.IdentificationNumber,
                //    };
                //    relationstatebll.UpdateLockRelationState(relationbllModel);
                //}
                //if (model.RelationPersonAudits != null)
                //{
                //    foreach (var item in model.RelationPersonAudits)
                //    {
                //        RelationStateBLLModel relationbllModel = new RelationStateBLLModel()
                //        {
                //            Number = item.IdentificationNumber,
                //        };
                //        relationstatebll.UpdateLockRelationState(relationbllModel);
                //    }
                //}
                //if (model.CollateralAudits != null)
                //{
                //    foreach (var item in model.CollateralAudits)
                //    {
                //        RelationStateBLLModel relationbllModel = new RelationStateBLLModel()
                //        {
                //            Number = item.HouseNumber,
                //        };
                //        relationstatebll.UpdateLockRelationState(relationbllModel);
                //    }
                //}

                if (ch.AddBaseAudit(model, "add") != null)
                {
                    response.Status = "Success";
                }
                else
                {
                    response.Status = "Failed";
                }
            }
            else if (model.CaseStatus == CaseStatus.SecondAudit)
            {
                baudit.LendingTerm = model.LendingTerm;
                baudit.ContractFileInfo = model.ContractFileInfo;
                baudit.AuditComment = model.AuditComment;
                baudit.AuditAmount = model.AuditAmount;
                baudit.AuditRate = model.AuditRate;
                baudit.AuditTerm = model.AuditTerm;
                baudit.RejectType = model.RejectType;
                baudit.Description = model.Description;
                baudit.CaseMode = model.CaseMode;
                baudit.ThirdParty = model.ThirdParty;
                baudit.MonthlyInterest = model.MonthlyInterest;
                baudit.LendingDate = model.LendingDate;
                baudit.PaymentDate = model.PaymentDate;
                baudit.ActualInterest = model.ActualInterest;
                baudit.AdvanceInterest = model.AdvanceInterest;
                //           baudit.CreateTime = DateTime.Now;
                //           baudit.CreateUser = CurrentUser.UserName;

                //if (model.BorrowerPerson != null)
                //{
                //    RelationStateBLLModel RelationbllModel = new RelationStateBLLModel()
                //    {
                //        Number = model.BorrowerPerson.IdentificationNumber,
                //    };
                //    relationstatebll.UpdateLockRelationState(RelationbllModel);
                //}
                //if (model.RelationPersonAudits != null)
                //{
                //    foreach (var item in model.RelationPersonAudits)
                //    {
                //        RelationStateBLLModel RelationbllModel = new RelationStateBLLModel()
                //        {
                //            Number = item.IdentificationNumber,
                //        };
                //        relationstatebll.UpdateLockRelationState(RelationbllModel);
                //    }
                //}
                //if (model.CollateralAudits != null)
                //{
                //    foreach (var item in model.CollateralAudits)
                //    {
                //        RelationStateBLLModel RelationbllModel = new RelationStateBLLModel()
                //        {
                //            Number = item.HouseNumber
                //        };
                //        relationstatebll.UpdateLockRelationState(RelationbllModel);
                //    }
                //}

                response.Status = ba.ReturnAudit(baudit, CurrentUser.UserName, CaseStatus.CloseCase) ? "Success" : "Failed";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 重载审批退回，审核退回调查
        /// </summary>
        /// <param name="model">案件信息</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReturnBaseAudit(BaseAuditViewModel model)
        {
            BaseResponse<BaseAuditViewModel> response = new BaseResponse<BaseAuditViewModel>();
            var baseAuditDal = new BaseAuditDAL();
            var entity = baseAuditDal.GetAuthorizeAndSelf(model.ID, CurrentUser);
            if (entity == null)
            {
                response.Status = "Failed";
                response.Message = new ErrorMessage[] { new ErrorMessage("权限", "权限不足") { } };
                return Json(response);
            }

            BaseAuditDAL ba = new BaseAuditDAL();

            var validateResult = new ReturnBaseAuditValidator().Validate(model);
            if (validateResult.IsNotValid())
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = validateResult.GetErrors().ToArray();

                return Json(response);
            }

            BaseAudit baudit = ba.Get(model.ID);
            if (baudit == null || !(baudit.CaseStatus == CaseStatus.SecondAudit || baudit.CaseStatus == CaseStatus.HatsPending))
            {
                response.Status = "Failed";
                response.Message = new[] { new ErrorMessage("Data", "数据过期") };
                response.Data = null;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            baudit.LendingTerm = model.LendingTerm;
            baudit.ContractFileInfo = model.ContractFileInfo;
            baudit.AuditComment = model.AuditComment;
            baudit.AuditAmount = model.AuditAmount;
            baudit.AuditRate = model.AuditRate;
            baudit.AuditTerm = model.AuditTerm;
            baudit.Description = model.Description;
            baudit.CaseMode = model.CaseMode;
            baudit.ThirdParty = model.ThirdParty;
            baudit.MonthlyInterest = model.MonthlyInterest;
            baudit.LendingDate = model.LendingDate;
            baudit.PaymentDate = model.PaymentDate;
            baudit.ActualInterest = model.ActualInterest;
            baudit.AdvanceInterest = model.AdvanceInterest;
            //     baudit.CreateTime = DateTime.Now;
            //     baudit.CreateUser = CurrentUser.UserName;

            var pushResult = RejectAuditCase(model);
            if (pushResult.Item1)
            {
                response.Status = ba.ReturnAudit(baudit, CurrentUser.UserName, CaseStatus.FirstAudit) ? "Success" : "Failed";
            }
            else
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = new[] { new ErrorMessage("", pushResult.Item2) };
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 推送到HATS
        /// </summary>
        /// <param name="audit"></param>
        private Tuple<bool, string> RejectAuditCase(BaseAuditViewModel audit)
        {
            string hatshost = ConfigurationManager.AppSettings["hats_host"];
            if (hatshost.IsNullOrEmpty())
            {
                throw new Exception("缺少hats_host配置节");
            }

            var securityRequest = new SecurityRequest()
            {
                RequestData = audit.CaseNum.ToHatsString()
            };

            var request = new HttpItem()
            {
                URL = string.Format("{0}/api/BaseAuditPush/RejectBaseAuditByHEAS", hatshost),
                Method = "post",
                ContentType = "application/json;charset=utf-8",
                Postdata = securityRequest.ToJson(),
                Accept = "text/json",
                PostEncoding = Encoding.UTF8
            };
            var httpresult = new HttpHelper().GetHtml(request);
            if (httpresult.StatusCode == HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<ResponseResult>(httpresult.Html);

                return new Tuple<bool, string>(result.IsSuccess, result.Message);
            }
            else
            {
                return new Tuple<bool, string>(false, "Hats调用接口出错");
            }
        }

        /// <summary>
        ///  二审通过(审核)
        /// </summary>
        /// <param name="model">二审</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApprovalBaseAudit(BaseAuditViewModel model)
        {
            var response = new BaseResponse<string>();
            var baseAuditDal = new BaseAuditDAL();
            var entity = baseAuditDal.GetAuthorizeAndSelf(model.ID, CurrentUser);
            if (entity == null)
            {
                response.Status = "Failed";
                response.Message = new ErrorMessage[] { new ErrorMessage("权限", "权限不足") { } };
                return Json(response);
            }

            #region 验证信息

            //if (!ModelState.IsValid)
            //{
            //    foreach (var e in ModelState.Keys)
            //    {
            //        if (ModelState[e].Errors.Any())
            //        {
            //            if (e.Contains("FileName")) continue;
            //            if (string.IsNullOrEmpty(ModelState[e].Errors[0].ErrorMessage)) continue;
            //            if (e.Contains("BorrowerPerson.RelationType")) continue;
            //            ErrorMessageAdd(e, ModelState[e].Errors[0].ErrorMessage, errors);
            //        }
            //    }
            //}
            var validator = new ApprovalBaseAuditValidator();
            var validatorResult = validator.Validate(model);

            if (validatorResult.IsNotValid())
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = validatorResult.GetErrors().ToArray();
                return Json(response);
            }

            #endregion 验证信息

            BaseAudit baseAudit = _baseAuditDal.Get(model.ID);

            if (baseAudit == null || baseAudit.CaseStatus != CaseStatus.SecondAudit)
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = new ErrorMessage[] { new ErrorMessage("Data", "数据过期") };
                response.Data = null;
                return Json(response);
            }
            // 审核信息
            //baseAudit.CaseMode = model.CaseMode;
            //baseAudit.ThirdParty = model.ThirdParty;
            baseAudit.MonthlyInterest = model.MonthlyInterest;

            baseAudit.LendingDate = model.LendingDate;
            //baseAudit.PaymentDate = model.PaymentDate;
            //baseAudit.ActualInterest = model.ActualInterest;
            //baseAudit.AdvanceInterest = model.AdvanceInterest;

            baseAudit.LendingTerm = model.LendingTerm;
            baseAudit.ContractFileInfo = model.ContractFileInfo;
            baseAudit.Description = model.Description;
            baseAudit.AuditComment = model.AuditComment;
            baseAudit.AuditAmount = model.AuditAmount;
            baseAudit.AuditRate = model.AuditRate;
            baseAudit.AuditTerm = model.AuditTerm;

            baseAudit.CaseDetail = model.CaseDetail;
            baseAudit.CreateTime = DateTime.Now;
            baseAudit.CreateUser = CurrentUser.UserName;

            var pushResult = PushAuditCase(model);
            if (pushResult.Item1)
            {
                response.Status = _baseAuditDal.ReturnAudit(baseAudit, CurrentUser.UserName, CaseStatus.HatsPending) ? "Success" : "Failed";
            }
            else
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = new[] { new ErrorMessage("", pushResult.Item2) };
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 错误提示集合
        /// </summary>
        /// <param name="key">错误提示键</param>
        /// <param name="message">错误提示信息</param>
        /// <param name="listError">错误集合</param>
        public void ErrorMessageAdd(string key, string message, List<ErrorMessage> listError)
        {
            var error = new ErrorMessage();
            error.Key = key;
            error.Message = message;
            listError.Add(error);
        }

        /// <summary>
        /// 推送到HATS
        /// </summary>
        /// <param name="audit"></param>
        private Tuple<bool, string> PushAuditCase(BaseAuditViewModel audit)
        {
            string hatshost = ConfigurationManager.AppSettings["hats_host"];
            if (hatshost.IsNullOrEmpty())
            {
                throw new Exception("缺少hats_host配置节");
            }

            var securityRequest = new SecurityRequest()
            {
                RequestData = audit.ToHatsString()
            };

            var request = new HttpItem()
            {
                URL = string.Format("{0}/api/BaseAuditPush/AddBaseAuditBy2Audit", hatshost),
                Method = "post",
                ContentType = "application/json;charset=utf-8",
                Postdata = securityRequest.ToJson(),
                Accept = "text/json",
                PostEncoding = Encoding.UTF8
            };
            var httpresult = new HttpHelper().GetHtml(request);
            if (httpresult.StatusCode == HttpStatusCode.OK)
            {
                var result = JsonConvert.DeserializeObject<ResponseResult>(httpresult.Html);

                return new Tuple<bool, string>(result.IsSuccess, result.Message);
            }
            else
            {
                return new Tuple<bool, string>(false, "Hats调用接口出错");
            }
        }

        /// <summary>
        /// 二审只读页面
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ReadonlyBaseAudit(string id)
        {
            ViewBag.ID = id;

            var bad = new BaseAuditDAL();
            var entity = bad.GetAuthorizeAndSelf(id, CurrentUser);
            if (entity == null)
            {
                return RedirectToAction("Failed", "Home");
            }
            var model = new BaseAuditViewModel().CastModel(bad.Get(id));
            if (model == null)
            {
                return RedirectToAction("Failed", "Home");
            }
            return View(model);
        }

        /// <summary>
        /// 审核拒绝只读页面
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ReadonlyRejectBaseAudit(string id)
        {
            ViewBag.ID = id;

            var bad = new BaseAuditDAL();
            var entity = bad.GetAuthorizeAndSelf(id, CurrentUser);
            if (entity == null)
            {
                return RedirectToAction("Failed", "Home");
            }
            var model = new BaseAuditViewModel().CastModel(bad.Get(id));
            if (model == null)
            {
                return RedirectToAction("Failed", "Home");
            }
            return View(model);
        }

        /// <summary>
        /// 获取进件的辅助对象
        /// </summary>
        public class AuditHelper
        {
            private BaseAuditDAL baseAuditDAL; //审核相关DAL
            private DictionaryDAL dicdal;
            private RelationPersonAuditDAL relationPersonAuditDAL; //关系人相关DAL
            private AddressAuditDAL addressAuditDAL; //关系人地址相关DAL
            private ContactAuditDAL contactAuditDAL; //关系人联系方式相关DAL
            private EmergencyContactAuditDAL emergencyContactAuditDAL; //关系人紧急联系人相关DAL
            private RelationEnterpriseAuditDAL relationEnterpriseAuditDAL; //关系人企业信息相关DAL

            private RelationStateDAL relationstatedal;//人员锁定表相关dal
           // private RelationStateBLL relationstatebll; // 人员锁定表相关blL

            private CollateralAuditDAL collateralAuditDAL; //抵押物相关DAL
            private IntroducerAuditDAL IntroducerAuditDAL; //介绍人相关DAL
            private GuarantorDAL guarantorDAL; //担保人相关DAL
            private IndividualCreditDAL individualCreditDAL; //个人资信相关DAL
            private IndustryCommerceTaxDAL industryCommerceTaxDAL; //企业资信相关DAL
            private EnforcementPersonDAL enforcementPersonDAL; //被执行人相关DAL
            private EnterpriseCreditDAL enterpriseCreditDAL; //工商税务相关DAL

            private HouseDetailDAL houseDetailDAL; //房屋详细相关DAL
            private EstimateSourceDAL estimateSourceDAL; //房屋评估来源相关DAL

            public AuditHelper()
            {
                baseAuditDAL = new BaseAuditDAL();
                dicdal = new DictionaryDAL();
                relationPersonAuditDAL = new RelationPersonAuditDAL();
                addressAuditDAL = new AddressAuditDAL();
                contactAuditDAL = new ContactAuditDAL();
                emergencyContactAuditDAL = new EmergencyContactAuditDAL();
                enforcementPersonDAL = new EnforcementPersonDAL();
                enterpriseCreditDAL = new EnterpriseCreditDAL();
                relationstatedal = new RelationStateDAL();
               // relationstatebll = new RelationStateBLL();
                collateralAuditDAL = new CollateralAuditDAL();
                guarantorDAL = new GuarantorDAL();

                houseDetailDAL = new HouseDetailDAL();
                estimateSourceDAL = new EstimateSourceDAL();

                individualCreditDAL = new IndividualCreditDAL();
                industryCommerceTaxDAL = new IndustryCommerceTaxDAL();
                relationEnterpriseAuditDAL = new RelationEnterpriseAuditDAL();

                IntroducerAuditDAL = new IntroducerAuditDAL();
            }

            public BaseAuditViewModel AddBaseAudit(BaseAuditViewModel model, string flag)
            {
                if (model == null)
                {
                    return null;
                }
                var baseAudit = GetBaseAuditAsync(model, flag);
                if (model.RelationPersonAudits != null && model.RelationPersonAudits.Any())
                {
                    var relationperson = model.RelationPersonAudits.Where(x => x.RelationType == "-PersonType-JieKuanRen").Select(x => x.Name);
                    if (relationperson != null && relationperson.Any())
                    {
                        baseAudit.BorrowerName = relationperson.First();
                    }
                    else
                    {
                        baseAudit.BorrowerName = model.RelationPersonAudits.First().Name;
                    }
                }

                var relationPersonAudits = GetRelationPersonAuditsAsync(model);
                var ContactAudits = GetContactAuditsAsync(model);
                var relationEnterpriseAudits = GetRelationEnterpriseAuditsAsync(model);
                var emergencyContactAudits = GetEmergencyContactAuditsAsync(model);
                var addressAudits = GetAddressAuditsAsync(model);
                var IntroducerAudits = GetIntroducerAuditsAsync(model);

                var collateralAudits = GetCollateralAuditsAsync(model);
                var enforcementPerson = GetEnforcementPersonAsync(model);
                var enterpriseCredit = GetEnterpriseCreditAsync(model);
                var guarantor = GetGuarantorAsync(model);

                var houseDetail = GetHouseDetailAsync(model);
                var estimateSource = GetEstimateSourceAsync(model);

                var individualCredit = GetIndividualCreditAsync(model);
                var industryCommerceTax = GetIndustryCommerceTaxAsync(model);

                baseAuditDAL.Add(baseAudit);

                relationPersonAuditDAL.AddRange(relationPersonAudits);
                relationEnterpriseAuditDAL.AddRange(relationEnterpriseAudits);
                contactAuditDAL.AddRange(ContactAudits);
                emergencyContactAuditDAL.AddRange(emergencyContactAudits);
                addressAuditDAL.AddRange(addressAudits);

                collateralAuditDAL.AddRange(collateralAudits);
                enforcementPersonDAL.AddRange(enforcementPerson);
                enterpriseCreditDAL.AddRange(enterpriseCredit);
                guarantorDAL.AddRange(guarantor);

                houseDetailDAL.AddRange(houseDetail);
                estimateSourceDAL.AddRange(estimateSource);

                individualCreditDAL.AddRange(individualCredit);
                industryCommerceTaxDAL.AddRange(industryCommerceTax);
                IntroducerAuditDAL.AddRange(IntroducerAudits);

                SaveFiles(model, "add"); //保存附件信息
                //事务性提交
                baseAuditDAL.AcceptAllChange();
                return model;
            }

            /// <summary>
            /// 新增文件信息
            /// </summary>
            /// <param name="baseaudit">审核信息</param>
            public void SaveFiles(BaseAuditViewModel baseaudit, string flag)
            {
                FileUpload up = new FileUpload();
                //三个报告
                if (!string.IsNullOrEmpty(baseaudit.FaceReportFile))
                {
                    SaveFile(baseaudit.ID, baseaudit.ID, baseaudit.FaceReportFile, flag);
                }
                if (!string.IsNullOrEmpty(baseaudit.FieldReportFile))
                {
                    SaveFile(baseaudit.ID, baseaudit.ID, baseaudit.FieldReportFile, flag);
                }
                if (!string.IsNullOrEmpty(baseaudit.LoanDetailReportFile))
                {
                    SaveFile(baseaudit.ID, baseaudit.ID, baseaudit.LoanDetailReportFile, flag);
                }
                //关系人的文件上传
                if (baseaudit.RelationPersonAudits != null && baseaudit.RelationPersonAudits.Any())
                {
                    foreach (var model in baseaudit.RelationPersonAudits)
                    {
                        //证件复印件
                        if (!string.IsNullOrEmpty(model.IdentificationFile))
                        {
                            SaveFile(model.ID, baseaudit.ID, model.IdentificationFile, flag);
                        }
                        //结婚证
                        if (!string.IsNullOrEmpty(model.MarryFile))
                        {
                            SaveFile(model.ID, baseaudit.ID, model.MarryFile, flag);
                        }
                        //收入证明(授薪人士)
                        if (!string.IsNullOrEmpty(model.SalaryPersonFile))
                        {
                            SaveFile(model.ID, baseaudit.ID, model.SalaryPersonFile, flag);
                        }
                        //收入证明(自雇有执照)
                        if (!string.IsNullOrEmpty(model.SelfLicenseFile))
                        {
                            SaveFile(model.ID, baseaudit.ID, model.SelfLicenseFile, flag);
                        }
                        //收入证明(自雇无执照)
                        if (!string.IsNullOrEmpty(model.SelfNonLicenseFile))
                        {
                            SaveFile(model.ID, baseaudit.ID, model.SelfNonLicenseFile, flag);
                        }
                        //单身证明
                        if (!string.IsNullOrEmpty(model.SingleFile))
                        {
                            SaveFile(model.ID, baseaudit.ID, model.SingleFile, flag);
                        }
                        //出生证
                        if (!string.IsNullOrEmpty(model.BirthFile))
                        {
                            SaveFile(model.ID, baseaudit.ID, model.BirthFile, flag);
                        }
                        //户口本
                        if (!string.IsNullOrEmpty(model.AccountFile))
                        {
                            SaveFile(model.ID, baseaudit.ID, model.AccountFile, flag);
                        }
                    }
                }
                //抵押物
                if (baseaudit.CollateralAudits != null && baseaudit.CollateralAudits.Any())
                {
                    foreach (var model in baseaudit.CollateralAudits)
                    {
                        if (!string.IsNullOrEmpty(model.HouseFile))
                        {
                            SaveFile(model.ID, baseaudit.ID, model.HouseFile, flag);
                        }
                        if (!string.IsNullOrEmpty(model.HouseReportFile))
                        {
                            SaveFile(model.ID, baseaudit.ID, model.HouseReportFile, flag);
                        }
                    }
                }

                //个人资信
                if (baseaudit.IndividualCredits != null && baseaudit.IndividualCredits.Any())
                {
                    foreach (var model in baseaudit.IndividualCredits)
                    {
                        if (!string.IsNullOrEmpty(model.BankFlowFile))
                        {
                            SaveFile(model.ID, baseaudit.ID, model.BankFlowFile, flag);
                        }
                        if (!string.IsNullOrEmpty(model.IndividualFile))
                        {
                            SaveFile(model.ID, baseaudit.ID, model.IndividualFile, flag);
                        }
                    }
                }

                // 被执行对象情况列表
                if (baseaudit.EnforcementPersons != null && baseaudit.EnforcementPersons.Any())
                {
                    foreach (var person in baseaudit.EnforcementPersons)
                    {
                        if (person.AttachmentFile.IsNotNullOrEmpty())
                        {
                            SaveFile(person.ID, baseaudit.ID, person.AttachmentFile, flag);
                        }
                    }
                }
                //房屋明细
                if (baseaudit.CollateralAudits != null && baseaudit.CollateralAudits.Any())
                {
                    foreach (var model in baseaudit.CollateralAudits)
                    {
                        if (!string.IsNullOrEmpty(model.HouseFile))
                        {
                            SaveFile(model.ID, baseaudit.ID, model.HouseFile, flag);
                        }
                        if (!string.IsNullOrEmpty(model.HouseReportFile))
                        {
                            SaveFile(model.ID, baseaudit.ID, model.HouseReportFile, flag);
                        }
                    }
                }
            }

            private void SaveFile(string linkId, string linkkey, string filenames, string flag)
            {
                var up = new FileUpload(); //文件上传
                foreach (var file in filenames.Split(','))
                {
                    var filemodel = up.Single(new Guid(file));
                    filemodel.LinkID = new Guid(linkId);
                    filemodel.LinkKey = linkkey;
                    if (flag != "update")
                    {
                        var entity = CopyFile(filemodel);
                        up.SaveFileDescription(entity);
                    }
                    else
                    {
                        up.SaveFileDescription(filemodel);
                    }
                }
            }

            private FileDescription CopyFile(FileDescription model)
            {
                var entity = new FileDescription();
                ObjectExtend.CopyTo(model, entity);
                entity.ID = Guid.NewGuid();
                entity.FileCreateTime = DateTime.Now;
                return entity;
            }

            //获取审核基础信息
            public BaseAudit GetBaseAuditAsync(BaseAuditViewModel baseaudit, string flag)
            {
                int i = 0;
                //var basedb=  new BaseAuditViewModel().CastDB(baseaudit);
                //var basedb = new BaseAudit();
                //Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(baseaudit, basedb);
                // baseaudit.CopyTo(basedb);
                //basedb.ID = flag == "update" ? baseaudit.ID : Guid.NewGuid().ToString();
                // basedb.IsNeedReport = int.TryParse(baseaudit.IsNeedReport, out i) ? i : 0;

                #region

                var baseAudit = new BaseAudit()
                {
                    ID = flag == "update" ? baseaudit.ID : Guid.NewGuid().ToString(), //如果是修改就用以前的guid,新增则产生新的guid
                    BorrowerName = baseaudit.BorrowerName,
                    DistrictID = baseaudit.DistrictID,
                    SalesGroupID = baseaudit.SalesGroupID,
                    SalesID = baseaudit.SalesID,
                    CaseNum = baseaudit.CaseNum.Split('-')[1],
                    NewCaseNum = baseaudit.CaseNum,
                    CaseType = baseaudit.CaseType,
                    CreateTime = baseaudit.CreateTime,
                    CreateUser = baseaudit.CreateUser,
                    LoanAmount = baseaudit.LoanAmount,
                    CaseStatus = baseaudit.CaseStatus,
                    FaceReportFile = baseaudit.FaceReportFile,
                    FieldReportFile = baseaudit.FieldReportFile,
                    IsNeedReport = int.TryParse(baseaudit.IsNeedReport, out i) ? i : 0,
                    LoanDetailReportFile = baseaudit.LoanDetailReportFile,
                    Version = baseaudit.Version,
                    ServiceCharge = baseaudit.ServiceCharge,
                    ServiceChargeRate = baseaudit.ServiceChargeRate,
                    Deposit = baseaudit.Deposit,
                    DepositDate = baseaudit.DepositDate,
                    IsActivitieRate = baseaudit.IsActivitieRate,
                    // 新增字段
                    AnnualRate = baseaudit.AnnualRate,
                    AuditAmount = baseaudit.AuditAmount,
                    AuditRate = baseaudit.AuditRate,
                    AuditTerm = baseaudit.AuditTerm,
                    BankCard = baseaudit.BankCard,
                    CaseDetail = baseaudit.CaseDetail,
                    ComprehensiveRate = baseaudit.ComprehensiveRate,
                    MortgageOrder = baseaudit.MortgageOrder,
                    OpeningBank = baseaudit.OpeningBank,
                    OpeningSite = baseaudit.OpeningSite,
                    PlatformCharge = baseaudit.PlatformCharge,
                    Term = baseaudit.Term,
                    //Description = baseaudit.Description,
                    AuditComment = baseaudit.AuditComment,

                    #region 2016-06-27 再次新增

                    //跟单人
                    Merchandiser = baseaudit.Merchandiser,
                    //保证金
                    EarnestMoney = baseaudit.EarnestMoney,
                    //外访费（下户费）
                    OutboundCost = baseaudit.OutboundCost,
                    //代收公证费用
                    DebitNotarizationCost = baseaudit.DebitNotarizationCost,
                    //代收评估费
                    DebitEvaluationCost = baseaudit.DebitEvaluationCost,
                    //代收担保费
                    DebitGuaranteeCost = baseaudit.DebitGuaranteeCost,
                    //代收保险费
                    DebitInsuranceCost = baseaudit.DebitInsuranceCost,
                    //代收其他
                    DebitOtherCost = baseaudit.DebitOtherCost,
                    //公司承担的公证费
                    LevyNotarizationCost = baseaudit.LevyNotarizationCost,
                    //公司承担的产调费
                    LevyAssetsSurveyCost = baseaudit.LevyAssetsSurveyCost,
                    //公司承担的信用报告费
                    LevyCreditReportCost = baseaudit.LevyCreditReportCost,
                    //公司承担的其他费用
                    LevyOtherCost = baseaudit.LevyOtherCost,
                    //新增审核字段
                    //案件模式
                    CaseMode = baseaudit.CaseMode,
                    //第三方平台
                    ThirdParty = baseaudit.ThirdParty,

                    ThirdPartyAuditAmount = baseaudit.ThirdPartyAuditAmount,
                    ThirdPartyAuditTerm = baseaudit.ThirdPartyAuditTerm,
                    ThirdPartyAuditRate = baseaudit.ThirdPartyAuditAmount,

                    //月利息金额
                    MonthlyInterest = baseaudit.MonthlyInterest,
                    //放款日期
                    LendingDate = baseaudit.LendingDate,
                    //回款日期
                    PaymentDate = baseaudit.PaymentDate,
                    //实收利息（不退客户）
                    ActualInterest = baseaudit.ActualInterest,
                    //预收利息（可退客户）
                    AdvanceInterest = baseaudit.AdvanceInterest,
                    //还款来源
                    PaymentFactor = baseaudit.PaymentFactor,
                    //  借款用途
                    Purpose = baseaudit.Purpose,
                    //放款条件
                    LendingTerm = baseaudit.LendingTerm,
                    //签约要件
                    ContractFileInfo = baseaudit.ContractFileInfo,
                    //拒绝理由，拒绝批注使用Description
                    RejectReason = baseaudit.RejectReason,
                    RejectType = baseaudit.RejectType,
                    //借款申请书
                    LoanProposedFile = baseaudit.LoanProposedFile,
                    //客户保证金
                    CustEarnestMoney = baseaudit.CustEarnestMoney
                    #endregion 2016-06-27 再次新增

                };

                #endregion
                baseaudit.ID = baseAudit.ID;
                //先处理 借款人
                //if (baseaudit.BorrowerPerson != null)
                //{
                //    baseaudit.BorrowerPerson.RelationType = "-PersonType-JieKuanRen";
                //    basedb.BorrowerName = baseaudit.BorrowerPerson.Name;
                //    if (baseaudit.RelationPersonAudits == null)
                //    {
                //        baseaudit.RelationPersonAudits = new List<RelationPersonAuditViewModel>();
                //    }
                //    baseaudit.RelationPersonAudits.Add(baseaudit.BorrowerPerson);
                //}

                return baseAudit;
            }

            //联系人信息
            public List<RelationPersonAudit> GetRelationPersonAuditsAsync(BaseAuditViewModel baseaudit)
            {
                var personlist = new List<RelationPersonAudit>();
                if (baseaudit.RelationPersonAudits != null && baseaudit.RelationPersonAudits.Any())
                {
                    foreach (var model in baseaudit.RelationPersonAudits)
                    {
                        if (model == null) break;
                        var personmodel = new RelationPersonAudit()
                        {
                            ID = Guid.NewGuid().ToString(),
                            Birthday = model.Birthday,
                            AuditID = baseaudit.ID,
                            BorrowerRelation = model.BorrowerRelation,
                            ExpiryDate = model.ExpiryDate,
                            MaritalStatus = model.MaritalStatus,
                            Name = model.Name,
                            AccountFile = model.AccountFile,
                            BirthFile = model.BirthFile,
                            IdentificationFile = model.IdentificationFile,
                            IdentificationNumber = model.IdentificationNumber,
                            IdentificationType = model.IdentificationType,
                            MarryFile = model.MarryFile,
                            RelationType = model.RelationType,
                            SalaryDescription = model.SalaryDescription,
                            SalaryPersonFile = model.SalaryPersonFile,
                            SelfLicenseFile = model.SelfLicenseFile,
                            SelfNonLicenseFile = model.SelfNonLicenseFile,
                            SingleFile = model.SingleFile,
                            IsCoBorrower = model.IsCoBorrower,
                            Warranty = model.Warranty,
                            BankFlowFile = model.BankFlowFile,
                            IndividualFile = model.IndividualFile,
                            OtherFile = model.OtherFile,
                            Sequence = model.Sequence
                        };
                        if (baseaudit.IndividualCredits != null)
                        {
                            var indv = baseaudit.IndividualCredits.Where(t => t.PersonID == model.ID);
                            if (indv.Any())
                            {
                                foreach (var t in indv)
                                {
                                    t.PersonID = personmodel.ID;
                                }
                            }
                        }
                        if (baseaudit.EnforcementPersons != null)
                        {
                            var eforc = baseaudit.EnforcementPersons.Where(t => t.PersonID == model.ID);
                            if (eforc.Any())
                            {
                                foreach (var t in eforc)
                                {
                                    t.PersonID = personmodel.ID;
                                }
                            }
                        }
                        model.ID = personmodel.ID;
                        personlist.Add(personmodel);
                    }
                }

                return personlist;
            }

            //获取所有地址信息
            public List<AddressAudit> GetAddressAuditsAsync(BaseAuditViewModel baseaudit)
            {
                var addresslist = new List<AddressAudit>();
                if (baseaudit.RelationPersonAudits != null && baseaudit.RelationPersonAudits.Any())
                {
                    foreach (var person in baseaudit.RelationPersonAudits)
                    {
                        if (person.AddressAudits != null && person.AddressAudits.Any())
                        {
                            foreach (var model in person.AddressAudits)
                            {
                                if (model == null) break;
                                var address = new AddressAudit()
                                {
                                    ID = Guid.NewGuid().ToString(),
                                    AddressInfo = model.AddressInfo,
                                    AddressType = model.AddressType,
                                    PersonID = person.ID,
                                    IsDefault = model.IsDefault,
                                    Sequence = model.Sequence
                                };
                                model.ID = address.ID;
                                addresslist.Add(address);
                            }
                        }
                    }
                }
                return addresslist;
            }

            //获取所有联系人的联系方式
            public List<ContactAudit> GetContactAuditsAsync(BaseAuditViewModel baseaudit)
            {
                var contactlist = new List<ContactAudit>();
                if (baseaudit.RelationPersonAudits != null && baseaudit.RelationPersonAudits.Any())
                {
                    foreach (var person in baseaudit.RelationPersonAudits)
                    {
                        if (person.ContactAudits != null && person.ContactAudits.Any())
                        {
                            foreach (var model in person.ContactAudits)
                            {
                                if (model == null) break;
                                var contact = new ContactAudit()
                                {
                                    ID = Guid.NewGuid().ToString(),
                                    ContactNumber = model.ContactNumber,
                                    ContactType = model.ContactType,
                                    PersonID = person.ID,
                                    IsDefault = model.IsDefault,
                                    Sequence = model.Sequence
                                };
                                model.ID = contact.ID;
                                contactlist.Add(contact);
                            }
                        }
                    }
                }
                return contactlist;
            }

            //获取所有紧急联系方式
            public List<EmergencyContactAudit> GetEmergencyContactAuditsAsync(BaseAuditViewModel baseaudit)
            {
                var emergencyContactlist = new List<EmergencyContactAudit>();
                if (baseaudit.RelationPersonAudits != null && baseaudit.RelationPersonAudits.Any())
                {
                    foreach (var person in baseaudit.RelationPersonAudits)
                    {
                        if (person.EmergencyContactAudits != null && person.EmergencyContactAudits.Any())
                        {
                            foreach (var model in person.EmergencyContactAudits)
                            {
                                if (model == null) break;
                                var emergencyContact = new EmergencyContactAudit()
                                {
                                    ID = Guid.NewGuid().ToString(),
                                    ContactNumber = model.ContactNumber,
                                    ContactType = model.ContactType,
                                    PersonID = person.ID,
                                    Name = model.Name,
                                    Sequence = model.Sequence
                                };
                                model.ID = emergencyContact.ID;
                                emergencyContactlist.Add(emergencyContact);
                            }
                        }
                    }
                }
                return emergencyContactlist;
            }

            //获取联系人联系单位

            public List<RelationEnterpriseAudit> GetRelationEnterpriseAuditsAsync(BaseAuditViewModel baseaudit)
            {
                var enterpriselist = new List<RelationEnterpriseAudit>();
                if (baseaudit.RelationPersonAudits != null && baseaudit.RelationPersonAudits.Any())
                {
                    foreach (var person in baseaudit.RelationPersonAudits)
                    {
                        if (person.RelationEnterpriseAudits != null && person.RelationEnterpriseAudits.Any())
                        {
                            foreach (var model in person.RelationEnterpriseAudits)
                            {
                                if (model == null) break;
                                var enterprise = new RelationEnterpriseAudit()
                                {
                                    ID = Guid.NewGuid().ToString(),
                                    Address = model.Address,
                                    EnterpriseDes = model.EnterpriseDes,
                                    EnterpriseName = model.EnterpriseName,
                                    LegalPerson = model.LegalPerson,
                                    MainBusiness = model.MainBusiness,
                                    PersonID = person.ID,
                                    RegisteredCapital = model.RegisteredCapital,
                                    RegisterNumber = model.RegisterNumber,
                                    ShareholderDetails = model.ShareholderDetails,
                                    BankFlowFile = model.BankFlowFile,
                                    IndividualFile = model.IndividualFile,
                                    Sequence = model.Sequence
                                };
                                if (baseaudit.EnterpriseCredits != null)
                                {
                                    var entCR = baseaudit.EnterpriseCredits.Where(t => t.EnterpriseID == model.ID);
                                    if (entCR.Any())
                                    {
                                        foreach (var t in entCR)
                                        {
                                            t.EnterpriseID = enterprise.ID;
                                        }
                                    }
                                }
                                if (baseaudit.IndustryCommerceTaxs != null)
                                {
                                    var indTax = baseaudit.IndustryCommerceTaxs.Where(t => t.EnterpriseID == model.ID);
                                    if (indTax.Any())
                                    {
                                        foreach (var t in indTax)
                                        {
                                            t.EnterpriseID = enterprise.ID;
                                        }
                                    }
                                }
                                model.ID = enterprise.ID;
                                enterpriselist.Add(enterprise);
                            }
                        }
                    }
                }
                return enterpriselist;
            }

            //获取所有抵押物
            public List<CollateralAudit> GetCollateralAuditsAsync(BaseAuditViewModel baseaudit)
            {
                var collaterallist = new List<CollateralAudit>();
                if (baseaudit.CollateralAudits != null && baseaudit.CollateralAudits.Any())
                {
                    foreach (var model in baseaudit.CollateralAudits)
                    {
                        if (model == null) break;
                        var collateral = new CollateralAudit()
                        {
                            ID = Guid.NewGuid().ToString(),
                            Address = model.Address,
                            CollateralType = model.CollateralType,
                            BuildingName = model.BuildingName,
                            AuditID = baseaudit.ID,
                            HouseFile = model.HouseFile,
                            HouseNumber = model.HouseNumber,
                            HouseSize = model.HouseSize,
                            TotalHeight = model.TotalHeight,
                            RightOwner = model.RightOwner,
                            HouseReportFile = model.HouseReportFile,
                            Sequence = model.Sequence,
                            CompletionDate = model.CompletionDate,
                            LandType = model.LandType,
                            HouseType = model.HouseType
                        };
                        if (baseaudit.HouseDetails != null)
                        {
                            var house = baseaudit.HouseDetails.Where(t => t.CollateralID == model.ID);
                            if (house.Any())
                            {
                                foreach (var t in house)
                                    t.CollateralID = collateral.ID;
                            }
                        }
                        model.ID = collateral.ID;
                        collaterallist.Add(collateral);
                    }
                }
                return collaterallist;
            }

            //获取所有介绍人
            public List<IntroducerAudit> GetIntroducerAuditsAsync(BaseAuditViewModel baseaudit)
            {
                var IntroducerList = new List<IntroducerAudit>();
                if (baseaudit.Introducer != null && baseaudit.Introducer.Any())
                {
                    foreach (var model in baseaudit.Introducer)
                    {
                        if (model == null) break;
                        var Introducer = new IntroducerAudit()
                        {
                            ID = Guid.NewGuid().ToString(),
                            Account = model.Account,
                            AccountBank = model.AccountBank,
                            Contract = model.Contract,
                            RebateRate = model.RebateRate,
                            Name = model.Name,
                            RebateAmmount = model.RebateAmmount,
                            AuditID = baseaudit.ID,
                            Sequence = model.Sequence
                        };
                        model.ID = Introducer.ID;
                        IntroducerList.Add(Introducer);
                    }
                }
                return IntroducerList;
            }

            //获取被执行人情况
            public List<EnforcementPerson> GetEnforcementPersonAsync(BaseAuditViewModel baseaudit)
            {
                var enforcementPersonlist = new List<EnforcementPerson>();
                if (baseaudit.EnforcementPersons != null && baseaudit.EnforcementPersons.Any())
                {
                    foreach (var model in baseaudit.EnforcementPersons)
                    {
                        if (model == null) break;
                        var enforcementPersonModel = new EnforcementPerson()
                        {
                            ID = Guid.NewGuid().ToString(),
                            EnforcementWeb = model.EnforcementWeb,
                            BaseAuditID = baseaudit.ID,
                            TrialRecord = model.TrialRecord,
                            LawXP = model.LawXP,
                            ShiXin = model.ShiXin,
                            BadNews = model.BadNews,
                            PersonID = model.PersonID,
                            Sequence = model.Sequence,
                            AttachmentFile = model.AttachmentFile
                        };
                        model.ID = enforcementPersonModel.ID;
                        enforcementPersonlist.Add(enforcementPersonModel);
                    }
                }
                return enforcementPersonlist;
            }

            //获取企业资信情况
            public List<EnterpriseCredit> GetEnterpriseCreditAsync(BaseAuditViewModel baseaudit)
            {
                var enterpriseCreditlist = new List<EnterpriseCredit>();
                if (baseaudit.EnterpriseCredits != null && baseaudit.EnterpriseCredits.Any())
                {
                    foreach (var model in baseaudit.EnterpriseCredits)
                    {
                        if (model == null) break;
                        var enterpriseCreditModel = new EnterpriseCredit()
                        {
                            ID = Guid.NewGuid().ToString(),
                            CreditCard = model.CreditCard,
                            BaseAuditID = baseaudit.ID,
                            CreditInfo = model.CreditInfo,
                            EnterpriseID = model.EnterpriseID,
                            OtherDetailes = model.OtherDetailes,
                            ShareholderDetails = model.ShareholderDetails,
                            Sequence = model.Sequence
                        };
                        model.ID = enterpriseCreditModel.ID;
                        enterpriseCreditlist.Add(enterpriseCreditModel);
                    }
                }
                return enterpriseCreditlist;
            }

            //获取担保人情况
            public List<Guarantor> GetGuarantorAsync(BaseAuditViewModel baseaudit)
            {
                var guarantorlist = new List<Guarantor>();
                if (baseaudit.Guarantors != null && baseaudit.Guarantors.Any())
                {
                    foreach (var model in baseaudit.Guarantors)
                    {
                        if (model == null) break;
                        var guarantorsModel = new Guarantor()
                        {
                            ID = Guid.NewGuid().ToString(),
                            Name = model.Name,
                            BaseAuditID = baseaudit.ID,
                            ContactNumber = model.ContactNumber,
                            RelationType = model.RelationType,
                            GuarantType = model.GuarantType,
                            IdentityType = model.IdentityType,
                            IdentityNumber = model.IdentityNumber,
                            Address = model.Address,
                            MarriedInfo = model.MarriedInfo,
                            Sequence = model.Sequence
                        };
                        model.ID = guarantorsModel.ID;
                        guarantorlist.Add(guarantorsModel);
                    }
                }
                return guarantorlist;
            }

            #region 获取房屋相关

            //获取房屋明细
            public List<HouseDetail> GetHouseDetailAsync(BaseAuditViewModel baseaudit)
            {
                var houseDetaillist = new List<HouseDetail>();
                if (baseaudit.HouseDetails != null && baseaudit.HouseDetails.Any())
                {
                    foreach (var model in baseaudit.HouseDetails)
                    {
                        if (model == null) break;

                        var houseDetailModel = new HouseDetail()
                        //Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, houseDetailModel);
                        //houseDetailModel.ID = Guid.NewGuid().ToString();
                        //houseDetailModel.BaseAuditID = baseaudit.ID;
                        {
                            ID = Guid.NewGuid().ToString(),
                            AssessedValue = model.AssessedValue,
                            BaseAuditID = baseaudit.ID,
                            Accout = model.Accout,
                            TotalHeight = model.TotalHeight,
                            CompletionDate = model.CompletionDate,
                            HouseType = model.HouseType,
                            ServiceCondition = model.ServiceCondition,
                            RepairSituation = model.RepairSituation,
                            Collateral = model.Collateral,
                            LimitInfo = model.LimitInfo,
                            Description = model.Description,
                            CollateralID = model.CollateralID,
                            HousePhotoFile = model.HousePhotoFile,
                            HouseReportFile = model.HouseReportFile,
                            //LandType = model.LandType,
                            Sequence = model.Sequence,

                            #region 2016-9-8 大改添加
                            RealHigh = model.RealHigh,
                            IsDamage = model.IsDamage,
                            RealResident = model.RealResident,
                            WaterPaymentCheck = model.WaterPaymentCheck,
                            TaxPaymentCheck = model.TaxPaymentCheck,
                            Man2Wei1 = model.Man2Wei1,
                            SpecialResident = model.SpecialResident,
                            OtherDescription = model.OtherDescription,
                            SchoolInfo = model.SchoolInfo,
                            HospitalInfo = model.HospitalInfo,
                            TrafficInfo = model.TrafficInfo,
                            Supermarket = model.Supermarket,
                            Recreation = model.Recreation,
                            NegativeSite = model.NegativeSite,
                            VillagePhotoFile = model.VillagePhotoFile,
                            MainGatePhotoFile = model.MainGatePhotoFile,
                            ParlourPhotoFile = model.ParlourPhotoFile,
                            BedroomPhotoFile = model.BedroomPhotoFile,
                            KitchenRoomPhotoFile = model.KitchenRoomPhotoFile,
                            ToiletPhotoFile = model.ToiletPhotoFile,
                            #endregion
                        };
                        model.ID = houseDetailModel.ID;
                        houseDetaillist.Add(houseDetailModel);
                    }
                }
                return houseDetaillist;
            }

            //删除房屋信息及其字表
            protected void DeleteHouseDetail(IEnumerable<HouseDetail> detaillist)
            {
                foreach (var model in detaillist)
                {
                    //存在子表信息先删除
                    var estimateSourcelist = estimateSourceDAL.FindByHouseDetailID(model.ID);
                    estimateSourceDAL.DeleteRange(estimateSourcelist); //删除房屋估价来源
                }
                houseDetailDAL.DeleteRange(detaillist);
            }

            //获取房屋估价来源
            public List<EstimateSource> GetEstimateSourceAsync(BaseAuditViewModel baseaudit)
            {
                var estimateSourcelist = new List<EstimateSource>();
                if (baseaudit.HouseDetails != null && baseaudit.HouseDetails.Any())
                {
                    foreach (var houseDetail in baseaudit.HouseDetails)
                    {
                        if (houseDetail.EstimateSources != null && houseDetail.EstimateSources.Any())
                        {
                            foreach (var model in houseDetail.EstimateSources)
                            {
                                if (model == null) break;
                                var estimateSourceModel = new EstimateSource()
                                {
                                    ID = Guid.NewGuid().ToString(),
                                    EstimateInstitutions = model.EstimateInstitutions,
                                    RushEstimate = model.RushEstimate,
                                    InformationProvider = model.InformationProvider,
                                    ContactNumber = model.ContactNumber,
                                    HouseDetailID = houseDetail.ID,
                                    Sequence = model.Sequence,
                                    CertificateFile = model.CertificateFile
                                };
                                model.ID = estimateSourceModel.ID;
                                estimateSourcelist.Add(estimateSourceModel);
                            }
                        }
                    }
                }
                return estimateSourcelist;
            }

            #endregion 获取房屋相关

            // 获取个人资信情况
            public List<IndividualCredit> GetIndividualCreditAsync(BaseAuditViewModel baseaudit)
            {
                var individualCreditlist = new List<IndividualCredit>();
                if (baseaudit.IndividualCredits != null && baseaudit.IndividualCredits.Any())
                {
                    foreach (var model in baseaudit.IndividualCredits)
                    {
                        if (model == null) break;
                        var individualCreditModel = new IndividualCredit()
                        {
                            ID = Guid.NewGuid().ToString(),
                            CreditCard = model.CreditCard,
                            BaseAuditID = baseaudit.ID,
                            CreditInfo = model.CreditInfo,
                            OtherCredit = model.OtherCredit,
                            OverdueInfo = model.OverdueInfo,
                            IndividualFile = model.IndividualFile,
                            BankFlowFile = model.BankFlowFile,
                            PersonID = model.PersonID,
                            Sequence = model.Sequence
                        };
                        model.ID = individualCreditModel.ID;
                        individualCreditlist.Add(individualCreditModel);
                    }
                }
                return individualCreditlist;
            }

            //获取工商税务情况
            public List<IndustryCommerceTax> GetIndustryCommerceTaxAsync(BaseAuditViewModel baseaudit)
            {
                var industryCommerceTaxlist = new List<IndustryCommerceTax>();
                if (baseaudit.IndustryCommerceTaxs != null && baseaudit.IndustryCommerceTaxs.Any())
                {
                    foreach (var model in baseaudit.IndustryCommerceTaxs)
                    {
                        if (model == null) break;
                        var industryCommerceTaxModel = new IndustryCommerceTax()
                        {
                            ID = Guid.NewGuid().ToString(),
                            AnnualInspection = model.AnnualInspection,
                            BaseAuditID = baseaudit.ID,
                            ActualManagement = model.ActualManagement,
                            Description = model.Description,
                            EnterpriseID = model.EnterpriseID,
                            Sequence = model.Sequence
                        };
                        model.ID = industryCommerceTaxModel.ID;
                        industryCommerceTaxlist.Add(industryCommerceTaxModel);
                    }
                }
                return industryCommerceTaxlist;
            }

            public bool UpdateBaseAudit(BaseAuditViewModel model, string flag)
            {
                if (model == null)
                {
                    return false;
                }
                //获取基础信息
                var baseAudit = GetBaseAuditAsync(model, flag);

                var relationPersonAudit = GetRelationPersonAuditsAsync(model); //联系人信息
                var contactAudit = GetContactAuditsAsync(model); //获取所有联系人的联系方式
                var relationEnterpriseAudit = GetRelationEnterpriseAuditsAsync(model); //获取联系人联系单位
                var emergencyContactAudit = GetEmergencyContactAuditsAsync(model); //获取所有紧急联系方式
                var addressAudit = GetAddressAuditsAsync(model); //获取所有地址信息

                var collateralAudit = GetCollateralAuditsAsync(model); //获取所有抵押物信息
                var introducerAudits = GetIntroducerAuditsAsync(model); //获取介绍人信息
                var enforcementPerson = GetEnforcementPersonAsync(model); //获取被执行人情况
                var enterpriseCredit = GetEnterpriseCreditAsync(model); //获取企业资信情况
                var guarantor = GetGuarantorAsync(model); //获取担保人情况
                var individualCredit = GetIndividualCreditAsync(model); // 获取个人资信情况
                var industryCommerceTax = GetIndustryCommerceTaxAsync(model); //获取工商税务情况
                var houseDetail = GetHouseDetailAsync(model); //获取房屋明细
                var estimateSource = GetEstimateSourceAsync(model); //获取房屋估价来源

                baseAuditDAL.Update(baseAudit); //更新基础信息

                var relationPersonlist = relationPersonAuditDAL.FindByCaseID(model.ID);
                DeleteRelationPersonAudit(relationPersonlist);//删除关系人
                //DeleteRelationState(relationPersonlist);//删除锁定信息

                relationPersonAuditDAL.AddRange(relationPersonAudit); //添加新的关系人
                //AddRelationState(relationPersonAudit, baseAudit.SalesID);//添加锁定信息

                relationEnterpriseAuditDAL.AddRange(relationEnterpriseAudit); //更新联系人联系单位信息
                contactAuditDAL.AddRange(contactAudit); //更新联系人联系方式信息
                emergencyContactAuditDAL.AddRange(emergencyContactAudit); //更新联系人紧急联系方式信息
                addressAuditDAL.AddRange(addressAudit); //更新联系人地址信息

                var collaterallist = collateralAuditDAL.FindByAuditID(model.ID);
                collateralAuditDAL.DeleteRange(collaterallist); //删除抵押物
                collateralAuditDAL.AddRange(collateralAudit); //添加新的抵押物

                var enforcementPersonlist = enforcementPersonDAL.FindByAuditID(model.ID);
                enforcementPersonDAL.DeleteRange(enforcementPersonlist); //删除被执行人信息
                enforcementPersonDAL.AddRange(enforcementPerson); //添加新的被执行人信息

                var enterpriseCreditlist = enterpriseCreditDAL.FindByAuditID(model.ID);
                enterpriseCreditDAL.DeleteRange(enterpriseCreditlist); //删除企业资信信息
                enterpriseCreditDAL.AddRange(enterpriseCredit); //添加新的企业资信信息

                var guarantorlist = guarantorDAL.FindByAuditID(model.ID);
                guarantorDAL.DeleteRange(guarantorlist); //删除担保人信息
                guarantorDAL.AddRange(guarantor); //添加新的担保人信息

                var individualCreditlist = individualCreditDAL.FindByAuditID(model.ID);
                individualCreditDAL.DeleteRange(individualCreditlist); //删除个人资信信息
                individualCreditDAL.AddRange(individualCredit); //添加新的个人资信信息

                var industryCommerceTaxlist = industryCommerceTaxDAL.FindByAuditID(model.ID);
                industryCommerceTaxDAL.DeleteRange(industryCommerceTaxlist); //删除工商税务信息
                industryCommerceTaxDAL.AddRange(industryCommerceTax); //添加新的工商税务信息

                var houseDetaillist = houseDetailDAL.FindByAuditID(model.ID);
                DeleteHouseDetail(houseDetaillist);
                houseDetailDAL.AddRange(houseDetail); //添加新的房屋明细信息

                var Introducerlist = IntroducerAuditDAL.FindByAuditID(model.ID);
                IntroducerAuditDAL.DeleteRange(Introducerlist); //删除介绍人
                IntroducerAuditDAL.AddRange(introducerAudits); //添加新的介绍人

                estimateSourceDAL.AddRange(estimateSource); //更新房屋估价来源信息

                SaveFiles(model, flag); //保存附件信息
                //事务性提交
                baseAuditDAL.AcceptAllChange();

                return true;
            }

            /// <summary>
            /// 删除锁定表信息
            /// </summary>
            /// <param name="RelationPerson"></param>
            //public void DeleteRelationState(IEnumerable<RelationPersonAudit> RelationPerson)
            //{
            //    if (RelationPerson != null && RelationPerson.Any())
            //    {
            //        foreach (var item in RelationPerson)
            //        {
            //            if (item.IdentificationNumber != null)
            //            {
            //                if (item.RelationType.Equals("-PersonType-JieKuanRenPeiOu"))
            //                {
            //                    var relationstate = relationstatedal.GetAll(s => s.RelationNumber.Equals(item.IdentificationNumber) && s.RelationType == 2).FirstOrDefault();
            //                    if (relationstate != null)
            //                    {
            //                        relationstatedal.Delete(relationstate);
            //                    }
            //                }
            //            }
            //        }
            //        //relationstatedal.AcceptAllChange();
            //    }
            //}

            /// <summary>
            /// 添加锁定信息
            /// </summary>
            /// <param name="RelationPerson"></param>
            /// <param name="SalesID"></param>
            //public void AddRelationState(IEnumerable<RelationPersonAudit> RelationPerson, string SalesID)
            //{
            //    if (RelationPerson != null && RelationPerson.Any())
            //    {
            //        foreach (var item in RelationPerson)
            //        {
            //            if (item.IdentificationNumber != null)
            //            {
            //                if (item.RelationType.Equals("-PersonType-JieKuanRenPeiOu"))
            //                {
            //                    RelationStateBLLModel RelationbllModel = new RelationStateBLLModel()
            //                    {
            //                        Number = item.IdentificationNumber,
            //                        Type = item.RelationType,
            //                        SalesID = SalesID
            //                    };
            //                    relationstatebll.AddLockRelationState(RelationbllModel);
            //                }
            //            }
            //        }
            //    }
            //}

            public BaseAuditViewModel FindByID(string id, User user)
            {
                //获取审核基础信息
                var baseAudit = SetBaseAuditAsync(id, user);
                if (baseAudit != null)
                {
                    //联系人相关设置
                    var refperson = SetRelationPersonAuditAsync(id, baseAudit.CaseNum); //设置联系人信息
                    SetContactAuditAsync(refperson); //设置联系人联系方式信息
                    SetEmergencyContactAuditAsync(refperson); //设置联系人紧急联系人信息
                    SetRelationEnterpriseAuditAsync(refperson); //设置联系人企业信息
                    SetAddressAuditAsync(refperson); //设置联系人地址信息

                    var collateral = SetCollateralAuditAsync(id, baseAudit.CaseNum); //设置抵押物信息
                    var Introducer = SetIntroducerAuditAsync(id); //设置介绍人信息
                    var enforcementPerson = SetEnforcementPersonAsync(id); //设置被执行人情况信息
                    var enterpriseCredit = SetEnterpriseCreditAsync(id); //设置企业资信信息
                    var guarantor = SetGuarantorAsync(id); //设置担保人信息
                    var individualCredit = SetIndividualCreditAsync(id); //设置个人资信信息
                    var industryCommerceTax = SetIndustryCommerceTaxAsync(id); //设置工商税务信息
                    //房屋明细信息设置
                    var houseDetail = SetHouseDetailAsync(id); //设置房屋明细信息
                    SetEstimateSourceAsync(houseDetail);

                    //if (refperson != null && refperson.Any())
                    //{
                    //    var borrowerperson = refperson.FirstOrDefault(t => t.RelationType == DictionaryType.PersonType.JieKuanRen);
                    //    if (borrowerperson != null)
                    //    {
                    //        refperson.Remove(borrowerperson);
                    //        baseAudit.BorrowerPerson = borrowerperson;
                    //    }
                    //}

                    baseAudit.RelationPersonAudits = refperson;
                    baseAudit.CollateralAudits = collateral;
                    baseAudit.IndividualCredits = individualCredit;
                    baseAudit.EnterpriseCredits = enterpriseCredit;
                    baseAudit.EnforcementPersons = enforcementPerson;
                    baseAudit.IndustryCommerceTaxs = industryCommerceTax;
                    baseAudit.HouseDetails = houseDetail;
                    baseAudit.Guarantors = guarantor;
                    baseAudit.Introducer = Introducer;
                }

                return baseAudit;
            }

            //设置审核基础信息
            public BaseAuditViewModel SetBaseAuditAsync(string id, User user)
            {
                var baseaudit = this.baseAuditDAL.GetAuthorizeAndSelf(id, user);
                if (baseaudit == null)
                {
                    return null;
                }
                var basemodel = new BaseAuditViewModel();
                ObjectExtend.CopyTo(baseaudit, basemodel);

                basemodel.CaseNum = baseaudit.NewCaseNum;
                basemodel.ID = baseaudit.ID ?? Guid.NewGuid().ToString();
                basemodel.CaseTypeText = dicdal.GetText(baseaudit.CaseType);
                basemodel.LoanProposedFileName = GetFiles(basemodel.LoanProposedFile);//借款申请书
                basemodel.LoanDetailReportFileName = GetFiles(basemodel.LoanDetailReportFile);
                basemodel.FaceReportFileName = GetFiles(basemodel.FaceReportFile);
                basemodel.FieldReportFileName = GetFiles(basemodel.FieldReportFile);
                basemodel.IsNeedReport = baseaudit.IsNeedReport.HasValue ? baseaudit.IsNeedReport.Value.ToString() : "0";
                basemodel.AuditTermText = dicdal.GetText(basemodel.AuditTerm);
                basemodel.MortgageOrderText = dicdal.GetText(basemodel.MortgageOrder);
                basemodel.TermText = dicdal.GetText(basemodel.Term);
                basemodel.IsActivitieRateText = basemodel.IsActivitieRate == null ? "" : basemodel.IsActivitieRate == 0 ? "否" : "是";

                if (baseaudit.RejectType != null)
                {
                    string RejectType = "";
                    string[] str = baseaudit.RejectType.Split(',');
                    DictionaryDAL dadal = new DictionaryDAL();
                    foreach (var item in str)
                    {
                        RejectType += dadal.GetText(item) + "，";
                    }
                    basemodel.RejectType = RejectType.Substring(0, RejectType.Length - 1);
                }
                return basemodel;
            }

            //设置联系人信息
            protected List<RelationPersonAuditViewModel> SetRelationPersonAuditAsync(string caseid, string caseNum)
            {
                var person = relationPersonAuditDAL.FindByCaseID(caseid);
                var personlist = new List<RelationPersonAuditViewModel>();
                var bplist = GetBaseCasePerson(caseNum);
                foreach (var model in person)
                {
                    var per = new RelationPersonAuditViewModel()
                    {
                        ID = model.ID,
                        Birthday = model.Birthday,
                        AuditID = caseid,
                        BorrowerRelation = model.BorrowerRelation,
                        BorrowerRelationText = dicdal.GetText(model.BorrowerRelation),
                        ExpiryDate = model.ExpiryDate,
                        MaritalStatus = model.MaritalStatus,
                        MaritalStatusText = dicdal.GetText(model.MaritalStatus),
                        //IsMarried = model.IsMarried,
                        //IsMarriedText = model.IsMarried == null ? "" : (model.IsMarried == 0 ? "否" : "是"),
                        //MaritalStatus = model.MaritalStatus,
                        Name = model.Name,
                        AccountFile = model.AccountFile,
                        AccountFileName = GetFiles(model.AccountFile),
                        BirthFile = model.BirthFile,
                        BirthFileName = GetFiles(model.BirthFile),
                        IdentificationFile = model.IdentificationFile,
                        IdentificationFileName = GetFiles(model.IdentificationFile),
                        IdentificationNumber = model.IdentificationNumber,
                        IdentificationType = model.IdentificationType,
                        IdentificationTypeText = dicdal.GetText(model.IdentificationType),
                        MarryFile = model.MarryFile,
                        MarryFileName = GetFiles(model.MarryFile),
                        RelationType = model.RelationType,
                        RelationTypeText = dicdal.GetText(model.RelationType),
                        SalaryDescription = model.SalaryDescription,
                        SalaryPersonFile = model.SalaryPersonFile,
                        SalaryPersonFileName = GetFiles(model.SalaryPersonFile),
                        SelfLicenseFile = model.SelfLicenseFile,
                        SelfLicenseFileName = GetFiles(model.SelfLicenseFile),
                        SelfNonLicenseFile = model.SelfNonLicenseFile,
                        SelfNonLicenseFileName = GetFiles(model.SelfNonLicenseFile),
                        SingleFile = model.SingleFile,
                        SingleFileName = GetFiles(model.SingleFile),
                        IsCoBorrower = model.IsCoBorrower,
                        IsCoBorrowerText = model.IsCoBorrower == null ? "" : (model.IsCoBorrower == 0 ? "否" : "是"),
                        Warranty = model.Warranty,
                        BankFlowFile = model.BankFlowFile,
                        BankFlowFileName = GetFiles(model.BankFlowFile),
                        IndividualFile = model.IndividualFile,
                        IndividualFileName = GetFiles(model.IndividualFile),
                        OtherFile = model.OtherFile,
                        OtherFileName = GetFiles(model.OtherFile),
                        Sequence = model.Sequence,
                        IsFrom = bplist.Contains(model.IdentificationNumber) ? "1" : "0"
                    };
                    personlist.Add(per);
                }
                return personlist.OrderBy(p => p.Sequence).ToList();
            }

            //删除相关联系人及其子表
            protected void DeleteRelationPersonAudit(IEnumerable<RelationPersonAudit> personlist)
            {
                foreach (var model in personlist)
                {
                    //存在子表信息先删除
                    var addresslist = addressAuditDAL.GetByPersonID(model.ID);
                    addressAuditDAL.DeleteRange(addresslist); //删除关系人对应的地址
                    var contactlist = contactAuditDAL.GetByPersonID(model.ID);
                    contactAuditDAL.DeleteRange(contactlist); //删除关系人对应的所有联系人的联系方式
                    var emergencyContact = emergencyContactAuditDAL.GetByPersonID(model.ID);
                    emergencyContactAuditDAL.DeleteRange(emergencyContact); //删除关系人对应的所有紧急联系方式
                    var relationEnterprise = relationEnterpriseAuditDAL.GetByPersonID(model.ID);
                    relationEnterpriseAuditDAL.DeleteRange(relationEnterprise); //删除关系人对应的所有联系单位
                }
                relationPersonAuditDAL.DeleteRange(personlist); //删除关系人
            }

            //设置联系人联系方式信息
            protected void SetContactAuditAsync(IEnumerable<RelationPersonAuditViewModel> relationpersonid)
            {
                if (relationpersonid != null && relationpersonid.Any())
                {
                    foreach (var person in relationpersonid)
                    {
                        var contact = this.contactAuditDAL.GetByPersonID(person.ID);
                        if (contact.Any())
                        {
                            var contactlist = new List<ContactAuditViewModel>();
                            foreach (var model in contact)
                            {
                                var cont = new ContactAuditViewModel()
                                {
                                    ID = model.ID,
                                    ContactNumber = model.ContactNumber,
                                    ContactType = model.ContactType,
                                    ContactTypeText = dicdal.GetText(model.ContactType),
                                    PersonID = person.ID,
                                    IsDefault = model.IsDefault,
                                    Sequence = model.Sequence
                                };
                                contactlist.Add(cont);
                            }
                            person.ContactAudits = contactlist.OrderBy(p => p.Sequence);
                        }
                    }
                }
            }

            //设置联系人地址信息
            public void SetAddressAuditAsync(IEnumerable<RelationPersonAuditViewModel> relationpersonid)
            {
                if (relationpersonid != null && relationpersonid.Any())
                {
                    foreach (var person in relationpersonid)
                    {
                        var addresses = addressAuditDAL.GetByPersonID(person.ID);
                        if (addresses != null && addresses.Any())
                        {
                            var addresslist = new List<AddressAuditViewModel>();
                            foreach (var model in addresses)
                            {
                                var add = new AddressAuditViewModel()
                                {
                                    ID = model.ID,
                                    AddressInfo = model.AddressInfo,
                                    AddressType = model.AddressType,
                                    AddressTypeText = dicdal.GetText(model.AddressType),
                                    IsDefault = model.IsDefault,
                                    PersonID = person.ID,
                                    Sequence = model.Sequence
                                };
                                addresslist.Add(add);
                            }
                            person.AddressAudits = addresslist.OrderBy(p => p.Sequence);
                        }
                    }
                }
            }

            //设置联系人企业信息
            protected void SetRelationEnterpriseAuditAsync(IEnumerable<RelationPersonAuditViewModel> relationpersonid)
            {
                if (relationpersonid != null && relationpersonid.Any())
                {
                    foreach (var person in relationpersonid)
                    {
                        var enterprise = this.relationEnterpriseAuditDAL.GetByPersonID(person.ID);
                        if (enterprise != null && enterprise.Any())
                        {
                            var enterpriselist = new List<RelationEnterpriseAuditViewModel>();
                            foreach (var model in enterprise)
                            {
                                var enter = new RelationEnterpriseAuditViewModel()
                                {
                                    ID = model.ID,
                                    Address = model.Address,
                                    EnterpriseDes = model.EnterpriseDes,
                                    EnterpriseName = model.EnterpriseName,
                                    LegalPerson = model.LegalPerson,
                                    MainBusiness = model.MainBusiness,
                                    PersonID = person.ID,
                                    RegisteredCapital = model.RegisteredCapital,
                                    RegisterNumber = model.RegisterNumber,
                                    ShareholderDetails = model.ShareholderDetails,
                                    BankFlowFile = model.BankFlowFile,
                                    BankFlowFileName = GetFiles(model.BankFlowFile),
                                    IndividualFile = model.IndividualFile,
                                    IndividualFileName = GetFiles(model.IndividualFile),
                                    Sequence = model.Sequence
                                };
                                enterpriselist.Add(enter);
                            }
                            person.RelationEnterpriseAudits = enterpriselist.OrderBy(p => p.Sequence);
                        }
                    }
                }
            }

            //设置联系人紧急联系人信息
            protected void SetEmergencyContactAuditAsync(IEnumerable<RelationPersonAuditViewModel> relationpersonid)
            {
                if (relationpersonid != null && relationpersonid.Any())
                {
                    foreach (var person in relationpersonid)
                    {
                        var emergency = this.emergencyContactAuditDAL.GetByPersonID(person.ID);
                        if (emergency != null && emergency.Any())
                        {
                            var emergencylist = new List<EmergencyContactAuditViewModel>();
                            foreach (var model in emergency)
                            {
                                var emergencyContact = new EmergencyContactAuditViewModel()
                                {
                                    ID = model.ID ?? Guid.NewGuid().ToString(),
                                    ContactNumber = model.ContactNumber,
                                    ContactType = model.ContactType,
                                    ContactTypeText = dicdal.GetText(model.ContactType),
                                    PersonID = person.ID,
                                    Name = model.Name,
                                    Sequence = model.Sequence
                                };
                                emergencylist.Add(emergencyContact);
                            }
                            person.EmergencyContactAudits = emergencylist.OrderBy(p => p.Sequence);
                        }
                    }
                }
            }

            //设置抵押物信息
            protected List<CollateralAuditViewModel> SetCollateralAuditAsync(string auditid, string caseNum)
            {
                var collatera = this.collateralAuditDAL.FindByAuditID(auditid);
                var collateralList = new List<CollateralAuditViewModel>();
                var collist = GetBaseCaseCol(caseNum);
                foreach (var model in collatera)
                {
                    var collateral = new CollateralAuditViewModel().CastModel(model);
                    collateral.HouseFileName = GetFiles(model.HouseFile);
                    collateral.HouseReportFileName = GetFiles(model.HouseReportFile);
                    collateral.IsFrom = collist.Contains(model.HouseNumber) ? "1" : "0";
                    //var collateral = new CollateralAuditViewModel()
                    //{
                    //    ID = model.ID,
                    //    Address = model.Address,
                    //    BuildingName = model.BuildingName,
                    //    CollateralType = model.CollateralType,
                    //    CollateralTypeText = dicdal.GetText(model.CollateralType),
                    //    AuditID = auditid,
                    //    HouseFile = model.HouseFile,
                    //    HouseFileName = GetFiles(model.HouseFile),
                    //    HouseNumber = model.HouseNumber,
                    //    HouseSize = model.HouseSize,
                    //    RightOwner = model.RightOwner,
                    //    HouseReportFile = model.HouseReportFile,
                    //    HouseReportFileName = GetFiles(model.HouseReportFile),
                    //    Sequence = model.Sequence
                    //};
                    collateralList.Add(collateral);
                }
                return collateralList.OrderBy(p => p.Sequence).ToList();
            }

            //设置介绍人信息
            protected List<IntroducerAuditViewModel> SetIntroducerAuditAsync(string auditid)
            {
                var IntroducerAudit = this.IntroducerAuditDAL.FindByAuditID(auditid);
                var IntroducerAuditList = new List<IntroducerAuditViewModel>();
                foreach (var model in IntroducerAudit)
                {
                    var collateral = new IntroducerAuditViewModel()
                    {
                        ID = model.ID,
                        Account = model.Account,
                        AccountBank = model.AccountBank,
                        AuditID = auditid,
                        Contract = model.Contract,
                        Name = model.Name,
                        RebateAmmount = model.RebateAmmount,
                        RebateRate = model.RebateRate,
                        Sequence = model.Sequence
                    };
                    IntroducerAuditList.Add(collateral);
                }
                return IntroducerAuditList.OrderBy(p => p.Sequence).ToList();
            }

            //设置被执行人情况信息
            protected List<EnforcementPersonViewModel> SetEnforcementPersonAsync(string auditid)
            {
                var enforcementPersons = this.enforcementPersonDAL.FindByAuditID(auditid);
                var enforcementPersonList = new List<EnforcementPersonViewModel>();

                foreach (var model in enforcementPersons)
                {
                    var name = relationPersonAuditDAL.Get(model.PersonID);
                    var enforcementPerson = new EnforcementPersonViewModel()
                    {
                        ID = model.ID,
                        EnforcementWeb = model.EnforcementWeb,
                        //        EnforcementWebText = string.IsNullOrEmpty(model.EnforcementWeb) ? "" : model.EnforcementWeb == "1" ? "有" : "无",
                        TrialRecord = model.TrialRecord,
                        //        TrialRecordText = string.IsNullOrEmpty(model.TrialRecord) ? "" : model.TrialRecord == "1" ? "有" : "无",
                        BaseAuditID = auditid,
                        LawXP = model.LawXP,
                        //         LawXPText = string.IsNullOrEmpty(model.LawXP) ? "" : model.LawXP == "1" ? "有" : "无",
                        ShiXin = model.ShiXin,
                        //         ShiXinText = string.IsNullOrEmpty(model.ShiXin) ? "" : model.ShiXin == "1" ? "有" : "无",
                        BadNews = model.BadNews,
                        //          BadNewsText = string.IsNullOrEmpty(model.BadNews) ? "" : model.BadNews == "1" ? "有" : "无",
                        PersonID = model.PersonID,
                        PersonIDText =
                            name == null
                                ? ""
                                : name.Name + "(" +
                                  (dicdal.GetText(name.RelationType) == "" ? "借款人" : dicdal.GetText(name.RelationType)) +
                                  ")",

                        Sequence = model.Sequence,
                        AttachmentFile = model.AttachmentFile,
                        AttachmentFileName = GetFiles(model.AttachmentFile)
                    };
                    enforcementPersonList.Add(enforcementPerson);
                }
                return enforcementPersonList.OrderBy(p => p.Sequence).ToList();
            }

            //设置企业资信信息
            protected List<EnterpriseCreditViewModel> SetEnterpriseCreditAsync(string auditid)
            {
                var enterpriseCredits = this.enterpriseCreditDAL.FindByAuditID(auditid);
                var enterpriseCreditList = new List<EnterpriseCreditViewModel>();
                foreach (var model in enterpriseCredits)
                {
                    var name = relationEnterpriseAuditDAL.Get(model.EnterpriseID);
                    var enterpriseCredit = new EnterpriseCreditViewModel()
                    {
                        ID = model.ID,
                        CreditCard = model.CreditCard,
                        CreditInfo = model.CreditInfo,
                        BaseAuditID = auditid,
                        EnterpriseID = model.EnterpriseID,
                        EnterpriseIDText = name == null ? "" : name.EnterpriseName,
                        OtherDetailes = model.OtherDetailes,
                        ShareholderDetails = model.ShareholderDetails,
                        Sequence = model.Sequence
                    };
                    enterpriseCreditList.Add(enterpriseCredit);
                }
                return enterpriseCreditList.OrderBy(p => p.Sequence).ToList();
            }

            //设置担保人信息
            protected List<GuarantorViewModel> SetGuarantorAsync(string auditid)
            {
                var guarantors = this.guarantorDAL.FindByAuditID(auditid);
                var guarantorList = new List<GuarantorViewModel>();
                foreach (var model in guarantors)
                {
                    var guarantor = new GuarantorViewModel()
                    {
                        ID = model.ID,
                        Address = model.Address,
                        Name = model.Name,
                        BaseAuditID = auditid,
                        ContactNumber = model.ContactNumber,
                        RelationType = model.RelationType,
                        RelationTypeText = dicdal.GetText(model.RelationType),
                        GuarantType = model.GuarantType,
                        GuarantTypeText = dicdal.GetText(model.GuarantType),
                        IdentityType = model.IdentityType,
                        IdentityTypeText = dicdal.GetText(model.IdentityType),
                        IdentityNumber = model.IdentityNumber,
                        MarriedInfo = model.MarriedInfo,
                        Sequence = model.Sequence
                    };
                    guarantorList.Add(guarantor);
                }
                return guarantorList.OrderBy(p => p.Sequence).ToList();
            }

            //设置个人资信信息
            protected List<IndividualCreditViewModel> SetIndividualCreditAsync(string auditid)
            {
                var individualCredits = this.individualCreditDAL.FindByAuditID(auditid);
                var individualCreditList = new List<IndividualCreditViewModel>();
                foreach (var model in individualCredits)
                {
                    var name = relationPersonAuditDAL.Get(model.PersonID);
                    var individualCredit = new IndividualCreditViewModel()
                    {
                        ID = model.ID,
                        CreditCard = model.CreditCard,
                        CreditInfo = model.CreditInfo,
                        BaseAuditID = auditid,
                        OtherCredit = model.OtherCredit,
                        OverdueInfo = model.OverdueInfo,
                        IndividualFile = model.IndividualFile,
                        IndividualFileName = GetFiles(model.IndividualFile),
                        BankFlowFile = model.BankFlowFile,
                        BankFlowFileName = GetFiles(model.BankFlowFile),
                        PersonID = model.PersonID,
                        PersonIDText =
                            name == null
                                ? ""
                                : name.Name + "(" +
                                  (dicdal.GetText(name.RelationType) == "" ? "借款人" : dicdal.GetText(name.RelationType)) +
                                  ")",
                        Sequence = model.Sequence
                    };
                    individualCreditList.Add(individualCredit);
                }
                return individualCreditList.OrderBy(p => p.Sequence).ToList();
            }

            //设置工商税务信息
            protected List<IndustryCommerceTaxViewModel> SetIndustryCommerceTaxAsync(string auditid)
            {
                var industryCommerceTaxs = this.industryCommerceTaxDAL.FindByAuditID(auditid);
                var industryCommerceTaxList = new List<IndustryCommerceTaxViewModel>();

                foreach (var model in industryCommerceTaxs)
                {
                    var name = relationEnterpriseAuditDAL.Get(model.EnterpriseID);
                    var collateral = new IndustryCommerceTaxViewModel()
                    {
                        ID = model.ID,
                        AnnualInspection = model.AnnualInspection,
                        ActualManagement = model.ActualManagement,
                        BaseAuditID = auditid,
                        Description = model.Description,
                        EnterpriseID = model.EnterpriseID,
                        EnterpriseIDText = name == null ? "" : name.EnterpriseName,
                        Sequence = model.Sequence
                    };
                    industryCommerceTaxList.Add(collateral);
                }
                return industryCommerceTaxList.OrderBy(p => p.Sequence).ToList();
            }

            //设置房屋明细信息
            protected List<HouseDetailViewModel> SetHouseDetailAsync(string auditid)
            {
                var houseDetails = this.houseDetailDAL.FindByAuditID(auditid);
                var houseDetailList = new List<HouseDetailViewModel>();
                foreach (var model in houseDetails)
                {
                    var collateral = collateralAuditDAL.Get(model.CollateralID);
                    var houseDetail = new HouseDetailViewModel().CastModel(model);
                    houseDetail.BaseAuditID = auditid;
                    houseDetail.CollateralIDText = collateral == null ? "" : collateral.BuildingName + "-" + collateral.Address + "(" + dicdal.GetText(collateral.CollateralType) + ")";
                    houseDetail.HousePhotoFileName = GetFiles(model.HousePhotoFile);
                    houseDetail.HouseReportFileName = GetFiles(model.HouseReportFile);
                    houseDetail.VillagePhotoFileName = GetFiles(model.VillagePhotoFile);
                    houseDetail.MainGatePhotoFileName = GetFiles(model.MainGatePhotoFile);
                    houseDetail.ParlourPhotoFileName = GetFiles(model.ParlourPhotoFile);
                    houseDetail.BedroomPhotoFileName = GetFiles(model.BedroomPhotoFile);
                    houseDetail.KitchenRoomPhotoFileName = GetFiles(model.KitchenRoomPhotoFile);
                    houseDetail.ToiletPhotoFileName = GetFiles(model.ToiletPhotoFile);

                    houseDetail.IsDamage = model.IsDamage;
                    houseDetail.Man2Wei1 = model.Man2Wei1;
                    houseDetail.SpecialResident = model.SpecialResident;
                    if (collateral != null)
                    {
                        houseDetail.BuildingName = collateral.BuildingName;//楼盘名称
                        houseDetail.Address = collateral.Address;//房屋地址
                        houseDetail.HouseSize = collateral.HouseSize;//房屋面积
                        houseDetail.LandType = collateral.LandType;//土地类型
                        houseDetail.CompletionDate = collateral.CompletionDate;//竣工日期
                        houseDetail.HouseType = collateral.HouseType;//房屋类型
                    }
                    #region 废弃
                    //{
                    //    ID = model.ID,
                    //    AssessedValue = model.AssessedValue,
                    //    Accout = model.Accout,
                    //    BaseAuditID = auditid,
                    //    TotalHeight = model.TotalHeight,
                    //    CompletionDate = model.CompletionDate,
                    //    HouseType = model.HouseType,
                    //    HouseTypeText = dicdal.GetText(model.HouseType),
                    //    ServiceCondition = model.ServiceCondition,
                    //    RepairSituation = model.RepairSituation,
                    //    Collateral = model.Collateral,
                    //    LimitInfo = model.LimitInfo,
                    //    // LimitInfoText = string.IsNullOrEmpty(model.LimitInfo) ? "" : model.LimitInfo == "1" ? "有" : "无",
                    //    Description = model.Description,
                    //    CollateralID = model.CollateralID,
                    //    CollateralIDText =
                    //        name == null
                    //            ? ""
                    //            : name.BuildingName + "-" + name.Address + "(" + dicdal.GetText(name.CollateralType) +
                    //              ")",
                    //    HousePhotoFile = model.HousePhotoFile,
                    //    HousePhotoFileName = GetFiles(model.HousePhotoFile),
                    //    HouseReportFile = model.HouseReportFile,
                    //    HouseReportFileName = GetFiles(model.HouseReportFile),
                    //    LandType = model.LandType,
                    //    Sequence = model.Sequence
                    //};
                    #endregion
                    houseDetailList.Add(houseDetail);
                }
                return houseDetailList.OrderBy(p => p.Sequence).ToList();
            }

            //设置房屋估价来源信息
            protected void SetEstimateSourceAsync(IEnumerable<HouseDetailViewModel> houseDetails)
            {
                if (houseDetails != null && houseDetails.Any())
                {
                    foreach (var houseDetail in houseDetails)
                    {
                        var estimateSources = estimateSourceDAL.FindByHouseDetailID(houseDetail.ID);
                        if (estimateSources.Any())
                        {
                            var estimateSourcelist = new List<EstimateSourceViewModel>();
                            foreach (var model in estimateSources)
                            {
                                var cont = new EstimateSourceViewModel()
                                {
                                    ID = model.ID,
                                    EstimateInstitutions = model.EstimateInstitutions,
                                    RushEstimate = model.RushEstimate,
                                    HouseDetailID = houseDetail.ID,
                                    InformationProvider = model.InformationProvider,
                                    ContactNumber = model.ContactNumber,
                                    Sequence = model.Sequence,
                                    CertificateFile = model.CertificateFile,
                                    CertificateFileName = GetFiles(model.CertificateFile)
                                };
                                estimateSourcelist.Add(cont);
                            }
                            houseDetail.EstimateSources = estimateSourcelist.OrderBy(p => p.Sequence);
                        }
                    }
                }
            }

            private List<string> GetBaseCasePerson(string CaseNum)
            {
                //获取进件信息，判断用户是不是来自于进件
                Framework.DAL.Biz.BaseCaseDAL bd = new Framework.DAL.Biz.BaseCaseDAL();
                var basecase = bd.GetAll(t => t.NewCaseNum == CaseNum).FirstOrDefault();
                var perlist = new List<string>();
                if (basecase != null && basecase.RelationPersons.Any())
                {
                    foreach (var per in basecase.RelationPersons)
                    {
                        perlist.Add(per.IdentificationNumber);
                    }
                }
                return perlist;
            }

            private List<string> GetBaseCaseCol(string CaseNum)
            {
                Framework.DAL.Biz.BaseCaseDAL bd = new Framework.DAL.Biz.BaseCaseDAL();
                var basecase = bd.GetAll(t => t.NewCaseNum == CaseNum).FirstOrDefault();
                var perlist = new List<string>();
                if (basecase != null && basecase.Collaterals.Any())
                {
                    foreach (var col in basecase.Collaterals)
                    {
                        perlist.Add(col.HouseNumber);
                    }
                }
                return perlist;
            }
        }
    }

    public class ResponseResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}