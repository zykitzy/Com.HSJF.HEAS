using Com.HSJF.Infrastructure.DoMain;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit
{
    public class IntroducerAudit : EntityModel
    {
        public IntroducerAudit()
        { }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Contract { get; set; }
        public decimal? RebateAmmount { get; set; }
        public decimal? RebateRate { get; set; }
        public string Account { get; set; }
        public string AccountBank { get; set; }
        public string AuditID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        public virtual BaseAudit BaseAudit { get; set; }
    }
}
