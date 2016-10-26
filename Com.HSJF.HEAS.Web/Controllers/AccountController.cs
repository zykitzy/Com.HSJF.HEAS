using Com.HSJF.Framework.DAL;
using Com.HSJF.Framework.DAL.SystemSetting;
using Com.HSJF.HEAS.Web.Domain;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Account;
using Com.HSJF.HEAS.Web.Models.Menu;
using Com.HSJF.Infrastructure.Crypto;
using Com.HSJF.Infrastructure.Identity.Manager;
using Com.HSJF.Infrastructure.Identity.Store;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Com.HSJF.HEAS.Web.Controllers
{
    [Authorize(Roles = "admin,IT")]
    public class AccountController : BaseController
    {
        //  Com.HSJF.Infrastructure.Identity.Model.User _currentUser;

        #region 用户基础操作

        public async Task<ActionResult> Index()
        {
            UserDAL ud = new UserDAL();
            var list = await ud.GetUsers();
            var Vlist = list.Select(t => new UserViewModel().CastView(t));
            return View(Vlist);
        }

        public async Task<ActionResult> EditUser(string id)
        {
            UserDAL ud = new UserDAL();
            var db = await ud.FindById(id);
            UserViewModel model = new UserViewModel();
            model = model.CastView(db);
            return View(model);
        }

        [HttpPost]
        public ActionResult EditUser(UserViewModel model)
        {
            UserDAL ud = new UserDAL();
            var user = model.CastDB(model);
            ud.Update(user);

            return Content("Success");
        }

        [HttpGet]
        public async Task<ActionResult> ResetPassword(string id)
        {
            var pass = WebConfigurationManager.AppSettings["ResetPassword"] ?? "a123456";
            UserDAL ud = new UserDAL();
            var result = await ud.ResetPassword(id, pass);
            if (result.Succeeded)
            {
                return Content("Success");
            }
            return Content("Failed");
        }

        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            if (WebConfigurationManager.AppSettings["LoginMode"] == "SSL")
            {
                byte[] _Key = Encoding.UTF8.GetBytes(WebConfigurationManager.AppSettings["Cryptokey"] ?? "HSJF!@#$12345678");
                byte[] _IV = Encoding.UTF8.GetBytes(WebConfigurationManager.AppSettings["CryptoIV"] ?? "HSJF^%$#12345678");
                var sysname = WebConfigurationManager.AppSettings["SystemName"];
                var username = CurrentUser.UserName;
                SymmCrypto symm = new SymmCrypto(_Key, _IV);
                var safeuser = symm.EncryptFromString(username, Encoding.UTF8);
                var user = Convert.ToBase64String(safeuser);
                Response.Redirect(WebConfigurationManager.AppSettings["LoginUrl"] + "Account/ModifyPassword?username=" + System.Web.HttpUtility.UrlEncode(user) + "&systemName=" + WebConfigurationManager.AppSettings["SystemName"]);
                return null;
            }
            else
            {
                var model = new ChangePasswordViewModel();
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            UserDAL ud = new UserDAL();
            var result = await ud.ChangePassword(CurrentUser.Id, model.oldPass, model.Password);
            if (result.Succeeded)
            {
                return base.GetBaseResponse<object>(true);
            }
            else
            {
                BaseResponse<string> br = new BaseResponse<string>();
                br.Status = "Failed";
                ErrorMessage[] em = new ErrorMessage[1];
                ErrorMessage ee = new ErrorMessage();
                ee.Key = "";
                ee.Message = "更新失败!请检查.";
                em[0] = ee;
                br.Message = em;
                return Json(br, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion 用户基础操作

        #region 登陆

        [AllowAnonymous]
        public async Task<ActionResult> Login()
        {
            //统一登陆
            if (WebConfigurationManager.AppSettings["LoginMode"] == "SSL")
            {
                var returnurl = Server.UrlEncode(HttpContext.Request.Url.AbsoluteUri);
                if (!Request.Url.AbsoluteUri.ToLower().Contains(WebConfigurationManager.AppSettings["LoginKey"].ToLower()))
                {
                    Response.Redirect(WebConfigurationManager.AppSettings["LoginUrl"] + "?returnUrl=" + returnurl + "&systemName=" + WebConfigurationManager.AppSettings["SystemName"]);
                    return null;
                }
                byte[] _Key = Encoding.UTF8.GetBytes(WebConfigurationManager.AppSettings["Cryptokey"] ?? "HSJF!@#$12345678");
                byte[] _IV = Encoding.UTF8.GetBytes(WebConfigurationManager.AppSettings["CryptoIV"] ?? "HSJF^%$#12345678");
                var userinfo = Request.QueryString[WebConfigurationManager.AppSettings["LoginKey"]];
                byte[] outputb = Convert.FromBase64String(userinfo);
                SymmCrypto symm = new SymmCrypto(_Key, _IV);
                var userstr = symm.DecryptToString(outputb, Encoding.UTF8);
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                var luser = (LoginUser)jsonSerializer.Deserialize(userstr, typeof(LoginUser));
                Microsoft.Owin.IOwinContext OwinContext = HttpContext.GetOwinContext();

                //初始化用户管理相关
                UserStore userStore = new UserStore();
                UserDAL userdal = new UserDAL();
                UserManager UserManager = new UserManager(userStore);
                Com.HSJF.Infrastructure.Identity.Model.User user = new Com.HSJF.Infrastructure.Identity.Model.User { UserName = luser.LoginName };
                //byte[] _Key = Encoding.UTF8.GetBytes(WebConfigurationManager.AppSettings["Cryptokey"] ?? "HSJF!@#$12345678");
                //byte[] _IV = Encoding.UTF8.GetBytes(WebConfigurationManager.AppSettings["CryptoIV"] ?? "HSJF^%$#12345678");
                var newuser = UserManager.FindByName(luser.LoginName);
                user.Password = symm.DecryptToString(Convert.FromBase64String(newuser.Password));
                if (!userdal.FindUser(user.UserName, Convert.ToBase64String(symm.EncryptFromString(user.Password))))
                {
                    ModelState.AddModelError("", "用户名不存在或者已被禁用！");
                    return View();
                }
                Microsoft.AspNet.Identity.Owin.SignInStatus SignInStatus = await PrivateLogin(user.UserName, user.Password);
                System.Web.HttpContext.Current.Session["_currentUser"] = UserManager.FindByName(user.UserName);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// 用户登陆，需要改进
        /// </summary>
        /// <param name="usermodel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel usermodel)
        {
            if (!ModelState.IsValid)
            {
                return View(usermodel);
            }

            Microsoft.Owin.IOwinContext OwinContext = HttpContext.GetOwinContext();

            //初始化用户管理相关
            UserStore userStore = new UserStore();
            UserDAL userdal = new UserDAL();
            UserManager UserManager = new UserManager(userStore);

            //初始化权限管理相关
            PermissionStore ps = new PermissionStore();
            PermissionManager pm = new PermissionManager(ps);
            //登录
            SignInManager signInManager = new SignInManager(UserManager, OwinContext.Authentication);
            Microsoft.AspNet.Identity.Owin.SignInStatus SignInStatus;
            string pass = usermodel.Password;
            string username = usermodel.LoginName;
            var user = new Com.HSJF.Infrastructure.Identity.Model.User { UserName = username, Password = pass };

            byte[] _Key = Encoding.UTF8.GetBytes(WebConfigurationManager.AppSettings["Cryptokey"] ?? "HSJF!@#$12345678");
            byte[] _IV = Encoding.UTF8.GetBytes(WebConfigurationManager.AppSettings["CryptoIV"] ?? "HSJF^%$#12345678");
            SymmCrypto symm = new SymmCrypto(_Key, _IV);
            if (!userdal.FindUser(usermodel.LoginName, Convert.ToBase64String(symm.EncryptFromString(usermodel.Password))))
            {
                ModelState.AddModelError("", "用户名不存在或者已被禁用！");
                return View();
            }
            //域登陆
            if (WebConfigurationManager.AppSettings["LoginMode"] == "LDAP")
            {
                LdapAuthentication ldap = new LdapAuthentication();
                if (!ldap.IsAuthenticated(usermodel.LoginName, usermodel.Password))
                {
                    ModelState.AddModelError("", "用户名或者密码错误！");
                    return View();
                }
                var newuser = UserManager.FindByName(username);
                user.Password = symm.DecryptToString(Convert.FromBase64String(newuser.Password));
            }

            SignInStatus = await PrivateLogin(user.UserName, user.Password);

            switch (SignInStatus)
            {
                //成功
                case Microsoft.AspNet.Identity.Owin.SignInStatus.Success:
                    //此处表示已经在startup 中配置
                    //标示
                    //System.Security.Claims.ClaimsIdentity identity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                    //授权登陆
                    //AutherticationManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties { IsPersistent = true }, identity);

                    System.Web.HttpContext.Current.Session["_currentUser"] = signInManager.UserManager.FindByName(user.UserName);
                    return RedirectToAction("Index", "Home");

                //锁定
                case Microsoft.AspNet.Identity.Owin.SignInStatus.LockedOut:
                    Response.Write("LockedOut!");
                    break;
                //要求验证
                case Microsoft.AspNet.Identity.Owin.SignInStatus.RequiresVerification:
                    Response.Write("RequiresVerification!");
                    break;
                //登录失败
                case Microsoft.AspNet.Identity.Owin.SignInStatus.Failure:
                    ModelState.AddModelError("", @"用户名或者密码错误！");
                    return View();
            }
            return View();
        }

        private async Task<Microsoft.AspNet.Identity.Owin.SignInStatus> PrivateLogin(string loginname, string password)
        {
            Microsoft.Owin.IOwinContext OwinContext = HttpContext.GetOwinContext();

            //初始化用户管理相关
            UserStore userStore = new UserStore();
            UserDAL userdal = new UserDAL();
            UserManager UserManager = new UserManager(userStore);

            SignInManager signInManager = new SignInManager(UserManager, OwinContext.Authentication);
            Microsoft.AspNet.Identity.Owin.SignInStatus SignInStatus;
            SignInStatus = await signInManager.PasswordSignInAsync(loginname, password,
               //是否记住
               isPersistent: true,
               shouldLockout: false);
            return SignInStatus;
        }

        #endregion 登陆

        #region 注册

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            UserDAL userdal = new UserDAL();
            Infrastructure.Identity.Model.User user = new Infrastructure.Identity.Model.User();
            user.DisplayName = model.DisplayName;
            user.Password = model.Password;
            user.UserName = model.LoginName;
            IdentityResult result = await userdal.Create(user);
            if (result.Succeeded)
                return Content("Success");
            else
                return Content("Failed");
        }

        #endregion 注册

        #region 用户权限相关操作

        //private IEnumerable<UserMenuViewModel> GetUserMenu(string userid)
        //{
        //    MenuDAL dal = new MenuDAL();
        //    User2MenuDAL u2mdal = new User2MenuDAL();
        //    var u2mlist = u2mdal.GetAll();
        //    var model = dal.GetAll().Select(t => new UserMenuViewModel()
        //    {
        //        ID = t.ID,
        //        Description = t.Description,
        //        Name = t.Name,
        //        ParentID = t.ParentID,
        //        State = t.State,
        //        Url = t.Url,
        //        IsSelect = u2mlist.Any(o => o.MenuID == t.ID && o.UserID == userid)
        //    });

        //    return model;
        //}
        /// <summary>
        /// 根据角色ID加载系统管理角色菜单
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <returns></returns>
        private IEnumerable<UserMenuViewModel> GetUserMenu(string roleid)
        {
            MenuDAL dal = new MenuDAL();
            Menu2RoleDAL m2rdal = new Menu2RoleDAL();
            var m2rlist = m2rdal.GetAll();
            var model = dal.GetAll().Select(t => new UserMenuViewModel()
            {
                ID = t.ID,
                Description = t.Description,
                Name = t.Name,
                ParentID = t.ParentID,
                State = t.State,
                Url = t.Url,
                IsSelect = m2rlist.Any(o => o.MenuID == t.ID && o.RoleID == roleid)
            });

            return model;
        }

        public ActionResult GetMenuList(string userid)
        {
            var model = GetUserMenu(userid);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> AccountPermission(string userid, string menuid)
        {
            MenuDAL dal = new MenuDAL();
            var plist = dal.GetPermission(menuid);
            List<CheckBoxListModel> list = new List<CheckBoxListModel>();
            UserDAL ud = new UserDAL();

            foreach (var p in plist)
            {
                var isin = await ud.IsInRole(userid, p.Name);
                list.Add(new CheckBoxListModel(p.Id, p.Name + "(" + p.Description.Trim() + ")", isin));
            }

            return View(list);
        }

        public async Task<ActionResult> SaveUserRole(string userid, string rolelist)
        {
            UserDAL ud = new UserDAL();
            var rerlist = rolelist.TrimEnd(',').Split(',');
            var result = await ud.FlashUserRoles(userid, rerlist);
            if (result.Succeeded)
                return Content("Success");
            else
                return Content("Failed");
        }

        /// <summary>
        /// 设置为可查看菜单
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        //public ActionResult SetUserMenu(string userid, string menuid)
        //{
        //    UserDAL ud = new UserDAL();
        //    ud.SetMenuShip(userid, menuid);
        //    var model = GetUserMenu(userid);
        //    return View("GetMenuList", model);
        //}

        /// <summary>
        /// 取消可查看菜单
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        //public ActionResult RemoveUserMenu(string userid, string menuid)
        //{
        //    UserDAL ud = new UserDAL();
        //    ud.DeleteMenuShip(userid, menuid);
        //    var model = GetUserMenu(userid);
        //    return View("GetMenuList", model);
        //}


        /// <summary>
        /// 设置为可查看菜单
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public ActionResult SetUserMenu(string roleid, string menuid)
        {
            UserDAL ud = new UserDAL();
            ud.SetMenuShip(roleid, menuid);
            var model = GetUserMenu(roleid);
            return View("GetMenuList", model);
        }

        /// <summary>
        /// 取消可查看菜单
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public ActionResult RemoveUserMenu(string roleid, string menuid)
        {
            UserDAL ud = new UserDAL();
            ud.DeleteMenuShip(roleid, menuid);
            var model = GetUserMenu(roleid);
            return View("GetMenuList", model);
        }
        #endregion 用户权限相关操作
    }
}