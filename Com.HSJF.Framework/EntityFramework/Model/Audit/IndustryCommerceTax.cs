using Com.HSJF.Infrastructure.DoMain;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit
{
    public class IndustryCommerceTax : EntityModel
    {
        public string ID { get; set; }

        public string AnnualInspection { get; set; }
        public string ActualManagement { get; set; }
        public string Description { get; set; }
        public string BaseAuditID { get; set; }
        public string EnterpriseID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        public virtual BaseAudit BaseAudit { get; set; }
    }
}
