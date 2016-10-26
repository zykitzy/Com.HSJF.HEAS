using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Identity.Model
{
    public class Role
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
        public virtual ICollection<RolePermission> RolePermission { get; set; }
    }
}
