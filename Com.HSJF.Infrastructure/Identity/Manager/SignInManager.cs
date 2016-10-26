using Com.HSJF.Infrastructure.Identity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.Identity.Manager
{
    public class SignInManager : Microsoft.AspNet.Identity.Owin.SignInManager<User, string>
    {
        public SignInManager(UserManager usermanager, Microsoft.Owin.Security.IAuthenticationManager aumanager)
            : base(usermanager, aumanager)
        {

        }

        /// <summary>
        /// 根据用户名密码，验证用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="isPersistent"></param>
        /// <param name="shouldLockout"></param>
        /// <returns></returns>
        public override System.Threading.Tasks.Task<Microsoft.AspNet.Identity.Owin.SignInStatus> PasswordSignInAsync(string userName,
                                                                                                                     string password,
                                                                                                                     bool isPersistent,
                                                                                                                     bool shouldLockout)
        {
            return base.PasswordSignInAsync(userName,
                                            password,
                                            isPersistent,
                                            shouldLockout);
        }
    }
    
}
