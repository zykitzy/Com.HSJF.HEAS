using Com.HSJF.Infrastructure.Identity.Model;
using Com.HSJF.Infrastructure.Identity.Store;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Identity.Manager
{
    public class RoleManager
    {
        RoleStore RoleStore;
        public RoleManager()
        {
            RoleStore = new RoleStore();
        }

        public Task<IdentityResult> Create(Role role)
        {
            var result = RoleStore.Create(role);
            return result;
        }

        public Task<IdentityResult> Update(Role role)
        {
            var result = RoleStore.Update(role);
            return result;
        }

        public Task<IdentityResult> Delete(Role role)
        {
            var result = RoleStore.Delete(role);
            return result;
        }

        public Task<IEnumerable<Role>> GetAll()
        {
            var result = RoleStore.GetAll();
            return result;
        }

        public Task<Role> GetRole(string id)
        {
            var result = RoleStore.GetRole(id);
            return result;
        }

        public Task FlushPermission(string roleid, params string[] permission)
        {
            return RoleStore.FlushPermission(roleid, permission);
        }

        public IEnumerable<Permission> GetPermissionByRole(string roleid)
        {
            return RoleStore.GetPermissionByRole(roleid);
        }

        public IEnumerable<Role> GetRoleByUser(string userid)
        {
            return RoleStore.GetRoleByUser(userid);
        }

    }
}
