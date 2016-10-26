
namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    public class EnterpriseCreditDto
    {
        public string ID { get; set; }

        public string EnterpriseID { get; set; }

        public string EnterpriseIDText { get; set; }

        /// <summary>
        /// 企业贷款卡
        /// </summary>
        public string CreditCard { get; set; }

        /// <summary>
        /// 企业负债信息
        /// </summary>
        public string CreditInfo { get; set; }

        /// <summary>
        /// 股东情况
        /// </summary>
        public string ShareholderDetails { get; set; }

        /// <summary>
        /// 其他情况说明
        /// </summary>
        public string OtherDetailes { get; set; }

        /// <summary>
        /// 审核ID
        /// </summary>
        public string BaseAuditID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }
    }
}
