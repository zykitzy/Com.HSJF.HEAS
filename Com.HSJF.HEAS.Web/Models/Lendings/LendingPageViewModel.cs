
using System;

namespace Com.HSJF.HEAS.Web.Models.Lendings
{
    /// <summary>
    /// 放款列表
    /// </summary>
    public class LendingPageViewModel
    {
        /// <summary>
        /// id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 业务编号
        /// </summary>
        public string CaseNum { get; set; }

        /// <summary>
        /// 借款人姓名
        /// </summary>
        public string BorrowerName { get; set; }

        /// <summary>
        /// 申请金额
        /// </summary>
        public decimal? LoanAmount { get; set; }

        /// <summary>
        /// 放款日期
        /// </summary>
        public DateTime? LendingDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 案件状态
        /// </summary>
        public string CaseStatus { get; set; }

        /// <summary>
        /// 案件状态名称
        /// </summary>
        public string CaseStatusText { get; set; }

        /// <summary>
        /// 销售组Id
        /// </summary>
        public string SalesGroupID { get; set; }

        /// <summary>
        /// 销售组名称
        /// </summary>
        public string SalesGroupText { get; set; }
    }
}