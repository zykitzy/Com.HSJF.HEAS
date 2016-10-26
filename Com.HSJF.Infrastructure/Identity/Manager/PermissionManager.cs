using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Com.HSJF.Infrastructure.Identity.Model;
using Com.HSJF.Infrastructure.Identity.Store;

namespace Com.HSJF.Infrastructure.Identity.Manager
{
    public class PermissionManager: Microsoft.AspNet.Identity.RoleManager<Permission,string>
    {
        private PermissionStore Store;
        public PermissionManager(PermissionStore PermissionStore) : base(PermissionStore)
        {
            Store = PermissionStore;
        }

        public IEnumerable<Permission> GetAll()
        {
            return Store.GetAll();
        }
    }
}
