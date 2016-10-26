using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Com.HSJF.Infrastructure.Identity.Model;
using Com.HSJF.Infrastructure.Identity.Context;

namespace Com.HSJF.Infrastructure.Identity.Store
{
    public class PermissionStore : IRoleStore<Permission, string>
    {
        private IdentityContext Context;
        public PermissionStore()
        {
            Context = new IdentityContext();
        }

        public Task CreateAsync(Permission role)
        {
            return Task.Run(() =>
            {
                var per =  FindByNameAsync(role.Name);
                if (per.Result != null)
                {
                    throw new Exception("权限已经存在");
                }
                role.Id = Guid.NewGuid().ToString();
                role.IsBasic = false;
                Context.Set<Permission>().Add(role);
                Context.SaveChanges();
            });
        }

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task DeleteAsync(Permission role)
        {
            return Task.Run(() =>
            {
                var per = Context.Set<Permission>().FirstOrDefault(o => o.Id == role.Id && (!role.IsBasic??false));
                Context.Set<Permission>().Remove(per);
                Context.SaveChanges();
            });
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<Permission> FindByIdAsync(string roleId)
        {
            return Task<Permission>.Run(() =>
            {
                var per = Context.Set<Permission>().FirstOrDefault(o => o.Id == roleId);

                return per;
            });
        }

        public Task<Permission> FindByNameAsync(string roleName)
        {
            return Task<Permission>.Run(() =>
            {
                var per = Context.Set<Permission>().FirstOrDefault(o => o.Name == roleName);

                return per;
            });
        }

        /// <summary>
        /// 目前只能修改PermissionName,Description，其他属性不能修改
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task UpdateAsync(Permission role)
        {
            return Task.Run(() =>
            {
                var per = Context.Set<Permission>().FirstOrDefault(o => o.Id == role.Id);
                per.Description = role.Description;
                per.Name = role.Name;
                Context.SaveChanges();
            });
        }

        public IEnumerable<Permission> GetAll()
        {
            return Context.Set<Permission>();
        }
    }
}
