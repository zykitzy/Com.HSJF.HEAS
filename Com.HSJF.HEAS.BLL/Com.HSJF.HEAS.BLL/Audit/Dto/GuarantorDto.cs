
namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    public class GuarantorDto
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string ContactNumber { get; set; }

        public string RelationType { get; set; }

        public string RelationTypeText { get; set; }

        public string GuarantType { get; set; }

        public string GuarantTypeText { get; set; }

        public string IdentityType { get; set; }

        public string IdentityTypeText { get; set; }

        public string IdentityNumber { get; set; }

        public string Address { get; set; }

        public string MarriedInfo { get; set; }

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
