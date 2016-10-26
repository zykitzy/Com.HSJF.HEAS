using Com.HSJF.Framework.DAL.SystemSetting;
using Com.HSJF.HEAS.Web.Models.Menu;
using System;
using System.Linq;
using System.Web.Mvc;
using Com.HSJF.Framework.DAL;
using System.Collections.Generic;

namespace Com.HSJF.HEAS.Web.Controllers
{
    [Authorize]
    public class MenuController : BaseController
    {
        // GET: Menu
        public ActionResult Index()
        {
            MenuDAL dal = new MenuDAL();
            var model = dal.GetAll().Select(t => new MenuViewModel()
            {
                ID = t.ID,
                Description = t.Description,
                Name = t.Name,
                ParentID = t.ParentID,
                State = t.State,
                Icon = t.Icon,
                Url = t.Url
            }).OrderBy(t => t.State);
            return View(model);
        }

        [Authorize(Roles = "IT,admin")]
        public ActionResult AddMenu()
        {
            return View();
        }

        [Authorize(Roles = "IT,admin")]
        [HttpPost]
        public ActionResult AddMenu(MenuViewModel model)
        {
            MenuDAL dal = new MenuDAL();
            model.ID = Guid.NewGuid().ToString();
            var db = model.CastDB(model);
            dal.Add(db);
            dal.AcceptAllChange();
            return ToJsonResult("Success");
        }

        [Authorize(Roles = "IT,admin")]
        public ActionResult EditMenu(string id)
        {
            MenuDAL dal = new MenuDAL();
            var menu = dal.Get(id);
            MenuViewModel Vmodel = new MenuViewModel();
            Vmodel = Vmodel.CastModel(menu);
            return View(Vmodel);
        }

        [Authorize(Roles = "IT,admin")]
        [HttpPost]
        public ActionResult EditMenu(MenuViewModel model)
        {
            MenuDAL dal = new MenuDAL();
            var db = model.CastDB(model);
            dal.Update(db);
            dal.AcceptAllChange();
            return Content("Success");
        }

        [Authorize(Roles = "IT,admin")]
        public ActionResult DeleteMenu(string id)
        {
            MenuDAL dal = new MenuDAL();
            dal.DeleteMenu(id);
            return Content("Success");
        }

        public ActionResult CurrentMenu()
        {
            User2RoleDAL urdal = new User2RoleDAL();
            //根据用户ID查询角色ID
            var urlist = urdal.GetUser2RoleByUserId(CurrentUser.Id).Select(s => s.RoleID).ToArray();
            MenuDAL dal = new MenuDAL();
            Menu2RoleDAL menurole = new Menu2RoleDAL();

            //User2MenuDAL u2mdal = new User2MenuDAL();
            // var u2mlist = u2mdal.GetAll().Where(o => o.UserID == CurrentUser.Id);

            //查询所有角色菜单
            var menurolelist = menurole.GetAll().Where(s => urlist.Contains(s.RoleID));
            //所有菜单
            var mlist = dal.GetAll().ToList();

            //var currlist = from i in mlist
            //               join j in u2mlist
            //               on i.ID equals j.MenuID
            //               select new MenuViewModel()
            //               {
            //                   ID = i.ID,
            //                   Name = i.Name,
            //                   Url = i.Url,
            //                   ParentID = i.ParentID,
            //                   Icon = i.Icon,
            //                   State = i.State
            //               };
            var currlist = from i in mlist
                           join j in menurolelist
                           on i.ID equals j.MenuID
                           select new MenuViewModel()
                           {
                               ID = i.ID,
                               Name = i.Name,
                               Url = i.Url,
                               ParentID = i.ParentID,
                               Icon = i.Icon,
                               State = i.State
                           };

            if (currlist.Any())
            {
                var list = currlist.Distinct(new Compare<MenuViewModel>(
                    delegate (MenuViewModel a, MenuViewModel b)
                    {
                        if (null == a || null == b)
                            return false;
                        else
                            return a.Url == b.Url;
                    })).OrderBy(t => t.State).ToList();
                return View(list);
            }
            else
            {
                return View(currlist);
            }
        }
    }
    public delegate bool EqualsComparer<T>(T x, T y);
    public class Compare<T> : IEqualityComparer<T>
    {
        private EqualsComparer<T> _equalsComparer;

        public Compare(EqualsComparer<T> equalsComparer)
        {
            this._equalsComparer = equalsComparer;
        }

        public bool Equals(T x, T y)
        {
            if (null != this._equalsComparer)
                return this._equalsComparer(x, y);
            else
                return false;
        }

        public int GetHashCode(T obj)
        {
            return obj.ToString().GetHashCode();
        }
    }


}