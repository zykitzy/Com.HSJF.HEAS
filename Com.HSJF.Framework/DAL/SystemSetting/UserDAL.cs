using Com.HSJF.Framework.DAL.SystemSetting;
using Com.HSJF.Framework.EntityFramework.Model.SystemSetting;
using Com.HSJF.Infrastructure.Identity.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.DAL
{
    public class UserDAL
    {
        private Com.HSJF.Infrastructure.Identity.Manager.UserManager manager;
        private Com.HSJF.Infrastructure.Identity.Store.UserStore store;
        private Infrastructure.Identity.Context.IdentityContext context;

        public UserDAL() : this(new Infrastructure.Identity.Context.IdentityContext())
        {
        }

        public UserDAL(Infrastructure.Identity.Context.IdentityContext conn)
        {
            store = new Infrastructure.Identity.Store.UserStore();
            manager = new Infrastructure.Identity.Manager.UserManager(store);
            context = conn;
        }

        #region 用户基础操作

        public Task<User> FindById(string id)
        {
            return Task.Run(() =>
            {
                return manager.FindById(id);
            });
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<IdentityResult> ResetPassword(string userid, string password)
        {
            return Task<IdentityResult>.Run(() =>
            {
                manager.RemovePassword(userid);
                return manager.AddPasswordAsync(userid, password);
            });
        }

        public Task<IdentityResult> ChangePassword(string userid, string oldpass, string password)
        {
            return Task<IdentityResult>.Run(() =>
            {
                return manager.ChangePasswordAsync(userid, oldpass, password);
            });
        }

        /// <summary>
        /// 新增一个用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IdentityResult> Create(User user)
        {
            //设置添加用户时的验证规则
            manager.UserValidator = new Microsoft.AspNet.Identity.UserValidator<User>(manager)
            {
                //是否只允许用户名称是字幕加数字
                AllowOnlyAlphanumericUserNames = false,
                //是否只允许Email 是空的
                RequireUniqueEmail = false
            };
            user.UserState = 1;

            return Task<IdentityResult>.Run(() =>
            {
                return manager.CreateAsync(user, user.Password);
            });
        }

        /// <summary>
        /// 删除一个用户，状态置为 0
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task DeleteUser(User user)
        {
            return Task.Run(() =>
            {
                manager.DeleteAsync(user);
            });
        }

        /// <summary>
        /// 更新一个用户，ID 和 password 不会更改，目前只能修改Display Name 和Password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task Update(User user)
        {
            return Task.Run(() =>
            {
                manager.UpdateAsync(user);
            });
        }

        /// <summary>
        /// 关联用户与权限（角色）
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public Task<IdentityResult> AddUserToRole(string userid, string rolename)
        {
            return Task<IdentityResult>.Run(() =>
            {
                return manager.AddToRoleAsync(userid, rolename);
            });
        }

        public Task<IdentityResult> FlashUserRoles(string userid, params string[] roleid)
        {
            return Task<IdentityResult>.Run(() =>
            {
                return manager.FlashRoles(userid, roleid);
            });
        }

        /// <summary>
        /// 用户是否在角色中
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public Task<bool> IsInRole(string userid, string rolename)
        {
            return Task<bool>.Run(() =>
            {
                return manager.IsInRoleAsync(userid, rolename);
            });
        }

        /// <summary>
        /// 获取所有状态为可用的用户 （Userstate：1）
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<User>> GetUsers()
        {
            return Task<IEnumerable<User>>.Run(() =>
            {
                return context.Set<User>().Where(o => o.UserState == 1).AsEnumerable();
            });
        }

        /// <summary>
        /// 根据用户用获取用户姓名
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GetDisplayName(string username)
        {
            if (string.IsNullOrEmpty(username)) return "";
            return context.Set<User>().Where(s => s.UserName == username).FirstOrDefault().DisplayName;
        }

        /// <summary>
        /// 根据用户名密码获取
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool FindUser(string LoginName, string Password)
        {
            var user = context.User.Where(o => o.UserName == LoginName && o.Password == Password && o.UserState == 1).ToList();
            if (user != null && user.Count > 0)
            {
                return true;
            }
            return false;
        }

        #endregion 用户基础操作

        #region 用户相关菜单操作

        public IEnumerable<Menu> GetMenuList(string userid)
        {
            MenuDAL md = new MenuDAL();
            User2MenuDAL u2md = new User2MenuDAL();
            var u2m = u2md.GetAll().Where(o => o.UserID == userid);

            var ml = from i in md.GetAll()
                     join j in u2m
                     on i.ID equals j.MenuID
                     select i;

            return ml.AsEnumerable();
        }

        /// <summary>
        ///  给用户增加一个菜单，同时增加相应的权限
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="menuid"></param>
        //public void SetMenuShip(string userid, string menuid)
        //{
        //    User2MenuDAL u2md = new User2MenuDAL();

        //    User2Menu model = new User2Menu();
        //    model.ID = Guid.NewGuid();
        //    model.MenuID = menuid;
        //    model.UserID = userid;
        //    model.MenuID = menuid;
        //    u2md.Add(model);

        //    u2md.AcceptAllChange();
        //}

        /// <summary>
        ///  删除用户的一个菜单，同时去除响应的权限
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="menuid"></param>
        //public void DeleteMenuShip(string userid, string menuid)
        //{
        //    User2MenuDAL u2md = new User2MenuDAL();
        //    var menu = u2md.GetAll().FirstOrDefault(o => o.MenuID == menuid && o.UserID == userid);
        //    if (menu != null)
        //    {
        //        u2md.Delete(menu);
        //    }

        //    u2md.AcceptAllChange();
        //}

        /// <summary>
        ///  给用户增加一个菜单，同时增加相应的权限
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="menuid"></param>
        public void SetMenuShip(string roleid, string menuid)
        {
            Menu2RoleDAL m2rdal = new Menu2RoleDAL();
            Menu2Role model = new Menu2Role();
            model.ID = Guid.NewGuid();
            model.MenuID = menuid;
            model.RoleID = roleid;
            m2rdal.Add(model);
            m2rdal.AcceptAllChange();
        }

        /// <summary>
        ///  删除角色的一个菜单，同时去除响应的权限
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="menuid"></param>
        public void DeleteMenuShip(string roleid, string menuid)
        {
            Menu2RoleDAL m2rdal = new Menu2RoleDAL();
            var menu = m2rdal.GetAll().FirstOrDefault(o => o.MenuID == menuid && o.RoleID == roleid);
            if (menu != null)
            {
                m2rdal.Delete(menu);
            }

            m2rdal.AcceptAllChange();
        }

        #endregion 用户相关菜单操作
    }
}