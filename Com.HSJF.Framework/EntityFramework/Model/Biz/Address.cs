using Com.HSJF.Infrastructure.DoMain;

namespace Com.HSJF.Framework.EntityFramework.Model.Biz
{
    public class Address : EntityModel
    {
        public string ID { get; set; }
        public string AddressType { get; set; }
        public string AddressInfo { get; set; }
        public bool IsDefault { get; set; }
        public string PersonID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        public virtual RelationPerson RelationPerson { get; set; }
    }
}
