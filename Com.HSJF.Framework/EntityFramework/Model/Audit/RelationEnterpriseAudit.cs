using Com.HSJF.Infrastructure.DoMain;
using System;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit
{
    public class RelationEnterpriseAudit : EntityModel
    {
        public string ID { get; set; }
        public string PersonID { get; set; }
        public string EnterpriseDes { get; set; }
        public string EnterpriseName { get; set; }
        public string RegisterNumber { get; set; }
        public string LegalPerson { get; set; }
        public string ShareholderDetails { get; set; }
        public string Address { get; set; }
        public Nullable<decimal> RegisteredCapital { get; set; }
        public string MainBusiness { get; set; }
        //个人征信报告
        public string IndividualFile { get; set; }
        //银行流水
        public string BankFlowFile { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        public virtual RelationPersonAudit RelationPersonAudit { get; set; }
    }
}
