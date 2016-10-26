using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Audit
{
    public class IndustryCommerceTaxViewModel
    {
        public string ID { get; set; }
        
        public string EnterpriseID { get; set; }
        
        public string EnterpriseIDText { get; set; }

        [Display(Name = "企业年检情况")]
        public string AnnualInspection { get; set; }

        [Display(Name = "公司实际经营")]
        public string ActualManagement { get; set; }

        [Display(Name = "备注")]
        public string Description { get; set; }

       
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