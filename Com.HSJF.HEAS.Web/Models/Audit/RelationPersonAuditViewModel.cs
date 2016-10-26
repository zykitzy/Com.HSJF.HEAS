using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Audit
{
    public class RelationPersonAuditViewModel
    {
        public string ID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "类型")]
        [Required]
        public string RelationType { get; set; }

        /// <summary>
        /// 类型Text
        /// </summary>
        public string RelationTypeText { get; set; }

        /// <summary>
        /// 与借款人关系
        /// </summary>
        [Display(Name = "与借款人关系")]
        public string BorrowerRelation { get; set; }

        /// <summary>
        /// 与借款人关系
        /// </summary>
        public string BorrowerRelationText { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        [Display(Name = "证件类型")]
        [Required]
        public string IdentificationType { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public string IdentificationTypeText { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        [Display(Name = "证件号码")]
        [Required]
        public string IdentificationNumber { get; set; }

        /// <summary>
        /// 证件有效期
        /// </summary>
        [Display(Name = "证件有效期")]
        [Required]
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [Display(Name = "出生日期")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 身份证复印件
        /// </summary>
        [Display(Name = "证件复印件")]
        public string IdentificationFile { get; set; }

        /// <summary>
        /// 身份证复印件文件结合
        /// </summary>
        public Dictionary<string, string> IdentificationFileName { get; set; }

        //[Display(Name = "婚否")]
        //public Nullable<int> IsMarried { get; set; }
        //public string IsMarriedText { get; set; }

        /// <summary>
        /// 婚姻状况
        /// </summary>
        [Display(Name = "婚姻状况")]
        public string MaritalStatus { get; set; }

        /// <summary>
        /// 婚姻状况text
        /// </summary>
        public string MaritalStatusText { get; set; }

        /// <summary>
        /// 结婚证
        /// </summary>
        [Display(Name = "结婚证")]
        public string MarryFile { get; set; }

        /// <summary>
        /// 结婚证文件集合
        /// </summary>
        public Dictionary<string, string> MarryFileName { get; set; }

        /// <summary>
        /// 单身证明
        /// </summary>
        [Obsolete("2016-9-8 大改停用")]
        [Display(Name = "单身证明")]
        public string SingleFile { get; set; }

        /// <summary>
        /// 单身证明文件
        /// </summary>
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

        [Display(Name = "收入证明说明情况")]
        public string SalaryDescription { get; set; }

        [Display(Name = "是否共同借款人")]
        public int? IsCoBorrower { get; set; }

        public string IsCoBorrowerText { get; set; }

        [Display(Name = "担保方式")]
        public string Warranty { get; set; }

        [Display(Name = " 个人征信报告")]
        public string IndividualFile { get; set; }

        public Dictionary<string, string> IndividualFileName { get; set; }

        [Display(Name = "银行流水")]
        public string BankFlowFile { get; set; }

        public Dictionary<string, string> BankFlowFileName { get; set; }

        [Display(Name = "其他证明")]
        public string OtherFile { get; set; }

        public Dictionary<string, string> OtherFileName { get; set; }

        /// <summary>
        /// 来自于案件环节
        /// </summary>
        public string IsFrom { get; set; }

        /// <summary>
        /// 审核ID
        /// </summary>
        public string AuditID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 审核详细
        /// </summary>
        public virtual BaseAuditViewModel BaseAudits { get; set; }
       
        /// <summary>
        /// 联系方式
        /// </summary>
        public virtual IEnumerable<ContactAuditViewModel> ContactAudits { get; set; }
       
        /// <summary>
        /// 地址
        /// </summary>
        public virtual IEnumerable<AddressAuditViewModel> AddressAudits { get; set; }
       
        /// <summary>
        /// 相关企业
        /// </summary>
        public virtual IEnumerable<RelationEnterpriseAuditViewModel> RelationEnterpriseAudits { get; set; }
       
        /// <summary>
        /// 紧急联系人
        /// </summary>
        public virtual IEnumerable<EmergencyContactAuditViewModel> EmergencyContactAudits { get; set; }
    }
}