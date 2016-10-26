using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.Infrastructure.File;
using Com.HSJF.Infrastructure.Identity.Manager;
using Com.HSJF.Infrastructure.Identity.Store;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Com.HSJF.HEAS.Web.Controllers
{
    public class BaseController : Controller
    {
        public static Infrastructure.LogExtend.LogManagerExtend logger = new Infrastructure.LogExtend.LogManagerExtend();

        public Infrastructure.Identity.Model.User CurrentUser
        {
            get
            {
                var user = (Infrastructure.Identity.Model.User)System.Web.HttpContext.Current.Session["_currentUser"];
                if (user == null)
                {
                    UserStore ps = new UserStore();
                    UserManager pm = new UserManager(ps);
                    user = pm.FindByName(User.Identity.Name);
                    System.Web.HttpContext.Current.Session["_currentUser"] = user;
                }
                return user;
            }
            set
            {
                System.Web.HttpContext.Current.Session["_currentUser"] = value;
            }
        }

        public JsonResult ToJsonResult(string brp)
        {
            JsonResult j = new JsonResult()
            {
                Data = brp
            };
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return j;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;
            logger.WriteException("Application Error", ex);
            if (ex.GetBaseException() != null)
            {
                logger.WriteException("Application Error", ex.GetBaseException());
            }
            ViewBag.CurrentUser = CurrentUser;
            //  base.OnException(filterContext);
            RedirectToAction("Failed", "Home");
            return;
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentUser == null)
            {
                string controller = filterContext.RouteData.Values["controller"] as string;
                string action = filterContext.RouteData.Values["action"] as string;

                if (string.Compare(controller, "account", StringComparison.OrdinalIgnoreCase) == 0 &&
                    string.Compare(action, "login", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    base.OnAuthorization(filterContext);
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Account/Login");
                }
            }
            else
            {
                ViewBag.CurrentUser = CurrentUser;
                base.OnAuthorization(filterContext);
            }
        }

        public static Dictionary<string, string> GetFiles(string fileids, bool flag = false)
        {
            FileUpload filedal = new FileUpload();
            var filelist = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(fileids))
            {
                return null;
            }
            var files = fileids.Trim(',').Split(',');

            foreach (var f in files)
            {
                filelist.Add(f, filedal.GetFileName(f, flag));
            }
            return filelist;
        }

        public JsonResult GetBaseResponse<T>(bool issuccess)
        {
            BaseResponse<T> br = new BaseResponse<T>();
            br.Status = issuccess ? "Success" : "Failed";
            return Json(br, JsonRequestBehavior.AllowGet);
        }
    }
}