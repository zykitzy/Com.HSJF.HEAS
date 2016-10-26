
namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    /// <summary>
    /// 紧急联系人
    /// </summary>
    public class EmergencyContactAuditDto
    {

        public string ID { get; set; }

        public string ContactType { get; set; }

        public string ContactTypeText { get; set; }

        public string Name { get; set; }

        public string ContactNumber { get; set; }

        /// <summary>
        /// 关系人ID
        /// </summary>
        public string PersonID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }
    }
}
