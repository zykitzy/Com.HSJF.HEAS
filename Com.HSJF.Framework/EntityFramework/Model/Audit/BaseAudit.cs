using Com.HSJF.Framework.EntityFramework.Model.Mortgage;
using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit
{
    public class BaseAudit : EntityModel
    {
        public BaseAudit()
        {
            this.IndividualCredits = new List<IndividualCredit>();
            this.EnterpriseCredits = new List<EnterpriseCredit>();
            this.EnforcementPersons = new List<EnforcementPerson>();
            this.IndustryCommerceTaxs = new List<IndustryCommerceTax>();
            this.HouseDetails = new List<HouseDetail>();
            this.Guarantors = new List<Guarantor>();
            this.CollateralAudits = new List<CollateralAudit>();
            this.RelationPersonAudits = new List<RelationPersonAudit>();
            this.IntroducerAudits = new List<IntroducerAudit>();
        }
        //继承进件 BaseCase
        public string ID { get; set; }

        [Obsolete("请使用NewCaseNum")]
        public string CaseNum { get; set; }

        /// <summary>
        /// 新的案件编号
        /// </summary>
        public string NewCaseNum { get; set; }
        public string CaseType { get; set; }
        public string SalesID { get; set; }
        public string SalesGroupID { get; set; }
        public string DistrictID { get; set; }
        public string BorrowerName { get; set; }
        public int Version { get; set; }
        public decimal? LoanAmount { get; set; }

        //以下 2-16-06-16 第一次测试之后新增
        public string Term { get; set; }
        public string Partner { get; set; }

        public decimal? AnnualRate { get; set; }

        [Obsolete("废弃")]
        public decimal? PlatformCharge { get; set; }
        public decimal? ComprehensiveRate { get; set; }
        public string MortgageOrder { get; set; }
        public string CaseDetail { get; set; }

        public string OpeningBank { get; set; }
        public string OpeningSite { get; set; }
        public string BankCard { get; set; }

        /// <summary>
        /// 批注意见
        /// </summary>
        public string AuditComment { get; set; }
        /// <summary>
        /// 备注，拒绝批注
        /// </summary>
        public string Description { get; set; }
        public string RejectType { get; set; }

        // 2016-06-27 再次新增
        public decimal? ServiceCharge { get; set; }
        public decimal? ServiceChargeRate { get; set; }
        public decimal? Deposit { get; set; }
        public DateTime? DepositDate { get; set; }
        public int? IsActivitieRate { get; set; }
        public string Merchandiser { get; set; }
        public string LenderName { get; set; }
        public decimal? EarnestMoney { get; set; }
        public decimal? OutboundCost { get; set; }
        public decimal? DebitNotarizationCost { get; set; }
        public decimal? DebitEvaluationCost { get; set; }
        public decimal? DebitGuaranteeCost { get; set; }
        public decimal? DebitInsuranceCost { get; set; }
        public decimal? DebitOtherCost { get; set; }
        public decimal? LevyNotarizationCost { get; set; }
        public decimal? LevyAssetsSurveyCost { get; set; }
        public decimal? LevyCreditReportCost { get; set; }
        public decimal? LevyOtherCost { get; set; }

        /// <summary>
        /// 审批金额
        /// </summary>
        public decimal? AuditAmount { get; set; }

        /// <summary>
        /// 审批贷款期限
        /// </summary>
        public string AuditTerm { get; set; }

        /// <summary>
        /// 审批利率
        /// </summary>
        public decimal? AuditRate { get; set; }



        /// <summary>
        /// 案件模式
        /// </summary>
        public string CaseMode { get; set; }

        /// <summary>
        /// 第三方资金
        /// </summary>
        public string ThirdParty { get; set; }

        /// <summary>
        /// 月利息金额
        /// </summary>
        public decimal? MonthlyInterest { get; set; }

        /// <summary>
        /// 放款日期
        /// </summary>
        public DateTime? LendingDate { get; set; }

        /// <summary>
        /// 回款日期
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 实收利息
        /// </summary>
        public decimal? ActualInterest { get; set; }

        /// <summary>
        /// 预收利息
        /// </summary>
        public decimal? AdvanceInterest { get; set; }


        #region 居间信息

        /// <summary>
        /// 审批金额
        /// </summary>
        public decimal? ThirdPartyAuditAmount { get; set; }

        /// <summary>
        /// 审批贷款期限
        /// </summary>
        public string ThirdPartyAuditTerm { get; set; }

        /// <summary>
        /// 审批利率
        /// </summary>
        public decimal? ThirdPartyAuditRate { get; set; }

        #endregion


        public int? IsNeedReport { get; set; }

        /// <summary>
        /// 面谈报告
        /// </summary>
        public string FaceReportFile { get; set; }

        /// <summary>
        /// 现场报告
        /// </summary>
        public string FieldReportFile { get; set; }

        /// <summary>
        /// 贷前尽调报告
        /// </summary>
        public string LoanDetailReportFile { get; set; }


        //2016-09-08 大改
        /// <summary>
        ///  还款来源
        /// </summary>
        public string PaymentFactor { get; set; }
        /// <summary>
        /// 借款用途
        /// </summary>
        public string Purpose { get; set; }
        /// <summary>
        /// 放款条件
        /// </summary>
        public string LendingTerm { get; set; }
        /// <summary>
        /// 签约要件
        /// </summary>
        public string ContractFileInfo { get; set; }
        /// <summary>
        /// 拒绝理由，拒绝批注使用Description
        /// </summary>
        public string RejectReason { get; set; }
        /// <summary>
        /// 借款申请书
        /// </summary>
        public string LoanProposedFile { get; set; }

        //2016-10-08 
        /// <summary>
        /// 客户保证金
        /// </summary>
        public decimal? CustEarnestMoney { get; set; }

        public string CaseStatus { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateUser { get; set; }
        public virtual ICollection<CollateralAudit> CollateralAudits { get; set; }
        public virtual ICollection<RelationPersonAudit> RelationPersonAudits { get; set; }


        public virtual ICollection<IndividualCredit> IndividualCredits { get; set; }
        public virtual ICollection<EnterpriseCredit> EnterpriseCredits { get; set; }
        public virtual ICollection<EnforcementPerson> EnforcementPersons { get; set; }
        public virtual ICollection<IndustryCommerceTax> IndustryCommerceTaxs { get; set; }
        public virtual ICollection<HouseDetail> HouseDetails { get; set; }
        public virtual ICollection<Guarantor> Guarantors { get; set; }
        public virtual ICollection<IntroducerAudit> IntroducerAudits { get; set; }
        //公证抵押
        public virtual PublicMortgage PublicMortgage { get; set; }
        //放款
        public virtual Com.HSJF.Framework.EntityFramework.Model.Lending.Lending Lending { get; set; }

    }
}
