
namespace Com.HSJF.HEAS.Web.Models.FinishedCase
{
    /// <summary>
    /// 案件摘要信息
    /// </summary>
    public class FinishedCaseSummary
    {
        /// <summary>
        /// 案件ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 案件编码
        /// </summary>
        public string CaseNum { get; set; }

        /// <summary>
        /// 案件状态
        /// </summary>
        public string CaseStatusText { get; set; }
    }
}