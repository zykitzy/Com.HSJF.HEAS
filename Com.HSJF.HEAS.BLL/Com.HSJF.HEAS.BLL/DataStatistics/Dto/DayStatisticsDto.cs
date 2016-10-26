
namespace Com.HSJF.HEAS.BLL.DataStatistics.Dto
{
    /// <summary>
    /// 每日数据统计
    /// </summary>
    public class DayStatisticsDto
    {

        /// <summary>
        /// 分公司Id
        /// </summary>
        public string SalesGroupId { get; set; }

        /// <summary>
        /// 分公司
        /// </summary>
        public string SalesGroupName { get; set; }

        /// <summary>
        /// 进件数量
        /// </summary>
        public int BaseCaseCount { get; set; }

        /// <summary>
        /// 进件申请总金额
        /// </summary>
        public decimal BaseCaseAmount { get; set; }

        /// <summary>
        /// 审核数量
        /// </summary>
        public int AuditCaseCount { get; set; }

        /// <summary>
        /// 审核总金额
        /// </summary>
        public decimal AuditCaseAmount { get; set; }

        /// <summary>
        /// 签约数量
        /// </summary>
        public int PublicCaseCount { get; set; }

        /// <summary>
        /// 签约总金额
        /// </summary>
        public decimal PublicCaseAmount { get; set; }

        /// <summary>
        /// 放款数量
        /// </summary>
        public int AfterCaseCount { get; set; }

        /// <summary>
        /// 放款总金额
        /// </summary>
        public decimal AfterCaseAmount { get; set; }

        /// <summary>
        /// 分公司月放款总额
        /// </summary>
        public decimal MonthAfterCaseAmount { get; set; }

        /// <summary>
        /// 分公司月放款总数
        /// </summary>
        public int MonthAfterCaseCount { get; set; }
    }
}
