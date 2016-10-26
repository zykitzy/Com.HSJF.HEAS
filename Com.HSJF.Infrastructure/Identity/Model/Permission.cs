using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.HSJF.Infrastructure.Identity.Model
{
    public class Permission : Microsoft.AspNet.Identity.IRole
    {
        public Permission() : base()
        {

        }

        public string Id { get; set; }
        public string Name { get; set; }
        public virtual string Description { get; set; }
        public Nullable<bool> IsBasic { get; set; }
        public int State { get; set; }

        public virtual ICollection<RolePermission> RolePermission { get; set; }
    }
}
