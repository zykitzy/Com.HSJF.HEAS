using Com.HSJF.Infrastructure.DoMain;

namespace Com.HSJF.Framework.EntityFramework.Model.Biz
{
    public class Introducer : EntityModel
    {
        public Introducer()
        { }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Contract { get; set; }
        public decimal? RebateAmmount { get; set; }
        public decimal? RebateRate { get; set; }
        public string Account { get; set; }
        public string AccountBank { get; set; }
        public string CaseID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        public virtual BaseCase BaseCase { get; set; }
    }
}
