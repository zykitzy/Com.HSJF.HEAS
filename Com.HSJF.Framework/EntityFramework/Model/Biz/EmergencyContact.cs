using Com.HSJF.Infrastructure.DoMain;

namespace Com.HSJF.Framework.EntityFramework.Model.Biz
{
    public class EmergencyContact : EntityModel
    {
        public string ID { get; set; }
        public string ContactType { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string PersonID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        public virtual RelationPerson RelationPerson { get; set; }

    }
}
