using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.Lendings;
using Com.HSJF.Framework.DAL.Mortgage;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.DAL.Sales;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.EntityFramework.Model.Lending;
using Com.HSJF.HEAS.BLL.Other;
using Com.HSJF.HEAS.BLL.Other.Dto;
using Com.HSJF.HEAS.BLL.Sales;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Audit;
using Com.HSJF.HEAS.Web.Models.BaseModel;
using Com.HSJF.HEAS.Web.Models.Lendings;
using Com.HSJF.Infrastructure.ExtendTools;
using Com.HSJF.Infrastructure.Extensions;
using Com.HSJF.Infrastructure.File;
using Com.HSJF.Infrastructure.Utility;
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
    [Authorize(Roles = "Finance")]
    public class LendingController : BaseController
    {
        private readonly SalesGroupBll _salesGroupBll;

        public LendingController()
        {
            _salesGroupBll = new SalesGroupBll();
        }

        [Authorize]
        public ActionResult LendingIndex()
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
        public ActionResult GetPageIndex(LendingCriteriaRequest request)
        {
            BaseAuditDAL bd = new BaseAuditDAL();
            LendingDAL ld = new LendingDAL();
            int total = 0;

            request.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            request.PageIndex = request.PageIndex == 0 ? 1 : request.PageIndex;

            var modellist = ld.GetAllAuthorizeAndSelf(request.StartDate, request.EndDate, CurrentUser);

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

            IEnumerable<BaseAudit> pageList = bd.GetAllPage(modellist, out total, request.PageSize, request.PageIndex, request.Order, request.Sort);

            var response = new PageResponseViewModel<LendingPageViewModel>();

            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Total = total;
            response.TotalPage = (int)Math.Ceiling((decimal)response.Total / response.PageSize);
            response.Data = Map(pageList.ToList());
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditLending(string id)
        {
            LendingDAL led = new LendingDAL();
            var lem = led.GetAuthorizeAndSelf(id, CurrentUser);
            var morvm = new LendingViewModel();
            if (lem != null)
            {
                morvm = morvm.CastModel(lem);
            }
            else
            {
                BaseAuditDAL bad = new BaseAuditDAL();
                MortgageDAL md = new MortgageDAL();
                var audit = bad.GetMaxAuditAuthorizeAndSelf(id, CurrentUser);

                if (audit == null)
                {
                    RedirectToAction("Failed", "Home");
                }
                var mor = md.GetAll().FirstOrDefault(t => t.BaseAudit.CaseNum == audit.CaseNum);
                if (mor == null)
                {
                    RedirectToAction("Failed", "Home");
                }
                var borrower = audit.RelationPersonAudits.FirstOrDefault(t => t.RelationType == "-PersonType-JieKuanRen");
                if (borrower == null)
                {
                    RedirectToAction("Failed", "Home");
                }
                lem = new Lending();
                //lem.Borrower = borrower.Name;
                //lem.ContactNumber = borrower.ContactAudits.FirstOrDefault(t => t.IsDefault) == null ? "" : borrower.ContactAudits.FirstOrDefault(t => t.IsDefault).ContactNumber;
                //lem.BorrowNumber = audit.BankCard;
                //lem.OpeningBank = audit.OpeningBank;
                //lem.ContractAmount = mor.ContractAmount;
                lem.ID = id;

                morvm = morvm.CastModel(lem);
            }
            ViewBag.ID = id;
            return View(morvm);
        }

        public ActionResult GetLending(string id)
        {
            LendingDAL bd = new LendingDAL();
            DictionaryDAL dicdal = new DictionaryDAL();
            SalesManDAL Sales = new SalesManDAL();

            BaseResponse<LendingViewModel> br = new BaseResponse<LendingViewModel>();
            var mor = bd.Get(id);
            var morvm = new LendingViewModel();
            if (mor != null)
            {
                morvm = morvm.CastModel(mor);
                morvm.LendFileName = GetFiles(morvm.LendFile);
                morvm.Introducer = GetIntroducer(id);
                morvm.IsActivitieRateText = dicdal.GetText(morvm.IsActivitieRate.ToString());
                morvm.CaseModeText = dicdal.GetText(morvm.CaseMode);
                morvm.ThirdPartyText = dicdal.GetText(morvm.ThirdParty);
                morvm.AuditTermText = dicdal.GetText(morvm.AuditTerm);
                morvm.SalesIDText = Sales.FindBySalesID(morvm.SalesID) == null ? null : Sales.FindBySalesID(morvm.SalesID).Name;
            }
            else
            {
                mor = new Lending();
                mor.ID = id;
                morvm = morvm.CastModel(mor);
                morvm.Introducer = GetIntroducer(id);
                morvm.IsActivitieRateText = dicdal.GetText(morvm.IsActivitieRate.ToString());
                morvm.CaseModeText = dicdal.GetText(morvm.CaseMode);
                morvm.ThirdPartyText = dicdal.GetText(morvm.ThirdParty);
                morvm.AuditTermText = dicdal.GetText(morvm.AuditTerm);
                morvm.SalesIDText = Sales.FindBySalesID(morvm.SalesID) == null ? null : Sales.FindBySalesID(morvm.SalesID).Name;
            }
            br.Data = morvm;
            br.Status = "Success";
            return Json(br, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交放款
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitLending(LendingViewModel model)
        {
            var response = new BaseResponse<LendingViewModel>();
            var relationPersonauditDal = new RelationPersonAuditDAL();
            var baseAuditDal = new BaseAuditDAL();
            //var relationstatebll = new RelationStateBLL();

            #region 验证

            List<ErrorMessage> em = new List<ErrorMessage>();
            if (!ModelState.IsValid)
            {
                foreach (var e in ModelState.Keys)
                {
                    if (ModelState[e].Errors.Any())
                    {
                        if (e.Contains("FileName")) continue;
                        if (string.IsNullOrEmpty(ModelState[e].Errors[0].ErrorMessage)) continue;
                        var error = new ErrorMessage();
                        error.Key = e;
                        error.Message = ModelState[e].Errors[0].ErrorMessage;
                        em.Add(error);
                    }
                }
            }
            if (em.Any())
            {
                response.Status = "Failed";
                response.Message = em.ToArray();
                response.Data = null;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion 验证
            var baseaduit = baseAuditDal.GetAuthorizeAndSelf(model.ID, CurrentUser);
            if (baseaduit == null && baseaduit.CaseStatus != CaseStatus.Lending)
            {
                response.Status = "Failed";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            var db = new Lending();
            LendingDAL bd = new LendingDAL();
            db = model.CastDB(model);
            // 合同文件
            db.LendFile = SaveFiles(model.LendFile, model.ID, model.ID);

            //IEnumerable<RelationPersonAudit> relationPersonList = relationPersonauditDal.FindByCaseID(baseaduit.ID).ToList();
            //if (relationPersonList != null && relationPersonList.Any())
            //{
            //    foreach (var item in relationPersonList)
            //    {
            //        var relationbllModel = new RelationStateBLLModel()
            //        {
            //            Number = item.IdentificationNumber,
            //            Desc = "Bind"
            //        };
            //        relationstatebll.UpdateLockRelationState(relationbllModel);
            //    }
            //}

            var pushResult = PushToHats(model.CaseNum);
            if (pushResult.IsSuccess)
            {
                bd.SubmitCase(db, CurrentUser.UserName, model.Description);
                bd.AcceptAllChange();
                response.Status = "Success";
            }
            else
            {
                response.Status = StatusEnum.Failed.ToString();

                if (pushResult.Message.IsNullOrWhiteSpace() || pushResult.Message.IsNullOrWhiteSpace())
                {
                    response.Message = new ErrorMessage[] { new ErrorMessage("", "推送失败") };
                }
                else
                {
                    response.Message = new ErrorMessage[] { new ErrorMessage("", pushResult.Message) };
                }
            }

            return Json(response);
        }

        /// <summary>
        /// 拒绝放款
        /// </summary>
        /// <param name="id">案件id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RejectLending(string id)
        {
            var lendingDal = new LendingDAL();
            var response = new BaseResponse<LendingViewModel>();
            if (lendingDal.RejectCase(id, CurrentUser))
            {
                response.Status = StatusEnum.Success.ToString();
            }
            else
                response.Status = "Failed";
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 附件信息保存
        /// </summary>
        /// <param name="filenames">文件名</param>
        private string SaveFiles(string filenames, string linkId, string linkkey)
        {
            var up = new FileUpload();//文件上传
            string files = string.Empty;
            if (string.IsNullOrEmpty(filenames))
            {
                return files;
            }
            foreach (var file in filenames.Split(','))
            {
                var filemodel = up.Single(new Guid(file));
                if (filemodel != null)
                {
                    var entity = CopyFile(filemodel);
                    entity.LinkID = Guid.Parse(linkId);
                    entity.LinkKey = linkkey;
                    up.SaveFileDescription(entity);
                    files += entity.ID;
                    files += ",";
                }
            }
            return files.Trim(',');
        }

        private FileDescription CopyFile(FileDescription model)
        {
            var entity = new FileDescription();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, entity);
            entity.ID = Guid.NewGuid();
            entity.FileCreateTime = DateTime.Now;
            return entity;
        }

        private List<IntroducerAuditViewModel> GetIntroducer(string id)
        {
            var bad = new BaseAuditDAL();
            var introducer = new IntroducerAuditDAL();
            var baseaduit = bad.Get(id);
            if (baseaduit == null)
            {
                return null;
            }
            var introducerAudit = introducer.FindByAuditID(baseaduit.ID);
            var introducerAuditList = new List<IntroducerAuditViewModel>();
            foreach (var model in introducerAudit)
            {
                var collateral = new IntroducerAuditViewModel()
                {
                    ID = model.ID,
                    Account = model.Account,
                    AccountBank = model.AccountBank,
                    AuditID = baseaduit.ID,
                    Contract = model.Contract,
                    Name = model.Name,
                    RebateAmmount = model.RebateAmmount,
                    RebateRate = model.RebateRate,
                    Sequence = model.Sequence
                };
                introducerAuditList.Add(collateral);
            }
            return introducerAuditList.OrderBy(p => p.Sequence).ToList();
        }

        private List<LendingPageViewModel> Map(List<BaseAudit> audits)
        {
            var output = new List<LendingPageViewModel>();
            var baseAuditDal = new BaseAuditDAL();
            var saleGroups = new SalesGroupBll().GetAll().ToList();

            audits.ForEach(p =>
            {
                var lendModel = new LendingPageViewModel();

                lendModel.ID = p.ID;
                lendModel.BorrowerName = p.BorrowerName;
                lendModel.CaseNum = p.NewCaseNum;
                lendModel.CaseStatus = baseAuditDal.GetbyCaseNum(p.NewCaseNum).CaseStatus;
                lendModel.CaseStatusText = Helper.CaseStatusHelper.GetBigStatusText(lendModel.CaseStatus);
                lendModel.CreateTime = p.CreateTime;
                lendModel.LendingDate = p.LendingDate;
                lendModel.LoanAmount = p.LoanAmount;
                lendModel.SalesGroupID = p.SalesGroupID;
                lendModel.SalesGroupText = saleGroups.Single(sale => sale.ID == p.SalesGroupID).Name;

                output.Add(lendModel);
            });

            return output;
        }

        private AddBaseAuditByLendingResponse PushToHats(string caseNum)
        {
            string hatsHost = ConfigurationManager.AppSettings["hats_host"];

            var securityRequest = new SecurityRequest()
            {
                RequestData = caseNum.ToHatsString()
            };

            var request = new HttpItem()
            {
                URL = string.Format("{0}/api/BaseAuditPush/AddBaseAuditByLending", hatsHost),
                Method = "post",
                ContentType = "application/json; charset=utf-8",
                Postdata = securityRequest.ToJson(),
                Accept = "text/json",
                PostEncoding = Encoding.UTF8
            };

            var httpResult = new HttpHelper().GetHtml(request);
            if (httpResult.StatusCode == HttpStatusCode.OK)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<AddBaseAuditByLendingResponse>(httpResult.Html);
            }
            else
            {
                return new AddBaseAuditByLendingResponse()
                {
                    IsSuccess = false,
                    Message = "Hats接口调用错误"
                };
            }
        }

        /// <summary>
        /// 放款只读页面
        /// </summary>
        /// <returns></returns>

        public ActionResult ReadonlyLending(string id)
        {
            LendingDAL led = new LendingDAL();
            var lem = led.GetAuthorizeAndSelf(id, CurrentUser);
            var morvm = new LendingViewModel();
            if (lem != null)
            {
                morvm = morvm.CastModel(lem);
            }
            else
            {
                BaseAuditDAL bad = new BaseAuditDAL();
                MortgageDAL md = new MortgageDAL();
                var audit = bad.GetMaxAuditAuthorizeAndSelf(id, CurrentUser);

                if (audit == null)
                {
                    RedirectToAction("Failed", "Home");
                }
                var mor = md.GetAll().FirstOrDefault(t => t.BaseAudit.CaseNum == audit.CaseNum);
                if (mor == null)
                {
                    RedirectToAction("Failed", "Home");
                }
                var borrower = audit.RelationPersonAudits.FirstOrDefault(t => t.RelationType == "-PersonType-JieKuanRen");
                if (borrower == null)
                {
                    RedirectToAction("Failed", "Home");
                }
                lem = new Lending();
                //lem.Borrower = borrower.Name;
                //lem.ContactNumber = borrower.ContactAudits.FirstOrDefault(t => t.IsDefault) == null ? "" : borrower.ContactAudits.FirstOrDefault(t => t.IsDefault).ContactNumber;
                //lem.BorrowNumber = audit.BankCard;
                //lem.OpeningBank = audit.OpeningBank;
                //lem.ContractAmount = mor.ContractAmount;
                lem.ID = id;

                morvm = morvm.CastModel(lem);
            }
            ViewBag.ID = id;
            return View(morvm);
        }
    }

    public class AddBaseAuditByLendingResponse
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}