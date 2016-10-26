
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    public class BaseAuditCaseDto
    {
        public string ID { get; set; }

        /// <summary>
        /// 业务编号
        /// </summary>
        public string CaseNum { get; set; }

        /// <summary>
        /// 借款类型
        /// </summary>
        public string CaseType { get; set; }

        /// <summary>
        /// 借款类型text
        /// </summary>
        public string CaseTypeText { get; set; }

        /// <summary>
        /// 销售人员Id
        /// </summary>
        public string SalesID { get; set; }

        /// <summary>
        /// 销售组Id
        /// </summary>
        public string SalesGroupID { get; set; }

        /// <summary>
        /// 地区Id
        /// </summary>
        public string DistrictID { get; set; }

        /// <summary>
        /// 借款人姓名
        /// </summary>
        public string BorrowerName { get; set; }

        /// <summary>
        /// 借款期限
        /// </summary>
        public string Term { get; set; }

        /// <summary>
        /// 借款期限text
        /// </summary>
        public string TermText { get; set; }

        /// <summary>
        /// 合作???
        /// </summary>
        public string Partner { get; set; }

        /// <summary>
        /// 审批金额
        /// </summary>
        public decimal? AuditAmount { get; set; }

        /// <summary>
        /// 年化利率
        /// </summary>
        [Obsolete("迁移至进件")]
        public decimal? AnnualRate { get; set; }

        /// <summary>
        /// 平台费用
        /// </summary>
        public decimal? PlatformCharge { get; set; }

        /// <summary>
        /// 综合抵押率
        /// </summary>
        public decimal? ComprehensiveRate { get; set; }

        /// <summary>
        /// 一抵/二抵
        /// </summary>
        public string MortgageOrder { get; set; }

        public string MortgageOrderText { get; set; }

        /// <summary>
        /// 案件描述
        /// </summary>
        public string CaseDetail { get; set; }

        /// <summary>
        /// 审核期限
        /// </summary>
        public string AuditTerm { get; set; }

        public string AuditTermText { get; set; }

        /// <summary>
        /// 审核利率
        /// </summary>
        public decimal? AuditRate { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string OpeningBank { get; set; }

        /// <summary>
        /// 开户名称
        /// </summary>
        public string OpeningSite { get; set; }

        /// <summary>
        /// 银行卡
        /// </summary>
        public string BankCard { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>
        public string AuditComment { get; set; }

        /// <summary>
        /// 退回或者通过需要填注意见时可以使用此字段
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 拒绝理由
        /// </summary>
        public string RejectType { get; set; }

        /// <summary>
        /// 服务费
        /// </summary>
        public decimal? ServiceCharge { get; set; }

        /// <summary>
        /// 服务费点数
        /// </summary>
        public decimal? ServiceChargeRate { get; set; }

        /// <summary>
        /// 客户已支付金额
        /// </summary>
        public decimal? Deposit { get; set; }

        /// <summary>
        /// 客户支付定金日期
        /// </summary>
        public DateTime? DepositDate { get; set; }

        /// <summary>
        /// 是否为活动期间的优惠利率
        /// </summary>
        public int? IsActivitieRate { get; set; }

        public string IsActivitieRateText { get; set; }

        /// <summary>
        /// 数据版本
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// 申请金额
        /// </summary>
        public decimal? LoanAmount { get; set; }

        /// <summary>
        /// 借款人
        /// </summary>
        public RelationPersonAuditDto BorrowerPerson { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        //审核
        /// <summary>
        /// 本次业务是否需要出具评估
        /// </summary>
        [Display(Name = "本次业务是否需要出具评估")]
        public string IsNeedReport { get; set; }

        /// <summary>
        /// 案件状态
        /// </summary>
        public string CaseStatus { get; set; }

        public string CaseStatusText { get; set; }

        /// <summary>
        /// 免谈报告
        /// </summary>
        public string FaceReportFile { get; set; }

        public Dictionary<string, string> FaceReportFileName { get; set; }

        /// <summary>
        /// 现场报告
        /// </summary>
        public string FieldReportFile { get; set; }

        public Dictionary<string, string> FieldReportFileName { get; set; }

        /// <summary>
        /// 贷前尽调报告
        /// </summary>
        public string LoanDetailReportFile { get; set; }

        public Dictionary<string, string> LoanDetailReportFileName { get; set; }

        /// <summary>
        /// 跟单人
        /// </summary>
        public string Merchandiser { get; set; }

        /// <summary>
        /// 出借人姓名
        /// </summary>
        [Display(Name = "出借人姓名")]
        public string LenderName { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public decimal? EarnestMoney { get; set; }

        /// <summary>
        /// 外访费（下户费）
        /// </summary>
        public decimal? OutboundCost { get; set; }

        /// <summary>
        /// 代收公证费用
        /// </summary>
        public decimal? DebitNotarizationCost { get; set; }

        /// <summary>
        /// 代收评估费
        /// </summary>
        public decimal? DebitEvaluationCost { get; set; }

        /// <summary>
        /// 代收担保费
        /// </summary>
        public decimal? DebitGuaranteeCost { get; set; }

        /// <summary>
        /// 代收保险费
        /// </summary>
        public decimal? DebitInsuranceCost { get; set; }

        /// <summary>
        /// 代收其他
        /// </summary>
        public decimal? DebitOtherCost { get; set; }

        /// <summary>
        /// 公司承担的公证费
        /// </summary>
        public decimal? LevyNotarizationCost { get; set; }

        /// <summary>
        /// 公司承担的产调费
        /// </summary>
        public decimal? LevyAssetsSurveyCost { get; set; }

        /// <summary>
        /// 公司承担的信用报告费
        /// </summary>
        public decimal? LevyCreditReportCost { get; set; }

        /// <summary>
        /// 公司承担的其他费用
        /// </summary>
        public decimal? LevyOtherCost { get; set; }

        //新增审核字段
        /// <summary>
        /// 案件模式
        /// </summary>
        public string CaseMode { get; set; }

        /// <summary>
        /// 第三方平台
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
        /// 实收利息（不退客户）
        /// </summary>
        public decimal? ActualInterest { get; set; }

        /// <summary>
        /// 预收利息（可退客户）
        /// </summary>
        public decimal? AdvanceInterest { get; set; }

        /// <summary>
        /// 案件历史状态
        /// </summary>
        public IEnumerable<AuditHistoryDto> AuditHistory { get; set; }

        /// <summary>
        /// 抵押物信息集合
        /// </summary>
        public virtual IEnumerable<CollateralAuditDto> CollateralAudits { get; set; }

        /// <summary>
        /// 关系人信息集合
        /// </summary>
        public virtual ICollection<RelationPersonAuditDto> RelationPersonAudits { get; set; }

        /// <summary>
        /// 个人资信情况
        /// </summary>
        public virtual IEnumerable<IndividualCreditDto> IndividualCredits { get; set; }

        /// <summary>
        /// 企业资信情况
        /// </summary>
        public virtual IEnumerable<EnterpriseCreditDto> EnterpriseCredits { get; set; }

        /// <summary>
        /// 被执行人情况
        /// </summary>
        public virtual IEnumerable<EnforcementPersonDto> EnforcementPersons { get; set; }

        /// <summary>
        /// 工商税务情况
        /// </summary>
        public virtual IEnumerable<IndustryCommerceTaxDto> IndustryCommerceTaxs { get; set; }

        /// <summary>
        /// 房屋明细
        /// </summary>
        public virtual IEnumerable<HouseDetailDto> HouseDetails { get; set; }

        /// <summary>
        /// 担保人
        /// </summary>
        public virtual IEnumerable<GuarantorDto> Guarantors { get; set; }

        /// <summary>
        /// 介绍人
        /// </summary>
        public virtual IEnumerable<IntroducerAuditDto> Introducer { get; set; }
    }
}
