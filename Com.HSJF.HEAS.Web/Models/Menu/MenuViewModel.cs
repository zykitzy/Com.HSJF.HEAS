using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.HSJF.Framework.EntityFramework.Model;

namespace Com.HSJF.HEAS.Web.Models.Menu
{
    public class MenuViewModel
    {
        public string ID { get; set; }

        [Required]
        [Display(Name = "菜单名称")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "页面路径")]
        public string Url { get; set; }

        [Display(Name = "图标")]
        public string Icon { get; set; }

        [Display(Name = "说明")]
        public string Description { get; set; }

        [Display(Name = "层级状态")]
        public int State { get; set; }

        [Display(Name = "父级菜单")]
        public string ParentID { get; set; }

        public MenuViewModel CastModel(Com.HSJF.Framework.EntityFramework.Model.SystemSetting.Menu menu)
        {
            MenuViewModel model = new MenuViewModel();
            model.ID = menu.ID;
            model.Description = menu.Description;
            model.Name = menu.Name;
            model.ParentID = menu.ParentID;
            model.State = menu.State;
            model.Url = menu.Url;
            model.Icon = menu.Icon;
            return model;
        }

        public Com.HSJF.Framework.EntityFramework.Model.SystemSetting.Menu CastDB(MenuViewModel menu)
        {
            Com.HSJF.Framework.EntityFramework.Model.SystemSetting.Menu model = new Framework.EntityFramework.Model.SystemSetting.Menu();
            model.ID = menu.ID;
            model.Description = menu.Description;
            model.Name = menu.Name;
            model.ParentID = menu.ParentID;
            model.State = menu.State;
            model.Url = menu.Url;
            model.Icon = menu.Icon;
            return model;
        }
    }

    public class UserMenuViewModel : MenuViewModel
    {
         public bool IsSelect { get; set; }
    }

}
