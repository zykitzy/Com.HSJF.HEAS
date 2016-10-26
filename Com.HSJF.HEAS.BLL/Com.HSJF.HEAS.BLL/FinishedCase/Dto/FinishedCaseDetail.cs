using System;
using System.Collections.Generic;
using Com.HSJF.Framework.EntityFramework.Model.Biz;

namespace Com.HSJF.HEAS.BLL.FinishedCase.Dto
{
    /// <summary>
    /// 已还清案件详细信息
    /// </summary>
    public class FinishedCaseDetail
    {
        /// <summary>
        /// 案件ID
        /// </summary>
        public string ID { get; set; }

        
        /// <summary>
        /// 放款证明
        /// </summary>
        public string LendFile { get; set; }

        /// <summary>
        /// 放款文件
        /// </summary>
        public Dictionary<string, string> LendFileName { get; set; }

        /// <summary>
        /// 案件号
        /// </summary>
        public string CaseNum { get; set; }

        /// <summary>
        /// 案件状态
        /// </summary>
        public string CaseStatusText { get; set; }

        public bool IsCurrent { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// 借款人
        /// </summary>
        public string Borrower { get; set; }

        /// <summary>
        /// 借款人帐号
        /// </summary>
        public string BankCard { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string OpeningBank { get; set; }

        /// <summary>
        /// 开户名称
        /// </summary>
        public string OpeningSite { get; set; }

        /// <summary>
        /// 放款金额
        /// </summary>
        public decimal? ContractAmount { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 销售人员
        /// </summary>
        public string SalesID { get; set; }

        public string SalesIDText { get; set; }

        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime? CreateTime { get; set; }

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
        /// 介绍人集合
        /// </summary>
        public IEnumerable<Introducer> Introducer { get; set; }

        /// <summary>
        /// 跟单人
        /// </summary>
        public string Merchandiser { get; set; }

        /// <summary>
        /// 出借人姓名
        /// </summary>
        public string LenderName { get; set; }

        /// <summary>
        /// 出借人姓名
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

        /// <summary>
        /// 案件模式
        /// </summary>
        public string CaseMode { get; set; }

        public string CaseModeText { get; set; }

        /// <summary>
        /// 第三方平台
        /// </summary>
        public string ThirdParty { get; set; }

        public string ThirdPartyText { get; set; }

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
        /// 审核期限
        /// </summary>
        public string AuditTerm { get; set; }

        public string AuditTermText { get; set; }

        /// <summary>
        /// 审核利率
        /// </summary>
        public decimal? AuditRate { get; set; }
    }
}
