using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.Others
{
    public class RelationState : EntityModel
    {
        public Guid ID { get; set; }
        public string CaseNum { get; set; }
        public string SalesID { get; set; }
        public int RelationType { get; set; }
        public string RelationNumber { get; set; }
        public int IsLock { get; set; }
        public int IsBinding { get; set; }
        public DateTime CreateTime { get; set; }
        public string Desc { get; set; }
        public int State { get; set; }

    }
}
