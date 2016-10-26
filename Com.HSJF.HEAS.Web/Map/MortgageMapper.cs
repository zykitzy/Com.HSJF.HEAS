using System.Linq;
using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.EntityFramework.Model.Audit;
using Com.HSJF.Framework.EntityFramework.Model.Mortgage;
using Com.HSJF.HEAS.Web.Helper;
using Com.HSJF.HEAS.Web.Models.Mortgage;
using Com.HSJF.Infrastructure.Extensions;
using Com.HSJF.Infrastructure.Mapper;

namespace Com.HSJF.HEAS.Web.Map
{
    public static class MortgageMapper
    {

        /// <summary>
        /// 映射签约信息到viewmodel
        /// </summary>
        /// <param name="baseAudit">基础案件信息</param>
        /// <param name="publicMortgage">签约信息</param>
        /// <returns>新的签约ViewModel</returns>
        public static PublicMortgageViewModel MapToViewModel(BaseAudit baseAudit, PublicMortgage publicMortgage)
        {
            var mortgage = new PublicMortgageViewModel();

            if (publicMortgage.IsNotNull())
            {
                mortgage = Mapper.Map<PublicMortgage, PublicMortgageViewModel>(publicMortgage);

            }
            if (mortgage != null)
            {
                mortgage.AuditAmount = baseAudit.AuditAmount;
                mortgage.CaseNum = baseAudit.NewCaseNum;
                mortgage.CaseStatus = baseAudit.CaseStatus;
                mortgage.CaseStatusText = CaseStatusHelper.GetStatsText(baseAudit.CaseStatus);
                mortgage.IsCurrent = baseAudit.CaseStatus == CaseStatus.PublicMortgage;

                mortgage.OpeningBank = baseAudit.OpeningBank;
                mortgage.OpeningSite = baseAudit.OpeningSite;
                mortgage.BankCard = baseAudit.BankCard;

                mortgage.ServiceCharge = baseAudit.ServiceCharge;
                mortgage.ServiceChargeRate = baseAudit.ServiceChargeRate;
                mortgage.Deposit = baseAudit.Deposit;
                mortgage.DepositDate = baseAudit.DepositDate;
                mortgage.IsActivitieRate = baseAudit.IsActivitieRate;
                //更新第三方信息
                mortgage.CaseMode = baseAudit.CaseMode;
                mortgage.ThirdParty = baseAudit.ThirdParty;
                mortgage.ThirdPartyAuditAmount = baseAudit.ThirdPartyAuditAmount;
                mortgage.ThirdPartyAuditRate = baseAudit.ThirdPartyAuditRate;
                mortgage.ThirdPartyAuditTerm = baseAudit.ThirdPartyAuditTerm;
                //失败理由
                mortgage.RejectReason = baseAudit.RejectReason;

                mortgage.LenderName = baseAudit.LenderName;

                baseAudit.IntroducerAudits.IfNotNull(t =>
                {
                    mortgage.Introducer = baseAudit.IntroducerAudits.Select(p => p.MaptoIntroduceAuditViewModel()).ToList().OrderBy(b => b.Sequence);
                });

                return mortgage;
            }
            return mortgage;
        }

    }
}