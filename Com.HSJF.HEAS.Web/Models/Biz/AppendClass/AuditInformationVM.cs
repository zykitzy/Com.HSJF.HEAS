using System;

namespace Com.HSJF.HEAS.Web.Models.Biz.AppendClass
{
    public class AuditInformationVM
    {
        /// <summary>
        /// 审批金额
        /// </summary>
        public decimal? ThirdPartyAuditAmount { get; set; }

        /// <summary>
        /// 审批贷款期限
        /// </summary>
        public string ThirdPartyAuditTerm { get; set; }

        /// <summary>
        /// 审批利率（年）
        /// </summary>
        public decimal? ThirdPartyAuditRate { get; set; }

        /// <summary>
        ///  审批拒绝理由
        /// </summary>
        public string RefuseReason { get; set; }

        /// <summary>
        ///  审批拒绝描述
        /// </summary>
        public string RefuseDescription { get; set; }

        /// <summary>
        /// 签约金额
        /// </summary>
        public decimal? ContractAmount { get; set; }

        /// <summary>
        /// 签约日期
        /// </summary>
        public DateTime? ContractDate { get; set; }

        /// <summary>
        /// 经办人文本
        /// </summary>
        public string ContractPersonText { get; set; }

        /// <summary>
        /// 签约拒绝理由
        /// </summary>
        public string SignRefuseReason { get; set; }
    }
}