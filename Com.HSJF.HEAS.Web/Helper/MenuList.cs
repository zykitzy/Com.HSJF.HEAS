using Com.HSJF.Framework.DAL.SystemSetting;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Com.HSJF.HEAS.Web.Helper
{
    public class MenuList
    {
        public IEnumerable<SelectListItem> RoleList(string id, string parentid)
        {

            MenuDAL dal = new MenuDAL();
            var viewList = new List<SelectListItem>();
            var list = dal.GetAll();
            foreach (var vi in list)
            {
                if (vi.ParentID == id || vi.ID == id)
                {
                    continue;
                }
                SelectListItem se = new SelectListItem();
                se.Text = vi.Name;
                se.Value = vi.ID.ToString();
                if (parentid == vi.ID)
                {
                    se.Selected = true; ;
                }
                viewList.Add(se);
            }
            viewList.Insert(0,new SelectListItem() { Text = "最高级", Value = string.Empty });


            return viewList;

        }
    }
}
