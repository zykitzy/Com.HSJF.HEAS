using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.Others
{
    public partial class Dictionary : EntityModel
    {

        public string ID { get; set; }
        public string Path { get; set; }
        public string Key { get; set; }
        public string ParentKey { get; set; }
        public string Text { get; set; }
        public bool State { get; set; }
        public Nullable<int> Desc { get; set; }

    }
}
