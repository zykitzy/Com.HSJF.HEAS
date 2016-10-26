using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Audit
{
    public class EnterpriseCreditViewModel
    {
        public string ID { get; set; }
        
        public string EnterpriseID { get; set; }
        
        public string EnterpriseIDText { get; set; }

        [Display(Name = "企业贷款卡")]
        public string CreditCard { get; set; }

        [Display(Name = "企业负债信息")]
        public string CreditInfo { get; set; }
        
        [Display(Name = "股东情况")]
        public string ShareholderDetails { get; set; }
        
        [Display(Name = "其他情况说明")]
        public string OtherDetailes { get; set; }
        
        /// <summary>
        /// 审核ID
        /// </summary>
        public string BaseAuditID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 审核信息
        /// </summary>
        public virtual BaseAuditViewModel BaseAudit { get; set; }
    }
}