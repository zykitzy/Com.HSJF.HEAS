using Com.HSJF.Infrastructure.Identity.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.DAL.SystemSetting
{
    public class PermissionDAL
    {
        Infrastructure.Identity.Context.IdentityContext context;
        Infrastructure.Identity.Store.PermissionStore PermissionStore;
        Infrastructure.Identity.Manager.PermissionManager PermissionManager;

        public PermissionDAL() : this(new Infrastructure.Identity.Context.IdentityContext())
        { }

        public PermissionDAL(Infrastructure.Identity.Context.IdentityContext conn)
        {
            context = conn;
            PermissionStore = new Infrastructure.Identity.Store.PermissionStore();
            PermissionManager = new Infrastructure.Identity.Manager.PermissionManager(PermissionStore);
        }

        public Task<IdentityResult> CreateAsync(Permission model)
        {
            return Task<IdentityResult>.Run(() =>
            {
                return PermissionManager.CreateAsync(model);
            });
        }

        public Task<IdentityResult> DeleteAsync(Permission model)
        {
            return Task<IdentityResult>.Run(() =>
            {
               return PermissionManager.DeleteAsync(model);
            });
        }

        public Task<IdentityResult> UpdateAsync(Permission model)
        {
            return Task<IdentityResult>.Run(() =>
            {
                return PermissionManager.DeleteAsync(model);
            });
        }

        public IEnumerable<Permission> GetAll()
        {
            return PermissionManager.GetAll();
        }
    }
}
