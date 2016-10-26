using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Biz
{
    public class RelationEnterpriseViewModel
    {
        [Key]
        public string ID { get; set; }
        public string PersonID { get; set; }
       
        [Display(Name = "企业描述")]
        public string EnterpriseDes { get; set; }
       
        [Display(Name = "企业名称")]
        [Required]
        public string EnterpriseName { get; set; }
        [Display(Name = "企业注册号")]
        
        [Required]
        public string RegisterNumber { get; set; }
      
        [Display(Name = "企业法人代表")]
        public string LegalPerson { get; set; }
      
        [Display(Name = "股东情况")]
        public string ShareholderDetails { get; set; }
       
        [Display(Name = "企业地址")]
        public string Address { get; set; }
      
        [Display(Name = "企业注册资金")]
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
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
    }
}
