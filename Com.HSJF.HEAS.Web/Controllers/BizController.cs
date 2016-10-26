using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.Biz;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.DAL.Sales;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.EntityFramework.Model.Biz;
using Com.HSJF.HEAS.BLL.Audit;
using Com.HSJF.HEAS.BLL.Audit.Dto;
using Com.HSJF.HEAS.BLL.Biz;
using Com.HSJF.HEAS.BLL.Other;
using Com.HSJF.HEAS.BLL.Other.Dto;
using Com.HSJF.HEAS.BLL.Sales;
using Com.HSJF.HEAS.Web.Helper;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Biz;
using Com.HSJF.HEAS.Web.Validations;
using Com.HSJF.HEAS.Web.Validations.Biz;
using Com.HSJF.Infrastructure.Extensions;
using Com.HSJF.Infrastructure.File;
using Com.HSJF.Infrastructure.Lambda;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace Com.HSJF.HEAS.Web.Controllers
{
    [Authorize(Roles = "Recording")]
    public class BizController : BaseController
    {
        private readonly BaseCaseDAL _baseCaseDal;
        private readonly BaseAuditBll _baseAuditBll;
        private readonly BaseAuditDAL _baseAuditDal;
        private readonly BaseCaseBll _baseCaseBll;
        private readonly SalesGroupBll _salesGroupBll;

        public BizController()
        {
            _baseCaseDal = new BaseCaseDAL();
            _baseAuditBll = new BaseAuditBll();
            _baseAuditDal = new BaseAuditDAL();
            _baseCaseBll = new BaseCaseBll();
            _salesGroupBll = new SalesGroupBll();
        }

        #region 列表

        [HttpGet]
        public ActionResult BaseCaseIndex()
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
        public ActionResult GetBizIndex(BaseCaseListPageRequestViewModel request)
        {
            var caseList = new List<BaseCaseListViewModel>();

            int total = 0;

            var predicate = PredicateBuilder.True<BaseAudit>();

            {
                if (request.BorrowerName.IsNotNullOrWhiteSpace())
                {
                    predicate = predicate.And(p => p.BorrowerName.Contains(request.BorrowerName));
                }
                if (request.CaseNum.IsNotNullOrWhiteSpace())
                {
                    predicate = predicate.And(p => p.NewCaseNum.Contains(request.CaseNum));
                }
                if (request.SalesGroupId.IsNotNullOrWhiteSpace() && request.SalesGroupId.IsNotNullOrEmpty())
                {
                    predicate = predicate.And(p => p.SalesGroupID == request.SalesGroupId);
                }
            }

            switch (request.CaseStatus)
            {
                case null:
                case "":
                    var modellist1 = _baseCaseDal.GetAllAuthorizeAndSelfQuery(CurrentUser, null, request.BorrowerName, request.CaseNum);
                    if (request.SalesGroupId.IsNotNullOrWhiteSpace() && request.SalesGroupId.IsNotNullOrEmpty())
                        modellist1 = modellist1.Where(p => p.SalesGroupID == request.SalesGroupId);
                    var pageList1 = _baseCaseDal.GetAllPage(modellist1, out total, request.PageSize, request.PageIndex, request.Order, request.Sort);
                    caseList = pageList1.Select(t => new BaseCaseListViewModel().CastModel(t)).ToList();
                    break;

                case "NoSubmit":
                    var modellist = _baseCaseDal.GetAllAuthorizeAndSelfQuery(CurrentUser, 0, request.BorrowerName, request.CaseNum);
                    if (request.SalesGroupId.IsNotNullOrWhiteSpace() && request.SalesGroupId.IsNotNullOrEmpty())
                        modellist = modellist.Where(p => p.SalesGroupID == request.SalesGroupId);
                    modellist = modellist.Where(x => x.CaseNum == null || x.CaseNum == "");
                    var pageList = _baseCaseDal.GetAllPage(modellist, out total, request.PageSize, request.PageIndex, request.Order, request.Sort);
                    caseList = pageList.Select(t => new BaseCaseListViewModel().CastModel(t)).ToList();
                    break;

                case "PreSubmit":
                    var modellist2 = _baseCaseDal.GetAllAuthorizeAndSelfQuery(CurrentUser, 0, request.BorrowerName, request.CaseNum);
                    if (request.SalesGroupId.IsNotNullOrWhiteSpace() && request.SalesGroupId.IsNotNullOrEmpty())
                        modellist2 = modellist2.Where(p => p.SalesGroupID == request.SalesGroupId);
                    modellist2 = modellist2.Where(x => x.CaseNum != null && x.CaseNum != "");
                    var pageList2 = _baseCaseDal.GetAllPage(modellist2, out total, request.PageSize, request.PageIndex, request.Order, request.Sort);
                    caseList = pageList2.Select(t => new BaseCaseListViewModel().CastModel(t)).ToList();
                    break;

                case "Audit":
                    predicate =
                        predicate.And(p => p.CaseStatus == CaseStatus.FirstAudit || p.CaseStatus == CaseStatus.SecondAudit);
                    caseList = FromBaseAudit(predicate, request.PageSize, request.PageIndex, out total);
                    break;

                case "Close":
                    predicate = predicate.And(p => p.CaseStatus == CaseStatus.CloseCase);
                    caseList = FromBaseAudit(predicate, request.PageSize, request.PageIndex, out total);
                    break;

                case "HatsPending":
                    predicate = predicate.And(p => p.CaseStatus == CaseStatus.HatsPending);
                    caseList = FromBaseAudit(predicate, request.PageSize, request.PageIndex, out total);
                    break;

                case "Public":
                    predicate = predicate.And(p => p.CaseStatus == CaseStatus.PublicMortgage);
                    caseList = FromBaseAudit(predicate, request.PageSize, request.PageIndex, out total);
                    break;

                case "ConfrimPub":
                    predicate = predicate.And(p => p.CaseStatus == CaseStatus.ConfrimPublic);
                    caseList = FromBaseAudit(predicate, request.PageSize, request.PageIndex, out total);
                    break;

                case "ClosePublic":
                    predicate = predicate.And(p => p.CaseStatus == CaseStatus.ClosePublic);
                    caseList = FromBaseAudit(predicate, request.PageSize, request.PageIndex, out total);
                    break;

                case "Lending":
                    predicate = predicate.And(p => p.CaseStatus == CaseStatus.Lending);
                    caseList = FromBaseAudit(predicate, request.PageSize, request.PageIndex, out total);
                    break;

                case "After":
                    predicate = predicate.And(p => p.CaseStatus == CaseStatus.AfterCase);
                    caseList = FromBaseAudit(predicate, request.PageSize, request.PageIndex, out total);
                    break;

                case "Finish":
                    predicate = predicate.And(p => p.CaseStatus == CaseStatus.FinishCase);
                    caseList = FromBaseAudit(predicate, request.PageSize, request.PageIndex, out total);
                    break;
            }

            var response = new BaseCaseListPageResponseViewModel
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize == 0 ? 10 : request.PageSize,
                Total = total
            };

            response.TotalPage = (int)Math.Ceiling((decimal)response.Total / response.PageSize);
            response.Data = caseList;
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion 列表

        #region 基础进件操作

        #region 新增

        [HttpGet]
        public ActionResult AddBaseCase()
        {
            BaseCase model = new BaseCase
            {
                ID = Guid.NewGuid().ToString(),
                CreateTime = DateTime.Now,
                CreateUser = User.Identity.Name,
                Version = 0
            };

            _baseCaseBll.Add(model);

            return RedirectToAction("EditBaseCase", "Biz", new { id = model.ID });
        }

        [HttpPost]
        public ActionResult AddBaseCase(BaseCaseViewModel model)
        {
            model.CreateTime = DateTime.Now;
            model.CreateUser = User.Identity.Name;

            CaseHelper ch = new CaseHelper();
            var result = ch.AddBaseCase(model);//保存进件信息
            ch.SaveFile(result);//保存附件信息

            return GetBaseResponse<BaseCaseViewModel>(true);
        }

        #endregion 新增

        #region 删除

        [HttpPost]
        public ActionResult DeleteBaseCase(string id)
        {
            var response = new BaseResponse<string>();

            var model = _baseCaseDal.GetAuthorizeAndSelf(id, CurrentUser);
            if (model != null && model.Version == 0)
            {
                _baseCaseBll.Delete(model);
                response.Status = "Success";
            }
            else
            {
                response.Status = "Failed";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion 删除

        #region 编辑

        public ActionResult EditBaseCase(string id)
        {
            BaseCaseDAL bd = new BaseCaseDAL();
            var bae = bd.GetAuthorizeAndSelf(id, CurrentUser);
            if (bae == null)
            {
                return RedirectToAction("Failed", "Home");
            }

            var model = new BaseCaseViewModel().CastModel(bae);
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetBaseCase(string id)
        {
            var response = new BaseResponse<BaseCaseViewModel>();
            var caseHelper = new CaseHelper();
            var model = new BaseCaseViewModel();

            model = await caseHelper.FindByID(id, CurrentUser);
            if (model != null)
            {
                if (model.CaseNum != null)
                {
                    var list = _baseAuditDal.GetListByCaseNum(model.CaseNum);
                    model.AuditHistory = new AuditHisHelper().GetHistory(list);
                }
                response.Status = StatusEnum.Success.ToString();
                response.Data = model;
            }
            else
            {
                response.Status = StatusEnum.Failed.ToString();
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditBaseCase(BaseCaseViewModel model)
        {
            var response = new BaseResponse<string>
            {
                Data = string.Empty
            };
            var ch = new CaseHelper();
            var bd = new BaseCaseDAL();
            var em = new List<ErrorMessage>();
            var relationstatedal = new RelationStateDAL();
            var saleDal = new SalesManDAL();
            var result = new EditBaseCaseValidator().Validate(model);

            #region 查询锁定

            //if (model.BorrowerPerson !=null)
            //{
            //    var relation = relationstatedal.GetAll(s => s.RelationNumber == model.BorrowerPerson.IdentificationNumber && s.IsLock == 1).FirstOrDefault();
            //    if (relation != null)
            //    {
            //        em.Add(new ErrorMessage() { Key = "RelationState", Message = "借款人 " + model.BorrowerPerson.Name + " 已锁定" });
            //        if (relation.IsBinding == 1)
            //        {
            //            if (relation.SalesID != model.SalesID)
            //            {
            //                em.Add(new ErrorMessage() { Key = "RelationState", Message = model.BorrowerPerson.RelationTypeText + " " + model.BorrowerPerson.Name + " 已绑定销售人员" + (relation.SalesID == null ? "" : saleDal.Get(relation.SalesID) == null ? "" : saleDal.Get(relation.SalesID).Name) });
            //            }
            //        }
            //    }
            //}

            if (model.RelationPerson != null)
            {
                foreach (var item in model.RelationPerson)
                {
                    // 关系人列表当关系人未满18岁时，“出生证明”必填
                    if (item.Birthday != null && item.Birthday.Value.AddYears(18) > DateTime.Now && item.BirthFile == null)
                    {
                        em.Add(new ErrorMessage() { Key = "BirthFile", Message = "当关系人未满18岁时，“出生证明”必填" });
                    }

                    if (item.RelationType.Equals("-PersonType-JieKuanRenPeiOu") || item.RelationType.Equals("-PersonType-JieKuanRen"))
                    {
                        var relation = relationstatedal.GetAll(s => s.RelationNumber == item.IdentificationNumber && s.IsLock == 1).FirstOrDefault();
                        if (relation != null)
                        {
                            em.Add(new ErrorMessage() { Key = "RelationState", Message = item.RelationTypeText + " " + item.Name + "  已被其它业务员锁定" });
                            if (relation.IsBinding == 1)
                            {
                                if (relation.SalesID != model.SalesID)
                                {
                                    em.Add(new ErrorMessage() { Key = "RelationState", Message = item.RelationTypeText + " " + item.Name + " 已绑定销售人员" + (relation.SalesID == null ? "" : saleDal.Get(relation.SalesID) == null ? "" : saleDal.Get(relation.SalesID).Name) });
                                }
                            }
                        }
                    }
                }
            }

            if (model.Collateral != null)
            {
                foreach (var item in model.Collateral)
                {
                    if (item.CollateralType.Equals("-FacilityCategary-MainFacility"))
                    {
                        var relation = relationstatedal.GetAll(s => s.RelationNumber == item.HouseNumber && s.IsLock == 1).FirstOrDefault();
                        if (relation != null)
                        {
                            em.Add(new ErrorMessage() { Key = "RelationState", Message = item.CollateralTypeText + " " + item.HouseNumber + "  已被其它业务员锁定" });
                        }
                    }
                }
            }

            // 添加错误信息
            result.Add(em);

            #endregion 查询锁定

            if (result.IsNotValid())
            {
                response.Status = StatusEnum.Failed.ToString();
                response.Message = result.GetErrors().ToArray();
            }
            else
            {
                var baseCasemodel = bd.GetAuthorizeAndSelf(model.ID, CurrentUser);//根据key获取一边数据
                if (baseCasemodel == null || baseCasemodel.Version > 0)
                {
                    return GetBaseResponse<BaseCaseViewModel>(false);
                }

                model.CreateUser = CurrentUser.UserName;
                model.CreateTime = DateTime.Now;
                if (ch.UpdateBaseCase(model, true)) //保存编辑进件信息
                {
                    ch.SaveFile(model); //保存附件信息
                    response.Status = "Success";
                }
                else
                {
                    response.Status = "Failed";
                }
            }

            return Json(response);
        }

        #endregion 编辑

        #region 进件只读页面
        /// <summary>
        /// 进件只读页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ReadonlyBaseCase(string id)
        {
            var bd = new BaseCaseDAL();
            var bae = bd.GetAuthorizeAndSelf(id, CurrentUser);
            if (bae == null)
            {
                return RedirectToAction("Failed", "Home");
            }
            var model = new BaseCaseViewModel().CastModel(bae);
            return View(model);
        }
        #endregion
        #region 拷贝

        public ActionResult CopyBaseCase(string id)
        {
            BaseCaseDAL bd = new BaseCaseDAL();

            BaseCase basecase = bd.CopyBaseCase(id, CurrentUser);
            if (basecase != null)
            {
                var bae = bd.GetAuthorizeAndSelf(basecase.ID, CurrentUser);
                if (bae == null)
                {
                    RedirectToAction("Failed", "Home");
                }
                return RedirectToAction("EditBaseCase", "Biz", new { ID = basecase.ID });
            }
            else
            {
                return RedirectToAction("Failed", "Home");
            }
        }

        #endregion 拷贝

        #region 预提交功能

        /// <summary>
        /// 提交进件
        /// </summary>
        /// <param name="model">进件model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PreSubmitBaseCase(BaseCaseViewModel model)
        {
            var ch = new CaseHelper();
            var response = new BaseResponse<string>();

            #region 验证

            var validateResults = new SubmitBaseCaseValidator().Validate(model);
            var editValidateResults = new EditBaseCaseValidator().Validate(model);

            validateResults.Add(editValidateResults.GetErrors());

            if (!ModelState.IsValid)
            {
                foreach (var e in ModelState.Keys)
                {
                    if (ModelState[e].Errors.Any())
                    {
                        if (e.Contains("FileName"))
                            continue;
                        if (string.IsNullOrEmpty(ModelState[e].Errors[0].ErrorMessage))
                            continue;

                        if (e.Contains("BorrowerPerson.RelationType"))
                            continue;

                        validateResults.Add(new ErrorMessage(e, ModelState[e].Errors[0].ErrorMessage));
                    }
                }
            }

            #region 验证绑定信息

            // 锁定抵押物
            if (model.Collateral != null)
            {
                foreach (var item in model.Collateral)
                {
                    if (item.CompletionDate.IsNullOrEmpty())
                        validateResults.Add(new ErrorMessage("CompletionDate", "房产信息 - 竣工年份 不能为空"));
                    if (item.HouseType.IsNullOrEmpty())
                        validateResults.Add(new ErrorMessage("HouseType", "房产信息 - 房屋类型 不能为空"));
                    if (item.LandType.IsNullOrEmpty())
                        validateResults.Add(new ErrorMessage("LandType", "房产信息 - 土地类型 不能为空"));
                }
            }

            #endregion 验证绑定信息

            if (validateResults.IsNotValid())
            {
                response.Status = "Failed";
                response.Message = validateResults.GetErrors().ToArray();
                response.Data = null;

                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion 验证


            var bd = new BaseCaseDAL();
            var baseCasemodel = bd.GetAuthorizeAndSelf(model.ID, CurrentUser);//根据key获取一边数据
            if (baseCasemodel == null || baseCasemodel.Version > 0)
            {
                return GetBaseResponse<BaseCaseViewModel>(false);
            }

            // 没有案件号则生成案件号
            if (string.IsNullOrEmpty(baseCasemodel.CaseNum))
                model.CaseNum = _baseCaseBll.GenCaseNumber(model.SalesGroupID);
            else
                model.CaseNum = baseCasemodel.NewCaseNum;

            model.CreateUser = CurrentUser.UserName;
            model.CreateTime = DateTime.Now;

            if (ch.UpdateBaseCase(model, true, true)) //保存编辑进件信息
            {
                ch.SaveFile(model); //保存附件信息
                response.Status = "Success";
            }
            else
            {
                response.Status = "Failed";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion 提交进件审核

        #region 提交进件审核

        /// <summary>
        /// 提交进件
        /// </summary>
        /// <param name="model">进件model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitBaseCase(BaseCaseViewModel model)
        {
            var ch = new CaseHelper();
            var response = new BaseResponse<string>();
            // var relationstatebll = new RelationStateBLL();

            #region 验证

            var validateResults = new SubmitBaseCaseValidator().Validate(model);
            var editValidateResults = new EditBaseCaseValidator().Validate(model);

            validateResults.Add(editValidateResults.GetErrors());

            if (!ModelState.IsValid)
            {
                foreach (var e in ModelState.Keys)
                {
                    if (ModelState[e].Errors.Any())
                    {
                        if (e.Contains("FileName"))
                            continue;
                        if (string.IsNullOrEmpty(ModelState[e].Errors[0].ErrorMessage))
                            continue;

                        if (e.Contains("BorrowerPerson.RelationType"))
                            continue;

                        validateResults.Add(new ErrorMessage(e, ModelState[e].Errors[0].ErrorMessage));
                    }
                }
            }

            #region 验证绑定信息

            // 锁定联系人
            //if (model.BorrowerPerson != null)
            //{
            //    RelationStateBLLModel relationbllModel = new RelationStateBLLModel()
            //    {
            //        Number = model.BorrowerPerson.IdentificationNumber,
            //        Name = model.BorrowerPerson.Name,
            //        SalesID = model.SalesID,
            //        TextName = "借款人"
            //    };
            //    if (relationstatebll.QueryRelationStateLocking(relationbllModel) != null)
            //    {
            //        string key = relationstatebll.QueryRelationStateLocking(relationbllModel).Key;
            //        string message = relationstatebll.QueryRelationStateLocking(relationbllModel).Message;
            //        if (key != null || message != null)
            //        {
            //            validateResults.Add(new ErrorMessage(key, message));
            //        }
            //    }
            //    if (relationstatebll.QueryRelationStateBinding(relationbllModel) != null)
            //    {
            //        string key = relationstatebll.QueryRelationStateBinding(relationbllModel).Key;
            //        string message = relationstatebll.QueryRelationStateBinding(relationbllModel).Message;
            //        if (key != null || message != null)
            //        {
            //            validateResults.Add(new ErrorMessage(key, message));
            //        }
            //    }
            //}

            // 锁定关系人
            //Extensions.IfNotNull(model.RelationPerson, p =>
            //{
            //    foreach (var item in p)
            //    {
            //        var relationbllModel = new RelationStateBLLModel()
            //        {
            //            Number = item.IdentificationNumber,
            //            Name = item.Name,
            //            SalesID = model.SalesID,
            //            TextName = item.RelationTypeText
            //        };
            //        if (relationstatebll.QueryRelationStateLocking(relationbllModel) != null)
            //        {
            //            string key = relationstatebll.QueryRelationStateLocking(relationbllModel).Key;
            //            string message = relationstatebll.QueryRelationStateLocking(relationbllModel).Message;
            //            if (key != null || message != null)
            //            {
            //                validateResults.Add(new ErrorMessage(key, message));
            //            }
            //        }
            //        if (relationstatebll.QueryRelationStateBinding(relationbllModel) != null)
            //        {
            //            string key = relationstatebll.QueryRelationStateBinding(relationbllModel).Key;
            //            string message = relationstatebll.QueryRelationStateBinding(relationbllModel).Message;
            //            if (key != null || message != null)
            //            {
            //                validateResults.Add(new ErrorMessage(key, message));
            //            }
            //        }
            //    }
            //});

            // 锁定抵押物
            if (model.Collateral != null)
            {
                foreach (var item in model.Collateral)
                {
                    if (item.CompletionDate.IsNullOrEmpty())
                        validateResults.Add(new ErrorMessage("CompletionDate", "房产信息 - 竣工年份 不能为空"));
                    if (item.HouseType.IsNullOrEmpty())
                        validateResults.Add(new ErrorMessage("HouseType", "房产信息 - 房屋类型 不能为空"));
                    if (item.LandType.IsNullOrEmpty())
                        validateResults.Add(new ErrorMessage("LandType", "房产信息 - 土地类型 不能为空"));

                    //RelationStateBLLModel RelationbllModel = new RelationStateBLLModel()
                    //{
                    //    Number = item.HouseNumber,
                    //    Name = item.HouseNumber,
                    //    TextName = item.CollateralTypeText
                    //};
                    //if (item.CollateralType == "-FacilityCategary-MainFacility")
                    //{
                    //    if (relationstatebll.QueryRelationStateLocking(RelationbllModel) != null)
                    //    {
                    //        string key = relationstatebll.QueryRelationStateLocking(RelationbllModel).Key;
                    //        string message = relationstatebll.QueryRelationStateLocking(RelationbllModel).Message;
                    //        if (key != null || message != null)
                    //        {
                    //            validateResults.Add(new ErrorMessage(key, message));
                    //        }
                    //    }
                    //}
                }
            }

            #endregion 验证绑定信息

            if (validateResults.IsNotValid())
            {
                response.Status = "Failed";
                response.Message = validateResults.GetErrors().ToArray();
                response.Data = null;

                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion 验证

            var baseCaseModel = _baseCaseDal.GetAuthorizeAndSelf(model.ID, CurrentUser);//根据key获取一边数据
            var baseCaseViewModel = new BaseCaseViewModel();
            var falg = true;
            if (baseCaseModel == null)//判断数据有无，有则编辑，无则新增
            {
                model.CreateTime = DateTime.Now;
                model.CreateUser = User.Identity.Name;
                model.Version = 1;

                // 预提交的时候如果已经生成案件号不在重新生成
                if (string.IsNullOrEmpty(baseCaseModel.CaseNum))
                    model.CaseNum = _baseCaseBll.GenCaseNumber(model.SalesGroupID);
                else
                    model.CaseNum = baseCaseModel.CaseNum;

                baseCaseViewModel = ch.AddBaseCase(model); //保存进件信息
                ch.SaveFile(baseCaseViewModel, 1); //保存附件信息
            }
            else if (baseCaseModel.Version == 0)
            {
                // 预提交的时候如果已经生成案件号不在重新生成
                if (string.IsNullOrEmpty(baseCaseModel.CaseNum))
                    model.CaseNum = _baseCaseBll.GenCaseNumber(model.SalesGroupID);
                else
                    model.CaseNum = baseCaseModel.CaseNum;

                model.CreateTime = DateTime.Now;
                model.CreateUser = CurrentUser.UserName;
                model.Version = 1;
                falg = ch.UpdateBaseCase(model, true);//保存编辑进件信息
                ch.SaveFile(model, 1);//保存附件信息
            }
            if (baseCaseViewModel != null || falg)
            {
                // 提交进件审核

                #region 添加锁定

                //if (model.RelationPerson != null)
                //{
                //    foreach (var item in model.RelationPerson)
                //    {
                //        if (item.RelationType.Equals(DictionaryType.PersonType.JieKuanRenPeiOu) ||
                //            item.RelationType.Equals(DictionaryType.PersonType.JieKuanRen))
                //        {
                //            RelationStateBLLModel relationbllModel = new RelationStateBLLModel()
                //            {
                //                Number = item.IdentificationNumber,
                //                Type = item.RelationType,
                //                SalesID = model.SalesID
                //            };
                //            relationstatebll.AddLockRelationState(relationbllModel);
                //        }
                //    }
                //}
                //if (model.Collateral != null)
                //{
                //    foreach (var item in model.Collateral)
                //    {
                //        if (item.CollateralType.Equals(DictionaryType.FacilityCategary.MainFacility))
                //        {
                //            RelationStateBLLModel relationbllModel = new RelationStateBLLModel()
                //            {
                //                Number = item.HouseNumber,
                //                SalesID = model.SalesID,
                //                Type = DictionaryType.FacilityCategary.MainFacility
                //            };
                //            relationstatebll.AddLockRelationState(relationbllModel);
                //        }
                //    }
                //}

                #endregion 添加锁定

                _baseCaseDal.SubmitBaseCase(model.ID, CurrentUser.UserName);

                response.Status = "Success";
            }
            else
            {
                response.Status = "Failed";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion 提交进件审核

        #endregion 基础进件操作

        #region 进件辅助对象

        /// <summary>
        /// 获取进件的辅助对象
        /// </summary>
        public class CaseHelper
        {
            private BaseCaseDAL baseCaseDAL;
            private RelationPersonDAL relationPersonDAL;
            private RelationEnterpriseDAL relationEnterpriseDAL;
            private ContactDAL contactDAL;
            private CollateralDAL collateralDAL;
            private IntroducerDAL IntroducerDAL;
            private EmergencyContactDAL emergencyContactDAL;
            private AddressDAL addressDAL;
            private DictionaryDAL dicdal;
            private FileUpload filedal;

            public CaseHelper()
            {
                baseCaseDAL = new BaseCaseDAL();
                relationPersonDAL = new RelationPersonDAL();
                relationEnterpriseDAL = new RelationEnterpriseDAL();
                contactDAL = new ContactDAL();
                collateralDAL = new CollateralDAL();
                IntroducerDAL = new IntroducerDAL();
                emergencyContactDAL = new EmergencyContactDAL();
                addressDAL = new AddressDAL();
                dicdal = new DictionaryDAL();
                filedal = new FileUpload();
            }

            #region 新增进件

            public BaseCaseViewModel AddBaseCase(BaseCaseViewModel model)
            {
                if (model == null)
                {
                    return null;
                }
                var baseCase = GetBaseCaseAsync(model);

                if (model.RelationPerson != null && model.RelationPerson.Any())
                {
                    var relationperson = model.RelationPerson.Where(x => x.RelationType == DictionaryType.PersonType.JieKuanRen).Select(x => x.Name);
                    if (relationperson != null && relationperson.Any())
                    {
                        baseCase.BorrowerName = relationperson.First().ToString();
                    }
                    else
                    {
                        baseCase.BorrowerName = model.RelationPerson.First().Name;
                    }
                }
                var relationPerson = GetRelationPersonAsync(model);
                var Contact = GetContactAsync(model);
                var relationEnterprise = GetRelationEnterpriseAsync(model);
                var emergencyContact = GetEmergencyContactAsync(model);
                var address = GetAddressAsync(model);
                var collateral = GetCollateralAsync(model);

                baseCaseDAL.Add(baseCase);
                relationPersonDAL.AddRange(relationPerson);
                relationEnterpriseDAL.AddRange(relationEnterprise);
                contactDAL.AddRange(Contact);
                collateralDAL.AddRange(collateral);
                emergencyContactDAL.AddRange(emergencyContact);
                addressDAL.AddRange(address);
                //事务性提交
                baseCaseDAL.AcceptAllChange();
                return model;
            }

            /// <summary>
            /// 新增文件信息
            /// </summary>
            /// <param name="basecase">进件信息</param>
            public void SaveFile(BaseCaseViewModel basecase, int filestate = 0)
            {
                Infrastructure.File.FileUpload up = new Infrastructure.File.FileUpload();//文件上传
                if (basecase.RelationPerson != null && basecase.RelationPerson.Any())
                {
                    foreach (var model in basecase.RelationPerson)
                    {
                        if (model.ID.ToUpper().Contains("TEMP"))
                        {
                            model.ID = Guid.NewGuid().ToString();
                        }
                        //证件复印件
                        if (!string.IsNullOrEmpty(model.IdentificationFile))
                        {
                            string[] identificationFileGuids = model.IdentificationFile.Split(',');

                            for (int i = 0; i < identificationFileGuids.Length; i++)
                            {
                                FileDescription filemodel = up.Single(new Guid(identificationFileGuids[i]));
                                filemodel.LinkID = new Guid(model.ID);
                                filemodel.LinkKey = basecase.ID;
                                filemodel.FileState = filestate;
                                filemodel.Description = GetDisplayName(model.GetType().GetProperty("IdentificationFile"));
                                up.SaveFileDescription(filemodel);
                            }
                        }
                        //结婚证
                        if (!string.IsNullOrEmpty(model.MarryFile))
                        {
                            string[] marryFileGuids = model.MarryFile.Split(',');
                            for (int i = 0; i < marryFileGuids.Length; i++)
                            {
                                FileDescription filemodel = up.Single(new Guid(marryFileGuids[i]));
                                filemodel.LinkID = new Guid(model.ID);
                                filemodel.LinkKey = basecase.ID;
                                filemodel.FileState = filestate;
                                filemodel.Description = GetDisplayName(model.GetType().GetProperty("MarryFile"));
                                up.SaveFileDescription(filemodel);
                            }
                        }
                        //收入证明(授薪人士)
                        if (!string.IsNullOrEmpty(model.SalaryPersonFile))
                        {
                            string[] salaryPersonFileGuids = model.SalaryPersonFile.Split(',');
                            for (int i = 0; i < salaryPersonFileGuids.Length; i++)
                            {
                                FileDescription filemodel = up.Single(new Guid(salaryPersonFileGuids[i]));
                                filemodel.LinkID = new Guid(model.ID);
                                filemodel.LinkKey = basecase.ID;
                                filemodel.FileState = filestate;
                                filemodel.Description = GetDisplayName(model.GetType().GetProperty("SalaryPersonFile"));
                                up.SaveFileDescription(filemodel);
                            }
                        }
                        //收入证明(自雇有执照)
                        if (!string.IsNullOrEmpty(model.SelfLicenseFile))
                        {
                            string[] selfLicenseFileGuids = model.SelfLicenseFile.Split(',');
                            for (int i = 0; i < selfLicenseFileGuids.Length; i++)
                            {
                                FileDescription filemodel = up.Single(new Guid(selfLicenseFileGuids[i]));
                                filemodel.LinkID = new Guid(model.ID);
                                filemodel.LinkKey = basecase.ID;
                                filemodel.FileState = filestate;
                                filemodel.Description = GetDisplayName(model.GetType().GetProperty("SelfLicenseFile"));
                                up.SaveFileDescription(filemodel);
                            }
                        }
                        //收入证明(自雇无执照)
                        if (!string.IsNullOrEmpty(model.SelfNonLicenseFile))
                        {
                            string[] selfNonLicenseGuids = model.SelfNonLicenseFile.Split(',');
                            for (int i = 0; i < selfNonLicenseGuids.Length; i++)
                            {
                                FileDescription filemodel = up.Single(new Guid(selfNonLicenseGuids[i]));
                                filemodel.LinkID = new Guid(model.ID);
                                filemodel.LinkKey = basecase.ID;
                                filemodel.FileState = filestate;
                                filemodel.Description = GetDisplayName(model.GetType().GetProperty("SelfNonLicenseFile"));
                                up.SaveFileDescription(filemodel);
                            }
                        }
                        //单身证明
                        if (!string.IsNullOrEmpty(model.SingleFile))
                        {
                            string[] singleGuids = model.SingleFile.Split(',');
                            for (int i = 0; i < singleGuids.Length; i++)
                            {
                                FileDescription filemodel = up.Single(new Guid(singleGuids[i]));
                                filemodel.LinkID = new Guid(model.ID);
                                filemodel.LinkKey = basecase.ID;
                                filemodel.FileState = filestate;
                                filemodel.Description = GetDisplayName(model.GetType().GetProperty("SingleFile"));
                                up.SaveFileDescription(filemodel);
                            }
                        }
                        //出生证
                        if (!string.IsNullOrEmpty(model.BirthFile))
                        {
                            string[] birthFileGuids = model.BirthFile.Split(',');
                            for (int i = 0; i < birthFileGuids.Length; i++)
                            {
                                FileDescription filemodel = up.Single(new Guid(birthFileGuids[i]));
                                filemodel.LinkID = new Guid(model.ID);
                                filemodel.LinkKey = basecase.ID;
                                filemodel.FileState = filestate;
                                filemodel.Description = GetDisplayName(model.GetType().GetProperty("BirthFile"));
                                up.SaveFileDescription(filemodel);
                            }
                        }
                        //户口本
                        if (!string.IsNullOrEmpty(model.AccountFile))
                        {
                            string[] accountFileGuids = model.AccountFile.Split(',');
                            for (int i = 0; i < accountFileGuids.Length; i++)
                            {
                                FileDescription filemodel = up.Single(new Guid(accountFileGuids[i]));
                                filemodel.LinkID = new Guid(model.ID);
                                filemodel.LinkKey = basecase.ID;
                                filemodel.FileState = filestate;
                                filemodel.Description = GetDisplayName(model.GetType().GetProperty("AccountFile"));
                                up.SaveFileDescription(filemodel);
                            }
                        }
                        //其他证明
                        if (!string.IsNullOrEmpty(model.OtherFile))
                        {
                            string[] OtherFileFileGuids = model.OtherFile.Split(',');
                            for (int i = 0; i < OtherFileFileGuids.Length; i++)
                            {
                                FileDescription filemodel = up.Single(new Guid(OtherFileFileGuids[i]));
                                filemodel.LinkID = new Guid(model.ID);
                                filemodel.LinkKey = basecase.ID;
                                filemodel.FileState = filestate;
                                filemodel.Description = GetDisplayName(model.GetType().GetProperty("OtherFile"));
                                up.SaveFileDescription(filemodel);
                            }
                        }
                        //个人征信报告
                        if (!string.IsNullOrEmpty(model.IndividualFile))
                        {
                            string[] individualFileGuids = model.IndividualFile.Split(',');
                            for (int i = 0; i < individualFileGuids.Length; i++)
                            {
                                FileDescription filemodel = up.Single(new Guid(individualFileGuids[i]));
                                filemodel.LinkID = new Guid(model.ID);
                                filemodel.LinkKey = basecase.ID;
                                filemodel.FileState = filestate;
                                filemodel.Description = GetDisplayName(model.GetType().GetProperty("IndividualFile"));
                                up.SaveFileDescription(filemodel);
                            }
                        }
                        //银行流水
                        if (!string.IsNullOrEmpty(model.BankFlowFile))
                        {
                            string[] bankFlowFileGuids = model.BankFlowFile.Split(',');
                            for (int i = 0; i < bankFlowFileGuids.Length; i++)
                            {
                                FileDescription filemodel = up.Single(new Guid(bankFlowFileGuids[i]));
                                filemodel.LinkID = new Guid(model.ID);
                                filemodel.LinkKey = basecase.ID;
                                filemodel.FileState = filestate;
                                filemodel.Description = GetDisplayName(model.GetType().GetProperty("BankFlowFile"));
                                up.SaveFileDescription(filemodel);
                            }
                        }
                    }
                }
                if (basecase.Collateral != null && basecase.Collateral.Any())
                {
                    foreach (var model in basecase.Collateral)
                    {
                        if (model.ID.ToUpper().Contains("TEMP"))
                        {
                            model.ID = Guid.NewGuid().ToString();
                        }
                        if (!string.IsNullOrEmpty(model.HouseFile))
                        {
                            string[] accountFileGuids = model.HouseFile.Split(',');
                            for (int i = 0; i < accountFileGuids.Length; i++)
                            {
                                FileDescription filemodel = up.Single(new Guid(accountFileGuids[i]));
                                filemodel.LinkID = new Guid(model.ID);
                                filemodel.LinkKey = basecase.ID;
                                filemodel.FileState = filestate;
                                filemodel.Description = GetDisplayName(model.GetType().GetProperty("HouseFile"));
                                up.SaveFileDescription(filemodel);
                            }
                        }
                    }
                }
            }

            private string GetDisplayName(PropertyInfo property)
            {
                var attr = property.GetCustomAttribute<DisplayAttribute>();
                if (attr != null)
                {
                    return attr.Name;
                }
                return "";
            }

            //获取案件基础信息
            public BaseCase GetBaseCaseAsync(BaseCaseViewModel caseViewModel)
            {
                var basemodel = new BaseCase();

                if (!string.IsNullOrEmpty(caseViewModel.ID))
                {
                    if (caseViewModel.ID.ToUpper().Contains("TEMP"))
                    {
                        caseViewModel.ID = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        basemodel = baseCaseDAL.Get(caseViewModel.ID);
                    }
                }

                if (!string.IsNullOrEmpty(basemodel.CaseNum))
                    caseViewModel.CaseNum = basemodel.CaseNum;
                if (!string.IsNullOrEmpty(basemodel.NewCaseNum))
                    caseViewModel.CaseNum = basemodel.NewCaseNum;

                //    IEnumerable<string> exclud = new List<string>() { "Version", "CreateTime", "CreateUser" };
                Infrastructure.ExtendTools.ObjectExtend.CopyTo(caseViewModel, basemodel);
                //修改CaseNum 只有数字没有地区编号
                basemodel.CaseNum = string.IsNullOrEmpty(caseViewModel.CaseNum) ? string.Empty : caseViewModel.CaseNum.Split('-')[1];
                basemodel.NewCaseNum = caseViewModel.CaseNum;
                basemodel.ID = caseViewModel.ID;

                //caseViewModel.ID = basemodel.ID;

                //先处理 借款人
                //if (caseViewModel.BorrowerPerson != null)
                //{
                //    caseViewModel.BorrowerPerson.RelationType = "-PersonType-JieKuanRen";
                //    basemodel.BorrowerName = caseViewModel.BorrowerPerson.Name;
                //    if (caseViewModel.RelationPerson == null)
                //    {
                //        caseViewModel.RelationPerson = new List<RelationPersonViewModel>();
                //    }
                //    caseViewModel.RelationPerson.Add(caseViewModel.BorrowerPerson);
                //}

                return basemodel;
            }

            #region 联系人信息

            //联系人信息
            public List<RelationPerson> GetRelationPersonAsync(BaseCaseViewModel basecase)
            {
                var personlist = new List<RelationPerson>();
                if (basecase.RelationPerson != null && basecase.RelationPerson.Any())
                {
                    foreach (var model in basecase.RelationPerson)
                    {
                        if (model == null) break;

                        model.ID = Guid.NewGuid().ToString();
                        var personmodel = Setperson(model, basecase);

                        personlist.Add(personmodel);
                    }
                }
                return personlist;
            }

            //处理 relationperson，先删除所有相关信息
            private RelationPerson Setperson(RelationPersonViewModel rpvm, BaseCaseViewModel basecase)
            {
                var addresslist = addressDAL.GetByPersonID(rpvm.ID);
                addressDAL.DeleteRange(addresslist);//删除关系人对应的地址
                var contactlist = contactDAL.GetByPersonID(rpvm.ID);
                contactDAL.DeleteRange(contactlist);//删除关系人对应的所有联系人的联系方式
                var emergencyContact = emergencyContactDAL.GetByPersonID(rpvm.ID);
                emergencyContactDAL.DeleteRange(emergencyContact);//删除关系人对应的所有紧急联系方式
                var relationEnterprise = relationEnterpriseDAL.GetByPersonID(rpvm.ID);
                relationEnterpriseDAL.DeleteRange(relationEnterprise);//删除关系人对应的所有联系单位

                var personmodel = new RelationPerson();
                Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(rpvm, personmodel);
                personmodel.ID = rpvm.ID;
                personmodel.CaseID = basecase.ID;
                rpvm.CaseID = basecase.ID;
                return personmodel;
            }

            //获取所有联系人的联系方式
            public List<Contact> GetContactAsync(BaseCaseViewModel basecase)
            {
                var contactlist = new List<Contact>();
                if (basecase.RelationPerson != null && basecase.RelationPerson.Any())
                {
                    foreach (var person in basecase.RelationPerson)
                    {
                        if (person.Contacts != null && person.Contacts.Any())
                        {
                            foreach (var model in person.Contacts)
                            {
                                if (model == null) break;
                                model.ID = Guid.NewGuid().ToString();
                                var contact = new Contact();
                                Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, contact);
                                contact.ID = model.ID ?? Guid.NewGuid().ToString();
                                contact.PersonID = person.ID;
                                model.ID = contact.ID;
                                model.PersonID = person.ID;
                                contact.IsDefault = model.IsDefault;
                                contact.Sequence = model.Sequence;

                                contactlist.Add(contact);
                            }
                        }
                    }
                }
                return contactlist;
            }

            //获取所有地址信息
            public List<Address> GetAddressAsync(BaseCaseViewModel basecase)
            {
                var addresslist = new List<Address>();
                if (basecase.RelationPerson != null && basecase.RelationPerson.Any())
                {
                    foreach (var person in basecase.RelationPerson)
                    {
                        if (person.Addresses != null && person.Addresses.Any())
                        {
                            foreach (var model in person.Addresses)
                            {
                                if (model == null) break;
                                model.ID = Guid.NewGuid().ToString();
                                var address = new Address();
                                Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, address);
                                address.ID = model.ID ?? Guid.NewGuid().ToString();
                                address.PersonID = person.ID;
                                model.ID = address.ID;
                                model.PersonID = person.ID;
                                address.IsDefault = model.IsDefault;
                                addresslist.Add(address);
                            }
                        }
                    }
                }
                return addresslist;
            }

            //获取所有紧急联系方式
            public List<EmergencyContact> GetEmergencyContactAsync(BaseCaseViewModel basecase)
            {
                var emergencyContactlist = new List<EmergencyContact>();
                if (basecase.RelationPerson != null && basecase.RelationPerson.Any())
                {
                    foreach (var person in basecase.RelationPerson)
                    {
                        if (person.EmergencyContacts != null && person.EmergencyContacts.Any())
                        {
                            foreach (var model in person.EmergencyContacts)
                            {
                                if (model == null) break;
                                model.ID = Guid.NewGuid().ToString();

                                var emergencyContact = new EmergencyContact();
                                Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, emergencyContact);
                                emergencyContact.ID = model.ID ?? Guid.NewGuid().ToString();
                                emergencyContact.PersonID = person.ID;
                                model.ID = emergencyContact.ID;
                                model.PersonID = person.ID;
                                emergencyContactlist.Add(emergencyContact);
                            }
                        }
                    }
                }
                return emergencyContactlist;
            }

            //获取所有联系单位
            public List<RelationEnterprise> GetRelationEnterpriseAsync(BaseCaseViewModel basecase)
            {
                var enterpriselist = new List<RelationEnterprise>();
                if (basecase.RelationPerson != null && basecase.RelationPerson.Any())
                {
                    foreach (var person in basecase.RelationPerson)
                    {
                        if (person.RelationEnterprise != null && person.RelationEnterprise.Any())
                        {
                            foreach (var model in person.RelationEnterprise)
                            {
                                if (model == null) break;

                                model.ID = Guid.NewGuid().ToString();

                                var enterprise = new RelationEnterprise();
                                Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, enterprise);
                                enterprise.ID = model.ID ?? Guid.NewGuid().ToString();
                                enterprise.PersonID = person.ID;
                                model.ID = enterprise.ID;
                                model.PersonID = person.ID;
                                enterpriselist.Add(enterprise);
                            }
                        }
                    }
                }
                return enterpriselist;
            }

            #endregion 联系人信息

            //获取所有抵押物
            public List<Collateral> GetCollateralAsync(BaseCaseViewModel basecase)
            {
                var collaterallist = new List<Collateral>();
                if (basecase.Collateral != null && basecase.Collateral.Any())
                {
                    foreach (var model in basecase.Collateral)
                    {
                        if (model == null) break;
                        model.ID = Guid.NewGuid().ToString();
                        var collateral = new Collateral();
                        Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, collateral);
                        collateral.ID = model.ID ?? Guid.NewGuid().ToString();
                        collateral.CaseID = basecase.ID;
                        collateral.TotalHeight = model.TotalHeight;
                        model.ID = collateral.ID;
                        model.CaseID = basecase.ID;
                        collaterallist.Add(collateral);
                    }
                }
                return collaterallist;
            }

            //获取所有介绍人
            public List<Introducer> GetIntroducerAsync(BaseCaseViewModel basecase)
            {
                var Introducerlist = new List<Introducer>();
                if (basecase.Introducer != null && basecase.Introducer.Any())
                {
                    foreach (var model in basecase.Introducer)
                    {
                        if (model == null) break;

                        model.ID = Guid.NewGuid().ToString();
                        var Introducer = new Introducer();
                        Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, Introducer);
                        Introducer.ID = model.ID ?? Guid.NewGuid().ToString();
                        Introducer.CaseID = basecase.ID;
                        model.ID = Introducer.ID;
                        model.CaseID = basecase.ID;
                        Introducerlist.Add(Introducer);
                    }
                }
                return Introducerlist;
            }

            #endregion 新增进件

            #region 编辑进件

            public bool UpdateBaseCase(BaseCaseViewModel model, bool Accept, bool isPreSubmit = false)
            {
                if (model == null)
                {
                    return false;
                }

                var baseCase = GetBaseCaseAsync(model);

                if (model.RelationPerson != null && model.RelationPerson.Any())
                {
                    var relationperson = model.RelationPerson.Where(x => x.RelationType == "-PersonType-JieKuanRen").Select(x => x.Name);
                    baseCase.BorrowerName = relationperson.Any() ? relationperson.First() : model.RelationPerson.First().Name;
                }

                var relationPersonlist = relationPersonDAL.FindByCaseID(model.ID);
                if (model.RelationPerson != null && model.RelationPerson.Any())
                {
                    var relationPersonDAL = new RelationPersonDAL();
                    var relationPersons = relationPersonDAL.GetAll(x => x.CaseID == model.ID && x.IsLocked == true);
                    ICollection<RelationPersonViewModel> relationPersonsNew = new List<RelationPersonViewModel>();
                    foreach (var item in model.RelationPerson)
                    {
                        if (!relationPersons.Any(x => x.ID == item.ID))
                        {
                            relationPersonsNew.Add(item);

                            // 预提交的锁住
                            if (isPreSubmit)
                                item.IsLocked = true;
                        }
                    }
                    model.RelationPerson = relationPersonsNew;
                }
                relationPersonlist = relationPersonlist.Where(x => x.IsLocked != true);// 去掉被锁住的，被锁住的不更新，不删除

                var collaterallist = collateralDAL.FindByCaseID(model.ID);
                if (model.Collateral != null && model.Collateral.Any() && collaterallist != null)
                {
                    ICollection<CollateralViewModel> collateralNew = new List<CollateralViewModel>();
                    foreach (var item in model.Collateral)
                    {
                        var exist = collaterallist.FirstOrDefault(x => x.ID == item.ID);
                        if (exist == null || (exist != null && item.IsLocked != true))
                        {
                            collateralNew.Add(item);

                            // 预提交的锁住
                            if (isPreSubmit)
                                item.IsLocked = true;
                        }
                    }
                    model.Collateral = collateralNew;
                }
                collaterallist = collaterallist.Where(x => x.IsLocked != true);// 去掉被锁住的，被锁住的不更新，不删除

                var relationPerson = GetRelationPersonAsync(model);

                var contact = GetContactAsync(model);
                var relationEnterprise = GetRelationEnterpriseAsync(model);
                var emergencyContact = GetEmergencyContactAsync(model);
                var address = GetAddressAsync(model);
                var collateral = GetCollateralAsync(model);

                //获取介绍人
                var introducer = GetIntroducerAsync(model);
                baseCaseDAL.Update(baseCase);

                relationPersonDAL.DeleteRange(relationPersonlist);//删除关系人
                relationPersonDAL.AddRange(relationPerson);//添加新的关系人

                relationEnterpriseDAL.AddRange(relationEnterprise);
                contactDAL.AddRange(contact);
                emergencyContactDAL.AddRange(emergencyContact);
                addressDAL.AddRange(address);

                collateralDAL.DeleteRange(collaterallist);//删除抵押物
                collateralDAL.AddRange(collateral);//添加新的抵押物

                var Introducerlist = IntroducerDAL.FindByCaseID(model.ID);
                IntroducerDAL.DeleteRange(Introducerlist);//删除介绍人
                IntroducerDAL.AddRange(introducer);//添加新的介绍人
                if (Accept)
                {
                    //事务性提交
                    baseCaseDAL.AcceptAllChange();
                }
                return true;
            }

            #endregion 编辑进件

            #region 获取进件

            /// <summary>
            /// 获取进件
            /// </summary>
            /// <param name="id"></param>
            /// <param name="user"></param>
            /// <returns></returns>
            public async Task<BaseCaseViewModel> FindByID(string id, Infrastructure.Identity.Model.User user)
            {
                //获取基础信息
                var baseCase = await SetBaseCaseAsync(id, user);
                if (baseCase == null) return null;
                var refperson = await SetRelationPersonAsync(id);
                var collateral = await SetCollateralAsync(id);
                var introducer = await SetIntroducerAsync(id);

                SetContactAsync(refperson);
                SetEmergencyContactAsync(refperson);
                SetRelationEnterpriseAsync(refperson);
                SetAddressAsync(refperson);
                //if (refperson != null && refperson.Any())
                //{
                //    var borrowerperson = refperson.FirstOrDefault(t => t.RelationType == "-PersonType-JieKuanRen");
                //    if (borrowerperson != null)
                //    {
                //        refperson.Remove(borrowerperson);
                //        baseCase.BorrowerPerson = borrowerperson;
                //    }
                //}
                if (refperson != null) baseCase.RelationPerson = refperson.OrderBy(p => p.Sequence).ToList();
                baseCase.Collateral = collateral.OrderBy(p => p.Sequence).ToList();
                baseCase.Introducer = introducer.OrderBy(p => p.Sequence).ToList();

                return baseCase;
            }

            //基础信息
            public Task<BaseCaseViewModel> SetBaseCaseAsync(string id, Infrastructure.Identity.Model.User user)
            {
                return Task.Run(() =>
                {
                    var basecase = this.baseCaseDAL.GetAuthorizeAndSelf(id, user);
                    if (basecase == null)
                    {
                        return null;
                    }
                    var basemodel = new BaseCaseViewModel().CastModel(basecase);
                    //拷贝所有DBmodel 和 ViewModel 的属性
                    //Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(basecase, basemodel);
                    basemodel.ID = basecase.ID ?? Guid.NewGuid().ToString();
                    basemodel.CaseNum = basecase.NewCaseNum;
                    basemodel.TermText = dicdal.GetText(basemodel.Term);
                    basemodel.IsActivitieRateText = basemodel.IsActivitieRate == null ? "" : basemodel.IsActivitieRate == 0 ? "否" : "是";
                    return basemodel;
                });
            }

            //抵押物
            protected Task<List<CollateralViewModel>> SetCollateralAsync(string caseid)
            {
                return Task.Run(() =>
                {
                    var collatera = this.collateralDAL.FindByCaseID(caseid);
                    var collateralList = new List<CollateralViewModel>();
                    foreach (var model in collatera)
                    {
                        var collateral = new CollateralViewModel().CastModel(model);
                        collateral.HouseFileName = GetFiles(model.HouseFile, true);
                        collateral.CaseID = caseid;
                        //{
                        //    ID = model.ID,
                        //    Address = model.Address,
                        //    BuildingName = model.BuildingName,
                        //    CollateralType = model.CollateralType,
                        //    CollateralTypeText = dicdal.GetText(model.CollateralType),
                        //    CaseID = caseid,
                        //    HouseFile = model.HouseFile,
                        //    HouseFileName = GetFiles(model.HouseFile),
                        //    HouseNumber = model.HouseNumber,
                        //    HouseSize = model.HouseSize,
                        //    RightOwner = model.RightOwner,
                        //    Sequence = model.Sequence
                        //};
                        collateralList.Add(collateral);
                    }
                    return collateralList.ToList();
                });
            }

            //介绍人
            protected Task<List<IntroducerViewModel>> SetIntroducerAsync(string caseid)
            {
                return Task<List<IntroducerViewModel>>.Run(() =>
                {
                    var Introducer = this.IntroducerDAL.FindByCaseID(caseid);
                    var IntroducerList = new List<IntroducerViewModel>();
                    foreach (var model in Introducer)
                    {
                        var Introduce = new IntroducerViewModel()
                        {
                            ID = model.ID,
                            Account = model.Account,
                            AccountBank = model.AccountBank,
                            Contract = model.Contract,
                            Name = model.Name,
                            RebateAmmount = model.RebateAmmount,
                            CaseID = caseid,
                            RebateRate = model.RebateRate,
                            Sequence = model.Sequence
                        };
                        IntroducerList.Add(Introduce);
                    }
                    return IntroducerList.OrderBy(p => p.Sequence).ToList();
                });
            }

            #region 联系人信息

            protected Task<List<RelationPersonViewModel>> SetRelationPersonAsync(string caseid)
            {
                return Task.Run(() =>
                {
                    var person = relationPersonDAL.FindByCaseID(caseid);

                    var personlist = new List<RelationPersonViewModel>();
                    foreach (var model in person)
                    {
                        var per = new RelationPersonViewModel()
                        {
                            ID = model.ID,
                            Birthday = model.Birthday,
                            CaseID = caseid,
                            BorrowerRelation = model.BorrowerRelation,
                            BorrowerRelationText = dicdal.GetText(model.BorrowerRelation),
                            ExpiryDate = model.ExpiryDate,
                            MaritalStatus = model.MaritalStatus,
                            MaritalStatusText = dicdal.GetText(model.MaritalStatus),
                            //IsMarried = model.IsMarried,
                            //IsMarriedText = model.IsMarried == null ? "" : model.IsMarried == 0 ? "否" : "是",
                            Name = model.Name,
                            AccountFile = model.AccountFile,
                            AccountFileName = GetFiles(model.AccountFile, true),
                            BirthFile = model.BirthFile,
                            BirthFileName = GetFiles(model.BirthFile, true),
                            IdentificationFile = model.IdentificationFile,
                            IdentificationFileName = GetFiles(model.IdentificationFile, true),
                            IdentificationNumber = model.IdentificationNumber,
                            IdentificationType = model.IdentificationType,
                            IdentificationTypeText = dicdal.GetText(model.IdentificationType),
                            MarryFile = model.MarryFile,
                            MarryFileName = GetFiles(model.MarryFile, true),
                            RelationType = model.RelationType,
                            RelationTypeText = dicdal.GetText(model.RelationType),
                            SalaryDescription = model.SalaryDescription,
                            SalaryPersonFile = model.SalaryPersonFile,
                            SalaryPersonFileName = GetFiles(model.SalaryPersonFile, true),
                            SelfLicenseFile = model.SelfLicenseFile,
                            SelfLicenseFileName = GetFiles(model.SelfLicenseFile, true),
                            SelfNonLicenseFile = model.SelfNonLicenseFile,
                            SelfNonLicenseFileName = GetFiles(model.SelfNonLicenseFile, true),
                            SingleFile = model.SingleFile,
                            SingleFileName = GetFiles(model.SingleFile, true),
                            Warranty = model.Warranty,
                            IsCoBorrower = model.IsCoBorrower,
                            IsCoBorrowerText = model.IsCoBorrower == null ? "" : model.IsCoBorrower == 0 ? "否" : "是",
                            BankFlowFile = model.BankFlowFile,
                            BankFlowFileName = GetFiles(model.BankFlowFile, true),
                            IndividualFile = model.IndividualFile,
                            IndividualFileName = GetFiles(model.IndividualFile, true),
                            OtherFile = model.OtherFile,
                            OtherFileName = GetFiles(model.OtherFile, true),
                            Sequence = model.Sequence,
                            IsLocked = model.IsLocked
                        };
                        personlist.Add(per);
                    }
                    return personlist.OrderBy(p => p.Sequence).ToList();
                });
            }

            /// <summary>
            /// 获取联系人联系方式
            /// </summary>
            /// <param name="relationpersonid">联系人集合</param>
            protected void SetContactAsync(IEnumerable<RelationPersonViewModel> relationpersonid)
            {
                if (relationpersonid != null && relationpersonid.Any())
                {
                    foreach (var person in relationpersonid)
                    {
                        var contact = this.contactDAL.GetByPersonID(person.ID);
                        if (contact.Any())
                        {
                            var contactlist = new List<ContactViewModel>();
                            foreach (var model in contact)
                            {
                                var cont = new ContactViewModel
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
                            person.Contacts = contactlist.OrderBy(p => p.Sequence);
                        }
                    }
                }
            }

            /// <summary>
            /// 获取联系人地址
            /// </summary>
            /// <param name="relationpersonid">联系人集合</param>
            public void SetAddressAsync(IEnumerable<RelationPersonViewModel> relationpersonid)
            {
                if (relationpersonid != null && relationpersonid.Any())
                {
                    foreach (var person in relationpersonid)
                    {
                        var addresses = addressDAL.GetByPersonID(person.ID);
                        if (addresses != null && addresses.Any())
                        {
                            var addresslist = new List<AddressViewModel>();
                            foreach (var model in addresses)
                            {
                                var add = new AddressViewModel()
                                {
                                    ID = model.ID,
                                    AddressInfo = model.AddressInfo,
                                    AddressType = model.AddressType,
                                    AddressTypeText = dicdal.GetText(model.AddressType),
                                    PersonID = person.ID,
                                    IsDefault = model.IsDefault,
                                    Sequence = model.Sequence
                                };
                                addresslist.Add(add);
                            }
                            person.Addresses = addresslist.OrderBy(p => p.Sequence);
                        }
                    }
                }
            }

            /// <summary>
            /// 获取联系人企业信息
            /// </summary>
            /// <param name="relationpersonid">联系人集合</param>
            protected void SetRelationEnterpriseAsync(IEnumerable<RelationPersonViewModel> relationpersonid)
            {
                if (relationpersonid != null && relationpersonid.Any())
                {
                    foreach (var person in relationpersonid)
                    {
                        var enterprise = this.relationEnterpriseDAL.GetByPersonID(person.ID);
                        if (enterprise != null && enterprise.Any())
                        {
                            var enterpriselist = new List<RelationEnterpriseViewModel>();
                            foreach (var model in enterprise)
                            {
                                var enter = new RelationEnterpriseViewModel()
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
                                    BankFlowFileName = GetFiles(model.BankFlowFile, true),
                                    IndividualFile = model.IndividualFile,
                                    IndividualFileName = GetFiles(model.IndividualFile, true),
                                    Sequence = model.Sequence
                                };
                                enterpriselist.Add(enter);
                            }
                            person.RelationEnterprise = enterpriselist.OrderBy(p => p.Sequence);
                        }
                    }
                }
            }

            /// <summary>
            /// 紧急联系人集合
            /// </summary>
            /// <param name="relationpersonid"></param>
            protected void SetEmergencyContactAsync(IEnumerable<RelationPersonViewModel> relationpersonid)
            {
                if (relationpersonid != null && relationpersonid.Any())
                {
                    foreach (var person in relationpersonid)
                    {
                        var emergency = this.emergencyContactDAL.GetByPersonID(person.ID);
                        if (emergency != null && emergency.Any())
                        {
                            var emergencylist = new List<EmergencyContactViewModel>();
                            foreach (var model in emergency)
                            {
                                var emergencyContact = new EmergencyContactViewModel()
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
                            person.EmergencyContacts = emergencylist.OrderBy(p => p.Sequence);
                        }
                    }
                }
            }

            #endregion 联系人信息

            #endregion 获取进件
        }

        #region 辅助方法

        private List<BaseCaseListViewModel> FromBaseAudit(Expression<Func<BaseAudit, bool>> expression, int pageSize, int pageIndex, out int total)
        {
            IQueryable<BaseAudit> audits = _baseAuditBll.Query(CurrentUser, expression);
            var saleGroups = new SalesGroupBll().GetAll().ToList();

            total = audits.Count();

            IEnumerable<BaseAudit> auditsPage = _baseAuditBll.Query(new QueryByPageInput()
            {
                Audits = audits,
                Order = null,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Sort = null
            });

            IEnumerable<BaseCaseListViewModel> caseViewModels = auditsPage.Select(p => new BaseCaseListViewModel()
            {
                ID = _baseCaseDal.GetAll().First(t => t.NewCaseNum == p.NewCaseNum).ID,
                CaseNum = p.NewCaseNum,
                CaseType = p.CaseType,
                SalesID = p.SalesID,
                SalesGroupID = p.SalesGroupID,
                SalesGroupText = saleGroups.Single(sale => sale.ID == p.SalesGroupID).Name,
                DistrictID = p.DistrictID,
                BorrowerName = p.BorrowerName,
                LoanAmount = p.LoanAmount,
                Term = p.Term,
                TermText = "",
                //OpeningBank = p.OpeningBank,
                //OpeningSite = p.OpeningSite,
                //BankCard = p.BankCard,
                //ServiceCharge = p.ServiceCharge,
                //ServiceChargeRate = p.ServiceChargeRate,
                //Deposit = p.Deposit,
                //DepositDate = p.DepositDate,
                //IsActivitieRate = p.IsActivitieRate,
                CreateTime = p.CreateTime,
                Version = 1,
                CaseStatusText = CaseStatusHelper.GetBigStatusText(p.CaseStatus),
                CaseStatus = p.CaseStatus,// 所有不是未提交状态均为1,方便前端判断
            });

            return caseViewModels.ToList();
        }

        #endregion 辅助方法

        #endregion 进件辅助对象
    }
}