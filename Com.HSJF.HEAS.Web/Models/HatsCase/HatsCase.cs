namespace Com.HSJF.HEAS.Web.Models.HatsCase
{
    public class HatsCase
    {
        /// <summary>
        /// 案件编号
        /// </summary>
        public string CaseNum { get; set; }

        /// <summary>
        /// 案件模式
        /// </summary>
        public string TransferType { get; set; }

        /// <summary>
        /// 第三方Id
        /// </summary>
        public string ThirdParty { get; set; }

        /// <summary>
        /// 保证金
        /// </summary>
        public decimal? EarnestMoney { get; set; }

        /// <summary>
        /// 第三方利率(heas审批利率)
        /// </summary>
        public decimal? TransferInterest { get; set; }

        /// <summary>
        /// 第三方转让期限(heas审批期限)
        /// </summary>
        public string TransferTerm { get; set; }

        /// <summary>
        /// 第三方转让金额(heas审批金额)
        /// </summary>
        public decimal? TransferAmount { get; set; }
    }
}