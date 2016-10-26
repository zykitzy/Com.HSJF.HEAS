using Com.HSJF.Framework.DAL.Other;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Com.HSJF.HEAS.Web.Helper
{
    public class DictionaryHelper
    {
        public IEnumerable<SelectListItem> GetDicList(string key, string parentkey)
        {
            DictionaryDAL dal = new DictionaryDAL();
            var viewList = new List<SelectListItem>();
            var list = dal.GetAll().Where(t => t.State).OrderBy(t => t.Desc);
            foreach (var vi in list)
            {
                if ((vi.ParentKey == key || vi.Key == key) && (key != null))
                {
                    continue;
                }
                SelectListItem se = new SelectListItem();
                se.Text = vi.Text;
                se.Value = vi.Path;
                if (key == vi.Path)
                {
                    se.Selected = true; ;
                }
                viewList.Add(se);
            }
            viewList.Insert(0, new SelectListItem() { Text = "最高级", Value = string.Empty });


            return viewList;
        }

        public IEnumerable<SelectListItem> GetListByType(string key, string parentkey)
        {
            DictionaryDAL dal = new DictionaryDAL();
            var viewList = new List<SelectListItem>();
            var list = dal.FindByParentKey(parentkey);
            foreach (var vi in list)
            {
                SelectListItem se = new SelectListItem();
                se.Text = vi.Text;
                se.Value = vi.Path;
                if (key == vi.Path)
                {
                    se.Selected = true; ;
                }
                viewList.Add(se);
            }

            return viewList;
        }

        /// <summary>
        /// 获取同一类型的所有字典，不过滤状态
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parentkey"></param>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetListByTypeAll(string key, string parentkey)
        {
            DictionaryDAL dal = new DictionaryDAL();
            var viewList = new List<SelectListItem>();
            var list = dal.FindByParentKeyAll(parentkey);
            foreach (var vi in list)
            {
                SelectListItem se = new SelectListItem();
                se.Text = vi.Text;
                se.Value = vi.Path;
                se.Disabled = vi.State;
                if (key == vi.Path)
                {
                    se.Selected = true; ;
                }
                viewList.Add(se);
            }

            return viewList;
        }

    }
}
