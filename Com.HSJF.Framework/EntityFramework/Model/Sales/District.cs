using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.Sales
{
    public class District : EntityModel
    {
        public string ID { get; set; }
         //地区名称 
        public string Name { get; set; }
        //简号，用来表示地区，比如 上海 SH,北京 BJ
        public string ShortNumber { get; set; }

        public string Description { get; set; }

        public virtual ICollection<SalesGroup> SalesGroup { get; set; }

    }
}
