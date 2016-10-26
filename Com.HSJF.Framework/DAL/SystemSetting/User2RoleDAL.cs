using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.HSJF.Infrastructure.Identity.Model;
using Com.HSJF.Framework.EntityFramework.Context;
using Com.HSJF.Infrastructure.Identity.Context;

namespace Com.HSJF.Framework.DAL.SystemSetting
{
   public class User2RoleDAL
    {
        private Infrastructure.Identity.Context.IdentityContext context;

       public User2RoleDAL()
       {
            context = ContextFactory.EFContextFactory.GetCurrentDbContext<IdentityContext>();
        }
        /// <summary>
        /// 根据用户ID查询角色ID
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
       public IQueryable<UserRole> GetUser2RoleByUserId(string userid)
       {
          return context.Set<UserRole>().Where(s => s.UserID == userid);
       }
    }
}
