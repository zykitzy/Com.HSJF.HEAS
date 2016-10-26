using Com.HSJF.Framework.DAL.Sales;
using Com.HSJF.HEAS.Web.Models;
using Com.HSJF.HEAS.Web.Models.Sales;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Com.HSJF.HEAS.Web.Controllers
{
    [Authorize(Roles = "admin,IT,Recording,1Audit,2Audit")]
    public class SalesController : BaseController
    {
        #region 销售
        public ActionResult Index()
        {
            SalesManDAL smdal = new SalesManDAL();
            var model = smdal.GetAll().ToList().Select(t => new SalesManViewModel().CastModel(t));
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(SalesManViewModel model)
        {
            SalesManDAL smdal = new SalesManDAL();
            model.ID = Guid.NewGuid().ToString();
            smdal.Add(model.CastDB(model));
            smdal.AcceptAllChange();

            return GetBaseResponse<SalesManViewModel>(true);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            SalesManDAL smdal = new SalesManDAL();
            var model = new SalesManViewModel();
            model = model.CastModel(smdal.Get(id));
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(SalesManViewModel model)
        {
            SalesManDAL smdal = new SalesManDAL();
            smdal.Update(model.CastDB(model));
            smdal.AcceptAllChange();
            return GetBaseResponse<SalesManViewModel>(true);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            SalesManDAL smdal = new SalesManDAL();
            var model = smdal.Get(id);
            smdal.Delete(model);
            smdal.AcceptAllChange();
            return GetBaseResponse<SalesManViewModel>(true);
        }

        #endregion

        #region 地区
        /// <summary>
        /// 获取所有地区
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDistrict()
        {
            DistrictDAL sgdal = new DistrictDAL();
            var sglist = sgdal.GetAll().Select(x => new { x.ID, x.Name }).ToList();
            BaseResponse<object> br = new BaseResponse<object>();
            br.Status = "Success";
            br.Data = sglist;
            return Json(br, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 地区视图
        /// </summary>
        /// <returns></returns>
        public ActionResult DistrictIndex()
        {
            DistrictDAL sgdal = new DistrictDAL();
            var sglist = sgdal.GetAll().ToList().Select(t => new DistrictViewModel().CastModel(t));
            return View(sglist);
        }
        /// <summary>
        /// 新增地区
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateDistrict()
        {
            return View();
        }
        /// <summary>
        /// 保存新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateDistrict(DistrictViewModel model)
        {
            DistrictDAL sgdal = new DistrictDAL();
            model.ID = Guid.NewGuid().ToString();
            sgdal.Add(model.CastDB(model));
            sgdal.AcceptAllChange();
            return GetBaseResponse<DistrictViewModel>(true);
        }
        /// <summary>
        /// 修改地区
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditDistrict(string id)
        {
            DistrictDAL sgdal = new DistrictDAL();
            var model = new DistrictViewModel();
            model = model.CastModel(sgdal.Get(id));
            return View(model);
        }
        /// <summary>
        /// 保存修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditDistrict(DistrictViewModel model)
        {
            DistrictDAL sgdal = new DistrictDAL();
            sgdal.Update(model.CastDB(model));
            sgdal.AcceptAllChange();
            return GetBaseResponse<DistrictViewModel>(true);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteDistrict(string id)
        {
            DistrictDAL sgdal = new DistrictDAL();
            var model = sgdal.Get(id);
            sgdal.Delete(model);
            sgdal.AcceptAllChange();
            return GetBaseResponse<DistrictViewModel>(true);
        }
        #endregion

        #region 销售组

        public ActionResult GroupIndex()
        {
            SalesGroupDAL sgdal = new SalesGroupDAL();
            var sglist = sgdal.GetAll().ToList().Select(t => new SalesGroupViewModel().CastModel(t));
            return View(sglist);
        }

        public ActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGroup(SalesGroupViewModel model)
        {
            SalesGroupDAL sgdal = new SalesGroupDAL();
            model.ID = Guid.NewGuid().ToString();
            sgdal.Add(model.CastDB(model));
            sgdal.AcceptAllChange();
            return GetBaseResponse<DistrictViewModel>(true);
        }

        public ActionResult EditGroup(string id)
        {
            SalesGroupDAL sgdal = new SalesGroupDAL();
            var model = new SalesGroupViewModel();
            model = model.CastModel(sgdal.Get(id));
            return View(model);
        }

        [HttpPost]
        public ActionResult EditGroup(SalesGroupViewModel model)
        {
            SalesGroupDAL sgdal = new SalesGroupDAL();
            sgdal.Update(model.CastDB(model));
            sgdal.AcceptAllChange();
            return GetBaseResponse<DistrictViewModel>(true);
        }

        [HttpPost]
        public ActionResult DeleteGroup(string id)
        {
            SalesGroupDAL sgdal = new SalesGroupDAL();
            var model = sgdal.Get(id);
            sgdal.Delete(model);
            sgdal.AcceptAllChange();
            return GetBaseResponse<DistrictViewModel>(true);
        }
        /// <summary>
        /// 根据地区获取所有销售团队
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSalesGroup(string districtId)
        {
            SalesGroupDAL sgdal = new SalesGroupDAL();
            var sglist = sgdal.GetAll().Where(t => t.DistrictID == districtId && t.State == 1).Select(x => new
            {
                ID = x.ID,
                Name = x.Name
            }).ToList();
            BaseResponse<object> br = new BaseResponse<object>();
            br.Status = "Success";
            br.Data = sglist;
            return Json(br, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据销售团队获取所有销售人员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSales(string groupid)
        {
            SalesManDAL smdal = new SalesManDAL();
            var salesman = smdal.GetAll().Where(t => t.GroupID == groupid && t.State == 1).ToList().Select(t => new SalesManViewModel().CastModel(t));
            BaseResponse<object> br = new BaseResponse<object>();
            br.Status = "Success";
            br.Data = salesman;
            return Json(br, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}