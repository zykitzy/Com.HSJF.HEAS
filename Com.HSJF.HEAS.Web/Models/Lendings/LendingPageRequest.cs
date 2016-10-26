using Com.HSJF.HEAS.Web.Models.BaseModel;
using System;

namespace Com.HSJF.HEAS.Web.Models.Lendings
{
    /// <summary>
    /// 放款列表页请求
    /// </summary>
    public class LendingCriteriaRequest : PageRequestViewModel
    {
        /// <summary>
        /// 借款人姓名
        /// </summary>
        public string BorrowerName { get; set; }

        /// <summary>
        /// 案件号
        /// </summary>
        public string CaseNum { get; set; }

        /// <summary>
        /// 案件状态
        /// </summary>
        public string CaseStatus { get; set; }

        /// <summary>
        /// 销售团队Id
        /// </summary>
        public string SalesGroupId { get; set; }

        /// <summary>
        /// 放款起始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 放款结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}