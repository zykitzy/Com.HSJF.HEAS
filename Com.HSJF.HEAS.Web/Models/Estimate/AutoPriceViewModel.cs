namespace Com.HSJF.HEAS.Web.Models.Estimate
{
    /// <summary>
    /// 报价
    /// </summary>
    public class AutoPriceViewModel
    {
        /// <summary>
        /// 自动估价结果
        /// </summary>
        public double UnitPrice { get; set; }

        /// <summary>
        /// 案例均价
        /// </summary>
        public double AvagePrice { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// 案例最大值
        /// </summary>
        public double MaxPrice { get; set; }

        /// <summary>
        /// 案例最小值
        /// </summary>
        public double MinPrice { get; set; }

        /// <summary>
        /// 自动估价异常情况文字说明，正常估价改字段为空
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 面积
        /// </summary>
        public double BuildArea { get; set; }

        /// <summary>
        /// 估价状态值
        /// </summary>
        public int ReturnStatus { get; set; }
    }
}