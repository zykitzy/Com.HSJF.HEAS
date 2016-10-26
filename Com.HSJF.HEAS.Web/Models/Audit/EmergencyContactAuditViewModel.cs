using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Audit
{
    public class EmergencyContactAuditViewModel
    {
       
        public string ID { get; set; }
        [Display(Name = "紧急联系人类型")]
        [Required]
        public string ContactType { get; set; }
        public string ContactTypeText { get; set; }
        [Display(Name = "紧急联系人姓名")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "紧急联系人号码")]
        [Required]
        public string ContactNumber { get; set; }
        /// <summary>
        /// 关系人ID
        /// </summary>
        public string PersonID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 关系人对象
        /// </summary>
        public virtual RelationPersonAuditViewModel RelationPersonAudit { get; set; }
    }
}