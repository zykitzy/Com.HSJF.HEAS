using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Audit
{
    public class RelationEnterpriseAuditViewModel
    {
       
        public string ID { get; set; }

        [Display(Name = "企业描述")]
        public string EnterpriseDes { get; set; }

        [Display(Name = "企业名称")]
        [Required]
        public string EnterpriseName { get; set; }

        [Display(Name = "注册号/统一社会信用代码")]
        [Required]
        public string RegisterNumber { get; set; }

        [Display(Name = "法的人代表")]
        public string LegalPerson { get; set; }

        [Display(Name = "股东情况")]
        public string ShareholderDetails { get; set; }

        [Display(Name = "企业地址")]
        public string Address { get; set; }

        [Display(Name = "注册资金")]
        public Nullable<decimal> RegisteredCapital { get; set; }

        [Display(Name = "主营业务")]
        public string MainBusiness { get; set; }

        [Display(Name = "企业征信报告")]
        public string IndividualFile { get; set; }
        public Dictionary<string, string> IndividualFileName { get; set; }
        [Display(Name = "银行流水")]
        public string BankFlowFile { get; set; }
        public Dictionary<string, string> BankFlowFileName { get; set; }
        /// <summary>
        /// 联系人ID
        /// </summary>
        public string PersonID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 联系人详细
        /// </summary>
        public virtual RelationPersonAuditViewModel RelationPersonAudit { get; set; }
    }
}