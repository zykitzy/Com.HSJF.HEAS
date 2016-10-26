using Com.HSJF.Infrastructure.DoMain;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit
{
    public class Guarantor : EntityModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string RelationType { get; set; }
        public string GuarantType { get; set; }
        public string IdentityType { get; set;}
        public string IdentityNumber { get; set; }
        public string Address { get; set; }
        public string MarriedInfo { get; set; }
        public string BaseAuditID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        public virtual BaseAudit BaseAudit { get; set; }

    }
}
