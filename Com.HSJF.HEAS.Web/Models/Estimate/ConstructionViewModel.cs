namespace Com.HSJF.HEAS.Web.Models.Estimate
{
    /// <summary>
    /// 楼盘
    /// </summary>
    public class ConstructionViewModel
    {
        /// <summary>
        /// 楼盘ID
        /// </summary>
        public int ConstructionId { get; set; }

        /// <summary>
        /// 楼盘名称
        /// </summary>
        public string ConstructionName { get; set; }

        /// <summary>
        /// 楼盘别名
        /// </summary>
        public string SaleName { get; set; }
    }
}