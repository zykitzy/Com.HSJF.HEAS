using Com.HSJF.Framework.EntityFramework.Model.Audit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Audit
{
    public class ContactAuditViewModel
    {
        
        public string ID { get; set; }
        
        [Display(Name = "联系方式")]
        [Required]
        public string ContactType { get; set; }
        public string ContactTypeText { get; set; }
        
        [Display(Name = "号码")]
        [Required]
        public string ContactNumber { get; set; }
      
        [Display(Name = "是否为默认联系方式")]
        public bool IsDefault { get; set; }
        /// <summary>
        /// 联系人ID
        /// </summary>
        public string PersonID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 联系人详细信息
        /// </summary>
        public virtual RelationPersonAuditViewModel RelationPersonAudit { get; set; }

        public ContactAuditViewModel CastDB(ContactAudit db)
        {
            ContactAuditViewModel model = new ContactAuditViewModel();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(db, model);
            return model;
        }
        public ContactAudit CastModel(ContactAuditViewModel model)
        {
            ContactAudit db = new ContactAudit();
            Com.HSJF.Infrastructure.ExtendTools.ObjectExtend.CopyTo(model, db);
            return db;
        }
    }
}