using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.SystemSetting
{
    public class Menu2Permission : EntityModel
    {
        public string ID { get; set; }
        public string MenuID { get; set; }
        public string PermissionID { get; set; }
    }
}
