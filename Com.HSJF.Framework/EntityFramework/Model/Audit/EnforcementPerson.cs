using Com.HSJF.Infrastructure.DoMain;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit
{
    public class EnforcementPerson : EntityModel
    {
        public string ID { get; set; }
        public string EnforcementWeb { get; set; }
        public string TrialRecord { get; set; }
        public string LawXP { get; set; }
        public string ShiXin { get; set; }
        public string BadNews { get; set; }
        public string BaseAuditID { get; set; }
        public string PersonID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string AttachmentFile { get; set; }
        public virtual BaseAudit BaseAudit { get; set; }
    }
}
