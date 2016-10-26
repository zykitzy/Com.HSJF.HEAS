using Com.HSJF.Framework.EntityFramework.Model.Biz;
using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Biz
{
    public class ContactViewModel
    {
        [Key]
        public string ID { get; set; }

        [Display(Name = "联系方式")]
        [Required]
        public string ContactType { get; set; }

        public string ContactTypeText { get; set; }

        [Display(Name = "号码")]
        [Required]
        public string ContactNumber { get; set; }
      
        public string PersonID { get; set; }
       
        [Display(Name = "是否为默认联系方式")]
        public bool IsDefault { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        public ContactViewModel CastDB(Contact db)
        {
            ContactViewModel model = new ContactViewModel();
            Infrastructure.ExtendTools.ObjectExtend.CopyTo(db, model);
            return model;
        }
        public Contact CastModel(ContactViewModel model)
        {
            Contact db = new Contact();
            Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, db);
            return db;
        }
    }
}
