using Com.HSJF.Infrastructure.Identity.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Com.HSJF.Infrastructure.Identity.Context;
using System.Configuration;
using System.Collections;
using System.Diagnostics;

namespace Com.HSJF.Infrastructure.Identity.Store
{
    [DebuggerStepThrough]
    public class UserStore : IUserStore<User, string>,
                             IUserPasswordStore<User, string>,
                             IUserClaimStore<User, string>,
                             IUserLockoutStore<User, string>,
                             IUserEmailStore<User, string>,
                             IUserPhoneNumberStore<User, string>,
                             IUserTwoFactorStore<User, string>,
                             IUserRoleStore<User, string>
    {
        public IList<System.Security.Claims.Claim> Claims = null;
        public User UserIdentity = null;
        public IdentityContext Context;


        public UserStore()
        {
            //声明
            Claims = new List<System.Security.Claims.Claim>();
            Context = new IdentityContext();
        }

        #region 用户相关操作
        public Task AddClaimAsync(User user, Claim claim)
        {
            return Task.Run(() => { Claims.Add(claim); });
        }

        /// <summary>
        /// 新增一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task CreateAsync(User user)
        {
            return Task.Run(() =>
            {
                user.Id = Guid.NewGuid().ToString();
                Context.User.Add(user);
                int i = Context.SaveChanges();
                return i;
            });
        }

        /// <summary>
        /// 删除一个用户，将用户状态设为0 （1：启用，0；不启用）
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task DeleteAsync(User user)
        {
            return Task.Run(() =>
            {
                User tempuser;
                if (UserIdentity != null)
                {
                    tempuser = Context.User.FirstOrDefault(o => o.Id == UserIdentity.Id);
                }
                else
                {
                    tempuser = Context.User.FirstOrDefault(o => o.Id == user.Id);
                }
                tempuser.UserState = 0;
                Context.SaveChanges();
            });
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task UpdateAsync(User user)
        {
            return Task.Run(() =>
            {
                var tempuser = Context.User.FirstOrDefault(o => o.Id == user.Id);
                tempuser.DisplayName = user.DisplayName;
                tempuser.UserName = user.UserName;
                var i = Context.SaveChanges();
                return i;
            });
        }

        /// <summary>
        /// 根据ID获取
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<User> FindByIdAsync(string userId)
        {

            return Task.Run(() =>
            {
                if (UserIdentity != null)
                {
                    return UserIdentity;
                }
                var tempuser = Context.User.FirstOrDefault(o => o.Id == userId && o.UserState == 1);
                return tempuser;
            });
        }

        public Task<User> FindByNameAsync(string UserName)
        {
            return Task.Run(() =>
            {
                if (UserIdentity != null)
                {
                    return UserIdentity;
                }
                var tempuser = Context.User.FirstOrDefault(o => o.UserName == UserName && o.UserState == 1);
                return tempuser;
            });
        }

        #endregion

        #region 用户角色
        public Task AddToRoleAsync(User user, string roleName)
        {
            return Task.Run(() =>
            {
                var role = Context.Set<Role>().FirstOrDefault(o => o.Name == roleName);

                if (role == null)
                {
                    throw new Exception("不存在该角色!");
                }

                AddChildRole(user, role);
                Context.SaveChanges();
            });
        }

        /// <summary>
        /// 递归增加权限
        /// </summary>
        /// <param name="user"></param>
        /// <param name="parentrole"></param>
        /// <returns></returns>
        private Task AddChildRole(User user, Role parentrole)
        {
            return Task.Run(() =>
            {
                UserRole ur = new UserRole()
                {
                    ID = Guid.NewGuid().ToString(),
                    RoleID = parentrole.ID,
                    UserID = user.Id
                };
                Context.Set<UserRole>().Add(ur);
            });
        }
        /// <summary>
        /// 批量添加权限
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public Task<IdentityResult> FlashRolesAsync(string userid, params string[] roleid)
        {
            return Task<IdentityResult>.Run(() =>
            {
                if (RemoveAllRole(userid).Succeeded)
                {
                    foreach (var role in roleid)
                    {
                        var ur = new UserRole()
                        {
                            ID = Guid.NewGuid().ToString(),
                            RoleID = role,
                            UserID = userid
                        };
                        Context.Set<UserRole>().Add(ur);
                    }
                    Context.SaveChanges();
                }
                return IdentityResult.Success;
            });

        }

        //私有方法，先删除所有的权限
        private IdentityResult RemoveAllRole(string userid)
        {
            var oldroles = Context.Set<UserRole>().Where(t => t.UserID == userid);
            Context.Set<UserRole>().RemoveRange(oldroles);
            return IdentityResult.Success;

        }


        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            return Task.Run(() =>
            {
                var role = Context.Set<Role>().FirstOrDefault(o => o.Name == roleName);
                if (role == null)
                {
                    throw new Exception("不存在该角色!");
                }
                var entity = Context.Set<UserRole>().FirstOrDefault(t => t.RoleID == role.ID && t.UserID == user.Id);
                if (entity != null)
                {
                    Context.Set<UserRole>().Remove(entity);
                }

                return IdentityResult.Success;
            });
        }


        public Task<IList<string>> GetRolesAsync(User user)
        {
            return Task.Run(() =>
            {
                RoleStore rs = new RoleStore();
                var urlist = Context.Set<UserRole>().Where(o => o.UserID == user.Id);
                IList<string> rolelist = new List<string>();
                if (urlist.Any())
                {
                    foreach (var r in urlist)
                    {
                        var plist = rs.GetPermissionByRole(r.RoleID);
                        plist.Select(t => t.Name).ForEach(o => rolelist.Add(o));
                    }
                }
                return rolelist;

            });
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            return Task<bool>.Run(() =>
            {
                RoleStore rs = new RoleStore();
                var urlist = Context.Set<UserRole>().Where(o => o.UserID == user.Id);
                if (urlist.Any())
                {
                    foreach (var r in urlist)
                    {
                        var plist = rs.GetPermissionByRole(r.RoleID);
                        if (plist.Any(t => t.Name == roleName))
                        {
                            return true;
                        }
                    }
                }
                return false;

            });
        }

        #endregion

        #region 前4个接口

        public Task<int> GetAccessFailedCountAsync(User user)
        {

            return Task<int>.Run(() => { return 1; });
        }

        public Task<IList<Claim>> GetClaimsAsync(User user)
        {
            return Task.Run<IList<System.Security.Claims.Claim>>(() =>
            {
                return Claims;
            });
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task<bool>.Run(() => { return false; });
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return Task<DateTimeOffset>.Run(() => { return DateTimeOffset.Now; });
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return Task<string>.Run(() =>
            {
                return user.Password;
            });
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult<bool>(string.IsNullOrEmpty(user.Password));
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            return Task<int>.Run(() => { return 1; });
        }

        public Task RemoveClaimAsync(User user, Claim claim)
        {
            return Task.Run(() => { Claims.Remove(claim); });
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            return Task<int>.Run(() => { return 1; });
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            return Task.Run(() => { return true; });
        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            return Task<DateTimeOffset>.Run(() => { return DateTimeOffset.Now; });
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            return Task.Run(() => { user.Password = passwordHash; });
        }

        #endregion

        #region 邮件相关 - 未实现
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(User user, string email)
        {
            return Task.Run(() => { });
        }

        public Task<string> GetEmailAsync(User user)
        {
            return Task<string>.Run(() => { return string.Empty; });
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            return Task<bool>.Run(() => { return true; });
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            return Task.Run(() => { });
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return Task<User>.Run(() => { return new User(); });
        }
        #endregion

        #region 电话相关 - 未实现
        public Task SetPhoneNumberAsync(User user, string phoneNumber)
        {
            return Task.Run(() => { });
        }

        public Task<string> GetPhoneNumberAsync(User user)
        {
            return Task<string>.Run(() => { return string.Empty; });
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user)
        {
            return Task<bool>.Run(() => { return true; });
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed)
        {
            return Task.Run(() => { });
        }

        #endregion

        #region 双重验证 - 未实现
        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            return Task.Run(() => { });
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task<bool>.Run(() => { return false; });
        }
        #endregion
    }
}
