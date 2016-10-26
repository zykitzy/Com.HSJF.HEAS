using Com.HSJF.Framework.EntityFramework.Model.Sales;
using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.EntityFramework.Model.SystemSetting
{
    public class DataPermission : EntityModel
    {
        public string ID { get; set; }
        public string RoleID { get; set; }
        public string DataPermissionID { get; set; }

    }
}
