using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Biz
{
    public class RelationPersonViewModel
    {
        [Key]
        public string ID { get; set; }

        public string CaseID { get; set; }

        [Display(Name = "名称")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "关系类型")]
        [Required]
        public string RelationType { get; set; }

        public string RelationTypeText { get; set; }

        [Display(Name = "与借款人关系")]
        public string BorrowerRelation { get; set; }

        public string BorrowerRelationText { get; set; }

        [Display(Name = "证件类型")]
        [Required]
        public string IdentificationType { get; set; }

        public string IdentificationTypeText { get; set; }

        [Display(Name = "证件号码")]
        [Required]
        public string IdentificationNumber { get; set; }

        [Display(Name = "证件有效期")]
        [Required]
        public Nullable<System.DateTime> ExpiryDate { get; set; }

        [Display(Name = "出生日期")]
        public Nullable<System.DateTime> Birthday { get; set; }

        [Display(Name = "证件复印件")]
        public string IdentificationFile { get; set; }

        public Dictionary<string, string> IdentificationFileName { get; set; }

        [Display(Name = "婚姻状况")]
        public string MaritalStatus { get; set; }

        public string MaritalStatusText { get; set; }
        //[Display(Name = "是否已婚")]
        //public Nullable<int> IsMarried { get; set; }
        //public string IsMarriedText { get; set; }

        [Display(Name = "结婚证")]
        public string MarryFile { get; set; }

        public Dictionary<string, string> MarryFileName { get; set; }

        [Obsolete("2016-9-8 大改废弃")]
        [Display(Name = "单身证明")]
        public string SingleFile { get; set; }

        [Obsolete("2016-9-8大改废弃")]
        public Dictionary<string, string> SingleFileName { get; set; }

        [Display(Name = "出生证")]
        public string BirthFile { get; set; }

        public Dictionary<string, string> BirthFileName { get; set; }

        [Display(Name = "户口本")]
        public string AccountFile { get; set; }

        public Dictionary<string, string> AccountFileName { get; set; }

        [Display(Name = "收入证明(授薪人士)")]
        public string SalaryPersonFile { get; set; }

        public Dictionary<string, string> SalaryPersonFileName { get; set; }

        [Display(Name = "收入证明(自雇有执照)")]
        public string SelfLicenseFile { get; set; }

        public Dictionary<string, string> SelfLicenseFileName { get; set; }

        [Display(Name = "收入证明(自雇无执照)")]
        public string SelfNonLicenseFile { get; set; }

        public Dictionary<string, string> SelfNonLicenseFileName { get; set; }

        [Display(Name = "收入证明说明")]
        public string SalaryDescription { get; set; }

        [Display(Name = "担保方式")]
        public string Warranty { get; set; }

        [Display(Name = " 是否共同借款人")]
        public int? IsCoBorrower { get; set; }

        public string IsCoBorrowerText { get; set; }

        [Display(Name = " 个人征信报告")]
        public string IndividualFile { get; set; }

        public Dictionary<string, string> IndividualFileName { get; set; }

        [Display(Name = "银行流水")]
        public string BankFlowFile { get; set; }

        public Dictionary<string, string> BankFlowFileName { get; set; }

        [Display(Name = "其他证明")]
        public string OtherFile { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 是否锁住
        /// yanminchun 2016-10-19
        /// </summary>
        public bool? IsLocked { get; set; }

        public Dictionary<string, string> OtherFileName { get; set; }

        public IEnumerable<ContactViewModel> Contacts { get; set; }

        public IEnumerable<RelationEnterpriseViewModel> RelationEnterprise { get; set; }

        public IEnumerable<EmergencyContactViewModel> EmergencyContacts { get; set; }

        public IEnumerable<AddressViewModel> Addresses { get; set; }
    }
}