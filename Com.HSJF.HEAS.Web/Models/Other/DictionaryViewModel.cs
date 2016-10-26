using Com.HSJF.Framework.DAL.Other;
using Com.HSJF.Framework.EntityFramework.Model.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.HEAS.Web.Models.Other
{
    public class DictionaryViewModel
    {
        [Key]
        [Display(Name = "主键")]
        public string Path { get; set; }

        [Required]
        [Display(Name = "字典关键字")]
        public string Key { get; set; }

        [Display(Name = "显示名称")]
        public string Text { get; set; }

        [Display(Name = "是否启用")]
        public bool State { get; set; }

        [Display(Name = "父级字典")]
        public string ParentKey { get; set; }

        [Display(Name = "排序")]
        public Nullable<int> Desc { get; set; }

        public virtual DictionaryViewModel CastModel(Dictionary db)
        {
            DictionaryViewModel model = new DictionaryViewModel();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(db, model);
            return model;
        }

        public Dictionary CastDB(DictionaryViewModel model)
        {
            Dictionary db = new Dictionary();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, db);
            return db;
        }
    }

    public class DictionaryListViewModel : DictionaryViewModel
    {
        [Display(Name = "父级字典")]
        public string ParentKeyDisplay { get; set; }

        public DictionaryListViewModel CastModel(Dictionary db)
        {
            DictionaryListViewModel model = new DictionaryListViewModel();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(db, model);
            DictionaryDAL dal = new DictionaryDAL();
            var parent = dal.Get(db.ParentKey);
            model.ParentKeyDisplay = parent == null ? "" : parent.Text;
            return model;
        }
    }
}
