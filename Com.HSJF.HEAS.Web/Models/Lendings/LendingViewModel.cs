using Com.HSJF.Framework.DAL.Audit;
using Com.HSJF.Framework.DAL.Mortgage;
using Com.HSJF.Framework.EntityFramework.Model.Lending;
using Com.HSJF.HEAS.Web.Models.Audit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Com.HSJF.Framework.DAL.Other;

namespace Com.HSJF.HEAS.Web.Models.Lendings
{
    public class LendingViewModel
    {
        [Key]
        public string ID { get; set; }

        [Display(Name = "实际放款日期")]
        [Required]
        public DateTime? LendTime { get; set; }

        [Display(Name = "收款日")]
        [Required]
        public int? PaymentDay { get; set; }

        [Display(Name = "放款凭证")]
        [Required]
        public string LendFile { get; set; }

        public Dictionary<string, string> LendFileName { get; set; }

        [Display(Name = "案件号")]
        public string CaseNum { get; set; }

        [Display(Name = "案件状态")]
        public string CaseStatus { get; set; }

        public string CaseStatusText { get; set; }

        public bool IsCurrent { get; set; }

        [Display(Name = "借款人姓名")]
        public string CustomerName { get; set; }

        [Obsolete("2016-9-8d大改取消")]
        [Display(Name = "联系电话")]
        public string ContactNumber { get; set; }

        [Display(Name = "借款人")]
        public string Borrower { get; set; }

        [Display(Name = "借款人账号")]
        public string BankCard { get; set; }

        [Display(Name = "开户行")]
        public string OpeningBank { get; set; }

        [Display(Name = "开户名称")]
        public string OpeningSite { get; set; }
        public string OpeningSiteText { get; set; }

        [Display(Name = "放款金额")]
        public decimal? ContractAmount { get; set; }

        [Display(Name = "审批意见")]
        public string Description { get; set; }

        #region 2016-07-01 新增

        [Display(Name = "销售人员")]
        public string SalesID { get; set; }

        public string SalesIDText { get; set; }

        [Display(Name = "进件提交日期")]
        public DateTime? CreateTime { get; set; }

        [Display(Name = "服务费")]
        public decimal? ServiceCharge { get; set; }

        [Display(Name = "服务费率")]
        public decimal? ServiceChargeRate { get; set; }

        [Display(Name = "客户已支付金额")]
        public decimal? Deposit { get; set; }

        [Display(Name = "客户支付定金日期")]
        public DateTime? DepositDate { get; set; }

        [Display(Name = "是否为活动期间的优惠利率")]
        public int? IsActivitieRate { get; set; }

        public string IsActivitieRateText { get; set; }

        //介绍人集合
        public IEnumerable<IntroducerAuditViewModel> Introducer { get; set; }

        [Display(Name = "跟单人")]
        public string Merchandiser { get; set; }

        [Display(Name = "出借人姓名")]
        public string LenderName { get; set; }

        [Display(Name = "保证金")]
        public decimal? EarnestMoney { get; set; }

        [Display(Name = "外访费（下户费）")]
        public decimal? OutboundCost { get; set; }

        [Display(Name = "代收公证费用")]
        public decimal? DebitNotarizationCost { get; set; }

        [Display(Name = "代收评估费")]
        public decimal? DebitEvaluationCost { get; set; }

        [Display(Name = "代收担保费")]
        public decimal? DebitGuaranteeCost { get; set; }

        [Display(Name = "代收保险费")]
        public decimal? DebitInsuranceCost { get; set; }

        [Display(Name = "代收其他")]
        public decimal? DebitOtherCost { get; set; }

        [Display(Name = "公司承担的公证费")]
        public decimal? LevyNotarizationCost { get; set; }

        [Display(Name = "公司承担的产调费")]
        public decimal? LevyAssetsSurveyCost { get; set; }

        [Display(Name = "公司承担的信用报告费")]
        public decimal? LevyCreditReportCost { get; set; }

        [Display(Name = "公司承担的其他费用")]
        public decimal? LevyOtherCost { get; set; }

        [Display(Name = "案件模式")]
        public string CaseMode { get; set; }

