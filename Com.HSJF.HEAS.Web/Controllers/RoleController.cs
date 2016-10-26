using Com.HSJF.Framework.DAL.SystemSetting;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.HSJF.HEAS.Web.Controllers
{
    [Authorize(Roles = "IT,admin")]
    public class RoleController : BaseController
    {

        // GET: Role
        public async Task<ActionResult> Index()
        {
            RoleDAL dal = new RoleDAL();
            var rolelist = await dal.GetAll();
            var viewlist = rolelist.Select(t => new RoleViewModel().CastView(t));
            return View(viewlist);
        }

        public ActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddRole(RoleViewModel model)
        {
            RoleDAL dal = new RoleDAL();
            await dal.Add(model.CastDB(model));
            return ToJsonResult("Success");
        }

        public async Task<ActionResult> EditRole(string id)
        {
            RoleDAL dal = new RoleDAL();
            var db = await dal.GetRole(id);
            var model = new RoleViewModel().CastView(db);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditRole(RoleViewModel model)
        {
            RoleDAL dal = new RoleDAL();
            await dal.Update(model.CastDB(model));
            return ToJsonResult("Success");
        }

        public async Task<ActionResult> DeleteRole(string id)
        {
            RoleDAL dal = new RoleDAL();
            await dal.Delete(id);
            return ToJsonResult("Success");
        }


        public async Task<ActionResult> ChangeRoleIndex(string id)
        {
            RoleDAL dal = new RoleDAL();
            var allrole = await dal.GetAll();
            var rlist = dal.GetRoleByUser(id);
            List<CheckBoxListModel> list = new List<CheckBoxListModel>();
            foreach (var p in allrole)
            {
                list.Add(new CheckBoxListModel(p.ID, p.Name + "(" + p.Description.Trim() + ")", rlist.Any(t => t.ID == p.ID)));
            }
            return View(list);
        }
    }
}