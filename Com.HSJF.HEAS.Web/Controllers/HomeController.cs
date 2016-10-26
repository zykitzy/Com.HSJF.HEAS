using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Com.HSJF.HEAS.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            //BaseCaseDAL bc = new BaseCaseDAL();
            //var list = bc.GetAllAuthorize(new string[]{"-TestD", "-TestS" });
            //var t = list.ToList();

            return View();
        }

        public ActionResult TestF(Dictionary<string, string> list)
        {
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase
            file, string linkid, string filestate = "0")
        {
            Infrastructure.File.FileUpload up = new Infrastructure.File.FileUpload();
            if (string.IsNullOrEmpty(linkid))
            {
                linkid = Guid.Empty.ToString();
            }
            var state = Convert.ToInt32(filestate);
            var str = up.SaveToDB(file, Guid.Parse(linkid), state);

            return Json(str, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SignIn(string uname, string password)
        {
            return View();
        }


        public ActionResult SignOut()
        {
            Microsoft.Owin.IOwinContext OwinContext = HttpContext.GetOwinContext();
            OwinContext.Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
            if (WebConfigurationManager.AppSettings["LoginMode"] == "SSL")
            {
                var returnurl = Server.UrlEncode("http://" + HttpContext.Request.Url.Authority);
                Response.Redirect(WebConfigurationManager.AppSettings["LoginUrl"] + "Account/LoginOut?returnUrl=" + returnurl + "&systemName=" + WebConfigurationManager.AppSettings["SystemName"]);
                return null;
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }


        #region 测试数据
        //[AllowAnonymous]
        //public async Task<ActionResult> AddUser()
        //{
        //    Microsoft.Owin.IOwinContext OwinContext = HttpContext.GetOwinContext();
        //    UserStore userStore = new UserStore();

        //    //UserManager
        //    //UserManager
        //    UserManager UserManager = new UserManager(userStore);

        //    string pass = "Password@1";
        //    string displayname = "张翼";
        //    string username = "zhangyi";
        //    Com.HSJF.Infrastructure.Identity.Model.User user
        //                            = new Com.HSJF.Infrastructure.Identity.Model.User { UserName = username, Password = pass, DisplayName = displayname };

        //    //设置添加用户时的验证规则
        //    UserManager.UserValidator = new Microsoft.AspNet.Identity.UserValidator<User>(UserManager)
        //    {
        //        //是否只允许用户名称是字幕加数字
        //        AllowOnlyAlphanumericUserNames = false,
        //        //是否只允许Email 是空的
        //        RequireUniqueEmail = false
        //    };

        //    Microsoft.AspNet.Identity.IdentityResult result = await UserManager.CreateAsync(user, user.Password);

        //    if (result.Succeeded)
        //    {
        //        var t = await UserManager.AddToRoleAsync(user.Id, "admin");
        //        return Content("OK");
        //    }
        //    else
        //        return Content(result.Errors.FirstOrDefault());
        //}

        //public async Task<ActionResult> MapUserRole(string role)
        //{
        //    string username = "zhangyi";
        //    Microsoft.Owin.IOwinContext OwinContext = HttpContext.GetOwinContext();
        //    UserStore userStore = new UserStore();

        //    //UserManager
        //    UserManager UserManager = new UserManager(userStore);

        //    var user = await UserManager.FindByNameAsync(username);
        //    var result = await UserManager.AddToRoleAsync(user.Id, "test");

        //    if (result.Succeeded)
        //    {
        //        return Content("OK");
        //    }
        //    else
        //        return Content(result.Errors.FirstOrDefault());
        //}


        //public async Task<ActionResult> AddRole()
        //{
        //    Microsoft.Owin.IOwinContext OwinContext = HttpContext.GetOwinContext();
        //    PermissionStore ps = new PermissionStore();
        //    PermissionManager pm = new PermissionManager(ps);
        //    Permission pe = new Permission();
        //    pe.Id = Guid.NewGuid().ToString();
        //    pe.Name = "admin";
        //    pe.Description = "this. is  a admin";
        //    var result = await pm.CreateAsync(pe);
        //    if (result.Succeeded)
        //        return Content("OK");
        //    else
        //        return Content(result.Errors.FirstOrDefault());
        //}
        #endregion



        public ActionResult Success()
        {
            ViewBag.CurrentUser = CurrentUser;
            return View();
        }

        public ActionResult Failed()
        {
            ViewBag.CurrentUser = CurrentUser;
            return View();
        }

        public ActionResult Error()
        {
            ViewBag.CurrentUser = CurrentUser;
            return View();
        }

    }
}
