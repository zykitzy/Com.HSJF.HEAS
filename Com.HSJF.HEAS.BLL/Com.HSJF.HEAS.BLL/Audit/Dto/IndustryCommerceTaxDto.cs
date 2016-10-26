
namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    /// <summary>
    /// 工商税务
    /// </summary>
    public class IndustryCommerceTaxDto
    {
        public string ID { get; set; }

        public string EnterpriseID { get; set; }

        public string EnterpriseIDText { get; set; }

        /// <summary>
        /// 企业年检情况
        /// </summary>
        public string AnnualInspection { get; set; }

        /// <summary>
        /// 公司实际经营
        /// </summary>
        public string ActualManagement { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// 审核ID
        /// </summary>
        public string BaseAuditID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
    }
}
