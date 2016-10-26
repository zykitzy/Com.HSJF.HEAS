
namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    public class ContactAuditDto
    {
        public string ID { get; set; }

        public string ContactType { get; set; }

        public string ContactTypeText { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string ContactNumber { get; set; }

        /// <summary>
        /// 是否默认联系人
        /// </summary>
        public bool IsDefault { get; set; }
        
        /// <summary>
        /// 联系人ID
        /// </summary>
        public string PersonID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }
    }
}
