using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Audit
{
    public class GuarantorViewModel
    {
        public string ID { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "联系电话")]
        public string ContactNumber { get; set; }

        [Display(Name = "与借款人关系")]
        public string RelationType { get; set; }
        
        public string RelationTypeText { get; set; }

        [Display(Name = "担保方式")]
        public string GuarantType { get; set; }
        
        public string GuarantTypeText { get; set; }

        [Display(Name = "证件类型")]
        public string IdentityType { get; set; }
        
        public string IdentityTypeText { get; set; }
        
        [Display(Name = "证件号码")]
        public string IdentityNumber { get; set; }

        [Display(Name = "家庭地址")]
        public string Address { get; set; }

        [Display(Name = "婚姻状况")]
        public string MarriedInfo { get; set; }
        
        /// <summary>
        /// 审核ID
        /// </summary>
        public string BaseAuditID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 审核详细
        /// </summary>
        public virtual BaseAuditViewModel BaseAudit { get; set; }
    }
}