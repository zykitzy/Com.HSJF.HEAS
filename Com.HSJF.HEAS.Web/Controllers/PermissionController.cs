using Com.HSJF.Framework.DAL.SystemSetting;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Permission;
using Com.HSJF.Infrastructure.Identity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.HSJF.HEAS.Web.Controllers
{
    [Authorize(Roles = "ITSupport")]
    public class PermissionController : BaseController
    {
        // GET: Permission

        public ActionResult Index(string id)
        {
            RoleDAL rd = new RoleDAL();
            PermissionDAL pd = new PermissionDAL();
            var plist = pd.GetAll().OrderBy(t => t.State);
            var mplist = rd.GetPermissionByRole(id);
            List<CheckBoxListModel> list = new List<CheckBoxListModel>();
            foreach (var p in plist)
            {
                list.Add(new CheckBoxListModel(p.Id, p.Name + "(" + p.Description.Trim() + ")", mplist.Any(t => t.Id == p.Id)));
            }

            return View(list);
        }

        [Authorize(Roles = "ITSupport")]
        //内部使用方法，上线后应用权限限制
        public ActionResult AddPermission()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddPermission(PermissionViewModel model)
        {
            PermissionDAL pd = new PermissionDAL();
            Permission per = new Permission();
            per.Name = model.PermissionName;
            per.Description = model.Description;
            per.State = 1;
            var result = await pd.CreateAsync(per);
            if (result.Succeeded)
                return Content("Success");
            else
                return Content("Failed");
        }

        [HttpPost]
        public async Task<ActionResult> EditPermission(string id, string perid)
        {
            RoleDAL rd = new RoleDAL();
            var result = await rd.FlushPermission(id, perid.Trim(',').Split(','));
            if (result)
            {
                return ToJsonResult("Success");
            }
            return ToJsonResult("Failed");
        }
    }
}