using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;

namespace Com.HSJF.Framework.EntityFramework.Model.Sales
{
    public partial class SalesMan : EntityModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string SalesID { get; set; }
        public string Post { get; set; }
        public int? State { get; set; }
        public string GroupID { get; set; }
        public virtual SalesGroup SalesGroup { get; set; }
    }
}
