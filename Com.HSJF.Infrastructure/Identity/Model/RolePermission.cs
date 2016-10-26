using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Identity.Model
{
    public class RolePermission
    {
        public string ID { get; set; }
        public string RoleID { get; set; }
        public string PermissionID { get; set; }

        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
