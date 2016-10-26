using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Audit
{
    public class IndividualCreditViewModel
    {
        public string ID { get; set; }

        public string PersonID { get; set; }
      
        public string PersonIDText { get; set; }

        [Display(Name = "信用卡")]
        public string CreditCard { get; set; }

        [Display(Name = "房贷、车贷")]
        public string CreditInfo { get; set; }

        [Display(Name = "其他贷款")]
        public string OtherCredit { get; set; }

        [Display(Name = "逾期情况")]
        public string OverdueInfo { get; set; }

        [Display(Name = "个人征信报告")]
        public string IndividualFile { get; set; }
        
        public Dictionary<string, string> IndividualFileName { get; set; }

        [Display(Name = "银行流水")]
        public string BankFlowFile { get; set; }
        
        public Dictionary<string, string> BankFlowFileName { get; set; }

        /// <summary>
        /// 审核ID
        /// </summary>
        public string BaseAuditID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 审核详细
        /// </summary>
        public virtual BaseAuditViewModel BaseAudit { get; set; }
    }
}