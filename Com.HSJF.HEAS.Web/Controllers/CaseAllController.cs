using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.CaseALL;
using Com.HSJF.Framework.DAL.Lendings;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.DAL.Sales;
using Com.HSJF.HEAS.BLL.Mortgage;
using Com.HSJF.HEAS.Web.Helper;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.CaseAll;
using Com.HSJF.HEAS.Web.Models.Lendings;
using Com.HSJF.HEAS.Web.Models.Mortgage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Com.HSJF.HEAS.BLL.Sales;
using Com.HSJF.HEAS.Web.Models.Audit;
using Com.HSJF.Infrastructure;

namespace Com.HSJF.HEAS.Web.Controllers
{
    /// <summary>
    /// 查询所有案件
    /// </summary>
    [Authorize(Roles = "Observer")]
    public class CaseAllController : BaseController
    {
        // GET: CaseAll
        public ActionResult Index()
        {
            SalesGroupBll _salesGroupBll = new SalesGroupBll();
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

        /// <summary>
        /// 所有案件列表查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ActionResult GetPageIndex(CaseAllPageRequestModel request)
        {
            var bad = new ALLCaseDAL();
            var response = new CaseAllPageResponseModel();
            var list = bad.GetAll(CurrentUser);

            list = list.Where(s => !s.NewCaseNum.Contains("HIS"));
            if (!string.IsNullOrEmpty(request.CaseNum))
            {
                list = list.Where(s => s.NewCaseNum.Contains(request.CaseNum));
            }
            if (!string.IsNullOrEmpty(request.SalesGroupId))
            {
                list = list.Where(s => s.SalesGroupID == request.SalesGroupId);
            }
            if (!string.IsNullOrEmpty(request.CaseMode))
            {
                if (request.CaseMode == "-CaseMode-WeiXuanZe")
                {
                    list = list.Where(s => s.CaseMode == null);
                }
                else
                {
                    list = list.Where(s => s.CaseMode == request.CaseMode);
                }
            }
            if (!string.IsNullOrEmpty(request.ThirdParty))
            {
                list = list.Where(s => s.ThirdParty == request.ThirdParty);
            }
            if (!string.IsNullOrEmpty(request.BorrowerName))
            {
                list = list.Where(s => s.BorrowerName.Contains(request.BorrowerName));
            }
            if (!string.IsNullOrEmpty(request.CaseStatus))
            {
                list = list.Where(s => s.CaseStatus == request.CaseStatus);
            }
            if (!string.IsNullOrEmpty(request.LendTimeStart.ToString()))
            {
                list = list.Where(s => s.LendTime >= request.LendTimeStart);
            }
            if (!string.IsNullOrEmpty(request.LendTimeEnd.ToString()))
            {
                list = list.Where(s => request.LendTimeEnd != null && s.LendTime <= request.LendTimeEnd);
            }
            try
            {
                var caselist = list.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize);
                var newcaselist = caselist.Select(s => new CaseAllPageViewModel().CopyModel(s));
                response.PageIndex = request.PageIndex;
                response.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
                response.Total = list.Count();
                response.TotalPage = (int)Math.Ceiling((decimal)response.Total / response.PageSize);
                response.Data = newcaselist;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                response.PageIndex = request.PageIndex;
                response.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
                response.Total = 0;
                //response.TotalPage = (int)Math.Ceiling((decimal)response.Total / response.PageSize);
                response.Data = null;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ActionResult CaseDetails(string id)
        {
            ViewBag.ID = id;
            BaseAuditDAL bad = new BaseAuditDAL();

            var model = new BaseAuditViewModel().CastModel(bad.Get(id));
            if (model == null)
            {
                return RedirectToAction("Failed", "Home");
            }
            ViewBag.CaseStatus = model.CaseStatus;
            ViewBag.CaseNum = model.CaseNum;
            return View();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetCaseDetails(string id)
        {
            var response = new BaseResponse<CaseAllDetailsViewModel>();
            var ch = new AuditController.AuditHelper();
            var baseAuditDal = new BaseAuditDAL();
            var lendal = new LendingDAL();
            var dicdal = new DictionaryDAL();
            var sales = new SalesManDAL();
            var lendvm = new LendingViewModel();
            var mortgagevm = new PublicMortgageViewModel();
            var mortgageBll = new MortgageBll();
            var caseallmodel = new CaseAllDetailsViewModel();

            var baseauditvm = ch.FindByID(id, CurrentUser);
            if (baseauditvm != null)
            {
                var ahp = new AuditHisHelper();
                var list = baseAuditDal.GetListByCaseNum(baseauditvm.CaseNum);
                baseauditvm.AuditHistory = ahp.GetHistory(list);
            }
            var mort = mortgageBll.QueryById(id);
            if (mort != null)
            {
                mortgagevm = mortgagevm.CastModel(mort);
                mortgagevm.ContractFileName = GetFiles(mortgagevm.ContractFile);
                mortgagevm.OtherFileName = GetFiles(mortgagevm.OtherFile);
                mortgagevm.FourFileName = GetFiles(mortgagevm.FourFile);
                mortgagevm.RepaymentAttorneyFileName = GetFiles(mortgagevm.RepaymentAttorneyFile);
                mortgagevm.PowerAttorneyFileName = GetFiles(mortgagevm.PowerAttorneyFile);
                mortgagevm.CollectionFileName = GetFiles(mortgagevm.CollectionFile);
            }
            var lend = lendal.Get(id);
            if (lend != null)
            {
                lendvm = lendvm.CastModel(lend);
                lendvm.LendFileName = GetFiles(lendvm.LendFile);
                lendvm.IsActivitieRateText = dicdal.GetText(lendvm.IsActivitieRate.ToString());
                lendvm.CaseModeText = dicdal.GetText(lendvm.CaseMode);
                lendvm.ThirdPartyText = dicdal.GetText(lendvm.ThirdParty);
                lendvm.AuditTermText = dicdal.GetText(lendvm.AuditTerm);
                lendvm.SalesIDText = sales.FindBySalesID(lendvm.SalesID) == null ? null : sales.FindBySalesID(lendvm.SalesID).Name;
            }

            caseallmodel.Baseauditvm = baseauditvm;
            caseallmodel.Mortgagevm = mortgagevm;
            caseallmodel.Lendingvm = lendvm;
            response.Status = "Success";
            response.Data = caseallmodel;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}