        public string CaseModeText { get; set; }

        [Display(Name = "第三方平台")]
        public string ThirdParty { get; set; }

        public string ThirdPartyText { get; set; }

        [Display(Name = "月利息金额")]
        public decimal? MonthlyInterest { get; set; }

        [Display(Name = "放款日期")]
        public DateTime? LendingDate { get; set; }

        [Display(Name = "回款日期")]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "实收利息（不退客户）")]
        public decimal? ActualInterest { get; set; }

        [Display(Name = "预收利息（可退客户）")]
        public decimal? AdvanceInterest { get; set; }

        [Display(Name = "审核期限")]
        public string AuditTerm { get; set; }

        public string AuditTermText { get; set; }

        [Display(Name = "审核利率")]
        public decimal? AuditRate { get; set; }

        #endregion 2016-07-01 新增

        #region 2016-9-19 大改添加

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "审批金额")]
        public decimal? AuditAmount { get; set; }

        /// <summary>
        /// 第三方审批金额
        /// </summary>
        public decimal? ThirdPartyAuditAmount { get; set; }

        /// <summary>
        ///  第三方审批贷款期限
        /// </summary>

        public string ThirdPartyAuditTerm { get; set; }

        /// <summary>
        ///  第三方审批利率
        /// </summary>

        public decimal? ThirdPartyAuditRate { get; set; }

        #endregion 2016-9-19 大改添加

        //2016-10-08
        /// <summary>
        /// 客户保证金
        /// </summary>
        [Display(Name = "客户保证金")]
        public decimal? CustEarnestMoney { get; set; }

        public LendingViewModel CastModel(Lending model)
        {
            BaseAuditDAL bad = new BaseAuditDAL();
            MortgageDAL md = new MortgageDAL();
            var dicdal = new DictionaryDAL();
            var baseaudit = bad.Get(model.ID);
            var maxaudit = bad.GetMaxAudit(model.ID);
            var minaudit = bad.GetMinAudit(model.ID);
            var borrower = maxaudit.RelationPersonAudits.FirstOrDefault(t => t.RelationType == "-PersonType-JieKuanRen");
            var mor = md.GetAll().FirstOrDefault(t => t.BaseAudit.NewCaseNum == maxaudit.NewCaseNum);

            var baseauditRelaType = "";
            var maxauditRelaType = "";
            if (baseaudit != null)
            {
                if (!string.IsNullOrEmpty(baseaudit.OpeningSite))
                {
                    var baseauditRela = baseaudit.RelationPersonAudits.FirstOrDefault(t => t.IdentificationNumber == baseaudit.OpeningSite);
                    if (baseauditRela != null)
                    {
                        baseauditRelaType = baseauditRela.Name + "(" + dicdal.GetText(baseauditRela.RelationType) + ")";
                    }
                }
            }
            if (maxaudit!=null)
            {
                if (!string.IsNullOrEmpty(maxaudit.OpeningSite))
                {
                    var maxauditRela =maxaudit.RelationPersonAudits.FirstOrDefault(t => t.IdentificationNumber == maxaudit.OpeningSite);
                    if (maxauditRela != null)
                    {
                        maxauditRelaType = maxauditRela.Name + "(" + dicdal.GetText(maxauditRela.RelationType) + ")";
                    }
                }
            }
            
            LendingViewModel bcvm = new LendingViewModel();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, bcvm);
            bcvm.CaseNum = baseaudit == null ? maxaudit == null ? "" : maxaudit.NewCaseNum : baseaudit.NewCaseNum;
            bcvm.CaseStatus = baseaudit == null ? maxaudit == null ? "" : maxaudit.CaseStatus : baseaudit.CaseStatus;
            bcvm.CaseStatusText = baseaudit == null ? maxaudit == null ? "" : Com.HSJF.HEAS.Web.Helper.CaseStatusHelper.GetStatsText(maxaudit.CaseStatus) : Com.HSJF.HEAS.Web.Helper.CaseStatusHelper.GetStatsText(baseaudit.CaseStatus);
            bcvm.IsCurrent = baseaudit == null ? false : (baseaudit.CaseStatus == Com.HSJF.Framework.DAL.CaseStatus.Lending) ? true : false;
            //[Display(Name = "进件提交日期")]
            bcvm.CreateTime = baseaudit == null ? maxaudit == null ? null : maxaudit.CreateTime : baseaudit.CreateTime;
            bcvm.Borrower = borrower.Name;
            bcvm.ContactNumber = borrower.ContactAudits.FirstOrDefault(t => t.IsDefault) == null ? "" : borrower.ContactAudits.FirstOrDefault(t => t.IsDefault).ContactNumber;
            bcvm.BankCard = baseaudit == null ? maxaudit == null ? null : maxaudit.BankCard : baseaudit.BankCard;
            bcvm.OpeningBank = baseaudit == null ? maxaudit == null ? null : maxaudit.OpeningBank : baseaudit.OpeningBank;
            bcvm.OpeningSite = baseaudit == null ? maxaudit == null ? null : maxaudit.OpeningSite : baseaudit.OpeningSite;

            bcvm.OpeningSiteText = baseaudit == null ? maxaudit == null ? null : maxauditRelaType : baseauditRelaType;

            bcvm.ContractAmount = mor == null ? null : mor.ContractAmount;
            bcvm.SalesID = baseaudit == null ? maxaudit == null ? null : maxaudit.SalesID : baseaudit.SalesID;
            // [Display(Name = "服务费") 自动计算=放款金额*服务费率]
            bcvm.ServiceCharge = baseaudit == null ? maxaudit == null ? null : mor.ContractAmount * (maxaudit.ServiceChargeRate / 100) : mor.ContractAmount * (baseaudit.ServiceChargeRate / 100);
            // [Display(Name = "服务费点数")]
            bcvm.ServiceChargeRate = baseaudit == null ? maxaudit == null ? null : maxaudit.ServiceChargeRate : baseaudit.ServiceChargeRate;
            // [Display(Name = "客户已支付金额")]
            bcvm.Deposit = baseaudit == null ? maxaudit == null ? null : maxaudit.Deposit : baseaudit.Deposit;
            // [Display(Name = "客户支付定金日期")]
            bcvm.DepositDate = baseaudit == null ? maxaudit == null ? null : maxaudit.DepositDate : baseaudit.DepositDate;
            // [Display(Name = "是否为活动期间的优惠利率")]
            bcvm.IsActivitieRate = baseaudit == null ? maxaudit == null ? null : maxaudit.IsActivitieRate : baseaudit.IsActivitieRate;
            // [Display(Name = "跟单人")]
            bcvm.Merchandiser = baseaudit == null ? maxaudit == null ? null : maxaudit.Merchandiser : baseaudit.Merchandiser;
            // [Display(Name = "出借人姓名")]
            bcvm.LenderName = baseaudit == null ? maxaudit == null ? null : maxaudit.LenderName : baseaudit.LenderName;
            // [Display(Name = "保证金")]
            bcvm.EarnestMoney = baseaudit == null ? maxaudit == null ? null : maxaudit.EarnestMoney : baseaudit.EarnestMoney;
            // [Display(Name = "外访费（下户费）")]
            bcvm.OutboundCost = baseaudit == null ? maxaudit == null ? null : maxaudit.OutboundCost : baseaudit.OutboundCost;
            // [Display(Name = "代收公证费用")]
            bcvm.DebitNotarizationCost = baseaudit == null ? maxaudit == null ? null : maxaudit.DebitNotarizationCost : baseaudit.DebitNotarizationCost;
            // [Display(Name = "代收评估费")]
            bcvm.DebitEvaluationCost = baseaudit == null ? maxaudit == null ? null : maxaudit.DebitEvaluationCost : baseaudit.DebitEvaluationCost;
            // [Display(Name = "代收担保费")]
            bcvm.DebitGuaranteeCost = baseaudit == null ? maxaudit == null ? null : maxaudit.DebitGuaranteeCost : baseaudit.DebitGuaranteeCost;
            // [Display(Name = "代收保险费")]
            bcvm.DebitInsuranceCost = baseaudit == null ? maxaudit == null ? null : maxaudit.DebitInsuranceCost : baseaudit.DebitInsuranceCost;
            // [Display(Name = "代收其他")]
            bcvm.DebitOtherCost = baseaudit == null ? maxaudit == null ? null : maxaudit.DebitOtherCost : baseaudit.DebitOtherCost;
            // [Display(Name = "公司承担的公证费")]
            bcvm.LevyNotarizationCost = baseaudit == null ? maxaudit == null ? null : maxaudit.LevyNotarizationCost : baseaudit.LevyNotarizationCost;
            // [Display(Name = "公司承担的产调费")]
            bcvm.LevyAssetsSurveyCost = baseaudit == null ? maxaudit == null ? null : maxaudit.LevyAssetsSurveyCost : baseaudit.LevyAssetsSurveyCost;
            // [Display(Name = "公司承担的信用报告费")]
            bcvm.LevyCreditReportCost = baseaudit == null ? maxaudit == null ? null : maxaudit.LevyCreditReportCost : baseaudit.LevyCreditReportCost;
            // [Display(Name = "公司承担的其他费用")]
            bcvm.LevyOtherCost = baseaudit == null ? maxaudit == null ? null : maxaudit.LevyOtherCost : baseaudit.LevyOtherCost;
            // [Display(Name = "案件模式")]
            bcvm.CaseMode = baseaudit == null ? maxaudit == null ? null : maxaudit.CaseMode : baseaudit.CaseMode;
            // [Display(Name = "第三方平台")]
            bcvm.ThirdParty = baseaudit == null ? maxaudit == null ? null : maxaudit.ThirdParty : baseaudit.ThirdParty;
            // [Display(Name = "月利息金额")]
            bcvm.MonthlyInterest = baseaudit == null ? maxaudit == null ? null : maxaudit.MonthlyInterest : baseaudit.MonthlyInterest;
            // [Display(Name = "放款日期")]
            bcvm.LendingDate = baseaudit == null ? maxaudit == null ? null : maxaudit.LendingDate : baseaudit.LendingDate;
            // [Display(Name = "回款日期")]
            bcvm.PaymentDate = baseaudit == null ? maxaudit == null ? null : maxaudit.PaymentDate : baseaudit.PaymentDate;
            // [Display(Name = "实收利息（不退客户）")]
            bcvm.ActualInterest = baseaudit == null ? maxaudit == null ? null : maxaudit.ActualInterest : baseaudit.ActualInterest;
            // [Display(Name = "预收利息（可退客户）")]
            bcvm.AdvanceInterest = baseaudit == null ? maxaudit == null ? null : maxaudit.AdvanceInterest : baseaudit.AdvanceInterest;
            //审核期限
            bcvm.AuditTerm = baseaudit == null ? maxaudit == null ? null : maxaudit.AuditTerm : baseaudit.AuditTerm;
            //审核利率
            bcvm.AuditRate = baseaudit == null ? maxaudit == null ? null : maxaudit.AuditRate : baseaudit.AuditRate;

            //审批金额
            bcvm.AuditAmount = baseaudit == null ? maxaudit == null ? null : maxaudit.AuditAmount : baseaudit.AuditAmount;
            //第三方审批金额
            bcvm.ThirdPartyAuditAmount = baseaudit == null ? maxaudit == null ? null : maxaudit.ThirdPartyAuditAmount : baseaudit.ThirdPartyAuditAmount;
            //第三方审批期限
            bcvm.ThirdPartyAuditTerm = baseaudit == null ? maxaudit == null ? null : maxaudit.ThirdPartyAuditTerm : baseaudit.ThirdPartyAuditTerm;
            //第三方审批利率
            bcvm.ThirdPartyAuditRate = baseaudit == null ? maxaudit == null ? null : maxaudit.ThirdPartyAuditRate : baseaudit.ThirdPartyAuditRate;
            //客户保证金
            bcvm.CustEarnestMoney = baseaudit == null ? maxaudit == null ? null : maxaudit.CustEarnestMoney : baseaudit.CustEarnestMoney;
            return bcvm;
        }

        public Lending CastDB(LendingViewModel model)
        {
            Lending bc = new Lending();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, bc);
            return bc;
        }
    }
}