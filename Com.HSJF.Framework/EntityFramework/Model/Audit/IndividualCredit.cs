using Com.HSJF.Infrastructure.DoMain;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit
{
    public class IndividualCredit : EntityModel
    {
        public string ID { get; set; }

        public string CreditCard { get; set; }
        public string CreditInfo { get; set; }
        public string OtherCredit { get; set; }
        public string OverdueInfo { get; set; }
        public string IndividualFile { get; set; }
        public string BankFlowFile { get; set; }
        public string BaseAuditID { get; set; }
        public string PersonID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        public virtual BaseAudit BaseAudit { get; set; }
    }
}
