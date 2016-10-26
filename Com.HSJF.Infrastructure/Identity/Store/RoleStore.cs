using Com.HSJF.Infrastructure.Identity.Context;
using Com.HSJF.Infrastructure.Identity.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Identity.Store
{
    [DebuggerStepThrough]
    public class RoleStore
    {
        public IdentityContext Context;
        public RoleStore()
        {
            Context = new IdentityContext();
        }

        public Task<IdentityResult> Create(Role role)
        {
            return Task<IdentityResult>.Run(() =>
            {
                role.ID = Guid.NewGuid().ToString();
                Context.Set<Role>().Add(role);
                Context.SaveChanges();
                return IdentityResult.Success;
            });
        }
        public Task<IdentityResult> Update(Role role)
        {
            return Task<IdentityResult>.Run(() =>
            {
                var entry = Context.Entry<Role>(role);
                if (entry.State == EntityState.Detached)
                {
                    Context.Set<Role>().Attach(role);
                }
                if (entry.State == EntityState.Unchanged)
                {
                    Context.Entry(role).State = EntityState.Modified;
                }
                Context.SaveChanges();
                return IdentityResult.Success;
            });
        }

        public Task<IdentityResult> Delete(Role role)
        {
            return Task<IdentityResult>.Run(() =>
            {
                var dbSet = Context.Set<Role>();
                if (Context.Entry(role).State == EntityState.Detached)
                {
                    dbSet.Attach(role);
                }
                dbSet.Remove(role);
                var urlist = Context.Set<UserRole>().Where(t => t.RoleID == role.ID);
                Context.Set<UserRole>().RemoveRange(urlist);
                Context.SaveChanges();
                return IdentityResult.Success;
            });
        }

        public Task<IEnumerable<Role>> GetAll()
        {
            return Task<IEnumerable<Role>>.Run(() =>
            {
                return Context.Set<Role>().AsEnumerable();
            });
        }

        public Task<Role> GetRole(string id)
        {
            return Task<IEnumerable<Role>>.Run(() =>
            {
                return Context.Set<Role>().FirstOrDefault(t => t.ID == id);
            });
        }

        public IEnumerable<Permission> GetPermissionByRole(string roleid)
        {
            var list = from i in Context.Set<RolePermission>()
                       join s in Context.Set<Permission>()
                       on i.PermissionID equals s.Id
                       join j in Context.Set<Role>()
                       on i.RoleID equals j.ID
                       where j.ID == roleid
                       select s;
            return list;
        }

        public Task FlushPermission(string roleid, params string[] permission)
        {
            return Task.Run(() =>
            {
                var rplist = Context.Set<RolePermission>().Where(t => t.RoleID == roleid);
                Context.Set<RolePermission>().RemoveRange(rplist);
                foreach (var p in permission)
                {
                    Context.Set<RolePermission>().Add(new RolePermission()
                    {
                        ID = Guid.NewGuid().ToString(),
                        PermissionID = p,
                        RoleID = roleid
                    });
                }
                Context.SaveChanges();
            });

        }

        public IEnumerable<Role> GetRoleByUser(string userid)
        {
            var list = from i in Context.Set<Role>()
                       join s in Context.Set<UserRole>()
                       on i.ID equals s.RoleID
                       join j in Context.Set<User>()
                       on s.UserID equals j.Id
                       where j.Id == userid
                       select i;
            return list;
        }
    }
}
