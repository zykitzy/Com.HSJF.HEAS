using Com.HSJF.Framework.DAL.Sales;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Com.HSJF.HEAS.Web.Helper
{
    public class SalesGroupList
    {
        public IEnumerable<SelectListItem> GroupList(string id)
        {

            SalesGroupDAL dal = new SalesGroupDAL();
            var viewList = new List<SelectListItem>();
            var list = dal.GetAll();
            foreach (var vi in list)
            {
                SelectListItem se = new SelectListItem();
                se.Text = vi.Name;
                se.Value = vi.ID.ToString();
                if (id == vi.ID)
                {
                    se.Selected = true; ;
                }
                viewList.Add(se);
            }
            return viewList;

        }

        public IEnumerable<SelectListItem> GroupBySales(string id)
        {

            SalesGroupDAL dal = new SalesGroupDAL();
            SalesManDAL sd = new SalesManDAL();
            var sales = sd.Get(id);

            var viewList = new List<SelectListItem>();
            var list = dal.GetAll();
            foreach (var vi in list)
            {
                SelectListItem se = new SelectListItem();
                se.Text = vi.Name;
                se.Value = vi.ID.ToString();
                if (sales != null && sales.GroupID == vi.ID)
                {
                    se.Selected = true; ;
                }
                viewList.Add(se);
            }
            viewList.Insert(0, new SelectListItem() { Value = string.Empty, Text = "请选择" });
            return viewList;

        }

        public IEnumerable<SelectListItem> SalesList(string id)
        {
            SalesManDAL dal = new SalesManDAL();
            var viewList = new List<SelectListItem>();
            var model = dal.Get(id);
            var list = dal.GetAll().Where(t => t.GroupID == model.GroupID);

            foreach (var vi in list)
            {
                SelectListItem se = new SelectListItem();
                se.Text = vi.Name;
                se.Value = vi.ID.ToString();
                if (id == vi.ID)
                {
                    se.Selected = true; ;
                }
                viewList.Add(se);
            }
            return viewList;
        }

        public IEnumerable<SelectListItem> DistrictList(string id)
        {

            DistrictDAL dal = new DistrictDAL();
            var viewList = new List<SelectListItem>();
            var list = dal.GetAll();
            foreach (var vi in list)
            {
                SelectListItem se = new SelectListItem();
                se.Text = vi.Name;
                se.Value = vi.ID.ToString();
                if (id == vi.ID)
                {
                    se.Selected = true; ;
                }
                viewList.Add(se);
            }
            return viewList;

        }
    }
}
