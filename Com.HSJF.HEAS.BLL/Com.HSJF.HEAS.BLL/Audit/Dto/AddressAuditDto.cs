
namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    public class AddressAuditDto
    {
        public string ID { get; set; }

        /// <summary>
        /// 地址类型
        /// </summary>
        public string AddressType { get; set; }

        /// <summary>
        /// 地址类型text
        /// </summary>
        public string AddressTypeText { get; set; }

        /// <summary>
        /// 地址信息
        /// </summary>
        public string AddressInfo { get; set; }

        /// <summary>
        /// 联系人ID
        /// </summary> 
        public string PersonID { get; set; }

        /// <summary>
        /// 是否为默认地址
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }
    }
}
