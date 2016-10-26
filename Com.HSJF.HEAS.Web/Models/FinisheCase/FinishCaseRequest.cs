
namespace Com.HSJF.HEAS.Web.Models.FinisheCase
{
    /// <summary>
    /// 结清案件请求
    /// </summary>
    public class FinishCaseRequest
    {
        /// <summary>
        /// 案件号
        /// </summary>
        public string CaseNum { get; set; }

        /// <summary>
        /// 案件结清时间
        /// </summary>
        public string FinishTime { get; set; }
    }
}