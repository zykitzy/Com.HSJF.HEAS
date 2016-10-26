using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.HSJF.Infrastructure.Identity.Model;
using Com.HSJF.Infrastructure.Identity.Manager;

namespace Com.HSJF.Framework.DAL.SystemSetting
{

    public class RoleDAL
    {
        private RoleManager RoleManager;
        public RoleDAL()
        {
            RoleManager = new RoleManager();
        }

        public async Task<bool> Add(Role role)
        {
            var result = await RoleManager.Create(role);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Update(Role role)
        {
            var result = await RoleManager.Update(role);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Update(string roleid)
        {
            var result = await RoleManager.Update(new Role() { ID = roleid });
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(string roleid)
        {
            var result = await RoleManager.Delete(new Role() { ID = roleid });
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            var result = await RoleManager.GetAll();
            return result;
        }

        public async Task<Role> GetRole(string id)
        {
            var result = await RoleManager.GetRole(id);
            return result;
        }
        public IEnumerable<Permission> GetPermissionByRole(string roleid)
        {
            return RoleManager.GetPermissionByRole(roleid);
        }

        public async Task<bool> FlushPermission(string roleid, params string[] roles)
        {
            await RoleManager.FlushPermission(roleid, roles);
            return true;
        }

        public IEnumerable<Role> GetRoleByUser(string userid)
        {
            var result = RoleManager.GetRoleByUser(userid);
            return result;
        }
    }
}
