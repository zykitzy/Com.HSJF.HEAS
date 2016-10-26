using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.Lendings;
using Com.HSJF.Framework.DAL.Mortgage;
using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.DAL.Sales;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.HEAS.BLL.FinishedCase;
using Com.HSJF.HEAS.Web.Helper;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Audit;
using Com.HSJF.HEAS.Web.Models.FinishedCase;
using Com.HSJF.Infrastructure.ExtendTools;
using Com.HSJF.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Com.HSJF.HEAS.Web.Controllers
{
    /// <summary>
    /// 已结清案件信息
    /// </summary>
    [Authorize(Roles = "Finish")]
    public class FinishedCaseController : BaseController
    {
        private readonly FinishedCaseBll _finishedCaseBll;

        public FinishedCaseController()
        {
            _finishedCaseBll = new FinishedCaseBll();
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns>首页</returns>
        [HttpGet]
        public ActionResult FinishCaseIndex()
        {
            return View();
        }

        /// <summary>
        /// 分页获取结清案件列表
        /// </summary>
        /// <param name="request">查询条件</param>
        /// <returns>分页列表</returns>
        [HttpPost]
        public ActionResult GetFinishedCases(GetFinishedCaseRequest request)
        {
            int total;

            {
                request.PageIndex = request.PageIndex == 0 ? 1 : request.PageIndex;
                request.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            }

            IEnumerable<BaseAudit> cases = _finishedCaseBll.GetFinishedCases(request.Map(), out total);

            var response = new BaseAuditListPageResponseViewModel()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Total = total,
                TotalPage = (total.ToDouble() / request.PageSize).Ceiling().ToInt32(),
                Data = cases.Select(t => new BaseAuditViewModel().CastModel(t))
            };

            return Json(response);
        }

        /// <summary>
        /// 获取结清案件的具体信息
        /// </summary>
        /// <param name="id">案件ID</param>
        /// <returns>案件信息</returns>
        [HttpGet]
        public ActionResult ViewFinishedCase(string id)
        {
            BaseAudit finishedCase = _finishedCaseBll.GetFinishedCase(id);

            var summary = new FinishedCaseSummary
            {
                Id = finishedCase.ID,
                CaseNum = finishedCase.CaseNum,
                CaseStatusText = CaseStatusHelper.GetStatsText(finishedCase.CaseStatus)
            };

            return View(summary);
        }

        /// <summary>
        /// 获取案件详情
        /// </summary>
        /// <param name="id">案件ID</param>
        /// <returns>案件信息</returns>
        public ActionResult GetFinishedCase(string id)
        {
            BaseAudit finishedCase = _finishedCaseBll.GetFinishedCase(id);

            var viewModel = Map(finishedCase);

            var response = new BaseResponse<FinishedCaseViewModel>()
            {
                Data = viewModel,
                Status = "Success"
            };

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #region Private Methods

        private FinishedCaseViewModel Map(BaseAudit baseAudit)
        {
            var borrow = baseAudit.RelationPersonAudits.FirstOrDefault(t => t.RelationType == "-PersonType-JieKuanRen");
            var firstContacter = borrow.IfNotNull(p => p.ContactAudits.FirstOrDefault(t => t.IsDefault));
            var lendCase = new LendingDAL().Get(baseAudit.ID);
            var mortgage = new MortgageDAL().GetAll().Single(p => p.BaseAudit.CaseNum == baseAudit.CaseNum);
            var dictionaryDal = new DictionaryDAL();


            var finishedCase = new FinishedCaseViewModel();

            lendCase.CopyTo(finishedCase);

            finishedCase.ID = baseAudit.ID;
            finishedCase.CaseNum = baseAudit.CaseNum;
            finishedCase.CaseStatusText = CaseStatusHelper.GetBigStatusText(baseAudit.CaseStatus);
            finishedCase.IsCurrent = baseAudit.CaseStatus == CaseStatus.FinishCase;
            finishedCase.Borrower = borrow.IsNotNull() ? borrow.Name : "";
            finishedCase.ContactNumber = firstContacter.IsNotNull() ? firstContacter.ContactNumber : "";
            finishedCase.BankCard = baseAudit.BankCard;
            finishedCase.OpeningBank = baseAudit.OpeningBank;
            finishedCase.OpeningSite = baseAudit.OpeningSite;
            finishedCase.ContractAmount = mortgage.ContractAmount;
            finishedCase.SalesID = baseAudit.SalesID;
            finishedCase.SalesIDText = new SalesManDAL().FindBySalesID(baseAudit.SalesID).Name;
            finishedCase.Description = baseAudit.Description;
            finishedCase.CreateTime = baseAudit.CreateTime;
            finishedCase.ServiceCharge = baseAudit.ServiceCharge;
            finishedCase.ServiceChargeRate = baseAudit.ServiceChargeRate;
            finishedCase.Deposit = baseAudit.Deposit;
            finishedCase.DepositDate = baseAudit.DepositDate;
            finishedCase.IsActivitieRate = baseAudit.IsActivitieRate;
            finishedCase.IsActivitieRateText = dictionaryDal.GetText(baseAudit.IsActivitieRate.ToString());
            finishedCase.Introducer = GetIntroducer(baseAudit);
            finishedCase.Merchandiser = baseAudit.Merchandiser;
            finishedCase.LenderName = baseAudit.LenderName;
            finishedCase.EarnestMoney = baseAudit.EarnestMoney;
            finishedCase.OutboundCost = baseAudit.OutboundCost;
            finishedCase.DebitNotarizationCost = baseAudit.DebitNotarizationCost;
            finishedCase.DebitEvaluationCost = baseAudit.DebitEvaluationCost;
            finishedCase.DebitGuaranteeCost = baseAudit.DebitGuaranteeCost;
            finishedCase.DebitInsuranceCost = baseAudit.DebitInsuranceCost;
            finishedCase.DebitOtherCost = baseAudit.DebitOtherCost;
            finishedCase.LevyNotarizationCost = baseAudit.LevyNotarizationCost;
            finishedCase.LevyAssetsSurveyCost = baseAudit.LevyAssetsSurveyCost;
            finishedCase.LevyCreditReportCost = baseAudit.LevyCreditReportCost;
            finishedCase.LevyOtherCost = baseAudit.LevyOtherCost;
            finishedCase.CaseMode = baseAudit.CaseMode;
            finishedCase.CaseModeText = dictionaryDal.GetText(baseAudit.CaseMode);
            finishedCase.ThirdParty = baseAudit.ThirdParty;
            finishedCase.ThirdPartyText = dictionaryDal.GetText(baseAudit.ThirdParty);
            finishedCase.MonthlyInterest = baseAudit.MonthlyInterest;
            finishedCase.LendingDate = baseAudit.LendingDate;
            finishedCase.PaymentDate = baseAudit.PaymentDate;
            finishedCase.ActualInterest = baseAudit.ActualInterest;
            finishedCase.AdvanceInterest = baseAudit.AdvanceInterest;
            finishedCase.AuditTerm = baseAudit.AuditTerm;
            finishedCase.AuditTermText = dictionaryDal.GetText(baseAudit.AuditTerm);
            finishedCase.AuditRate = baseAudit.AuditRate;

            finishedCase.LendFileName = GetFiles(finishedCase.LendFile);

            return finishedCase;
        }

        private IEnumerable<IntroducerAuditViewModel> GetIntroducer(BaseAudit baseAudit)
        {
            var introducer = new IntroducerAuditDAL();

            var introducerAudit = introducer.FindByAuditID(baseAudit.ID);
            var introducerAuditList = introducerAudit.Select(model =>
                new IntroducerAuditViewModel
                {
                    ID = model.ID,
                    Account = model.Account,
                    AccountBank = model.AccountBank,
                    AuditID = baseAudit.ID,
                    Contract = model.Contract,
                    Name = model.Name,
                    RebateAmmount = model.RebateAmmount,
                    RebateRate = model.RebateRate,
                    Sequence = model.Sequence
                }).ToList();
            return introducerAuditList.OrderBy(p => p.Sequence).ToList();
        }


        #endregion
    }
}