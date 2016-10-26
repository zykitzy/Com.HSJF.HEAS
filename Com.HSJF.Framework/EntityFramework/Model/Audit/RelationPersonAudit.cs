using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;

namespace Com.HSJF.Framework.EntityFramework.Model.Audit
{
    public class RelationPersonAudit : EntityModel
    {
        public RelationPersonAudit()
        {
            this.ContactAudits = new List<ContactAudit>();
            this.AddressAudits = new List<AddressAudit>();
            this.RelationEnterpriseAudits = new List<RelationEnterpriseAudit>();
            this.EmergencyContactAudits = new List<EmergencyContactAudit>();
        }

        public string ID { get; set; }
        public string AuditID { get; set; }
        public string RelationType { get; set; }
        public string BorrowerRelation { get; set; }
        public string Name { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string IdentificationFile { get; set; }
        public string MaritalStatus { get; set; }
        public string MarryFile { get; set; }
        public string SingleFile { get; set; }
        public string BirthFile { get; set; }
        public string AccountFile { get; set; }
        public string SalaryPersonFile { get; set; }
        public string SelfLicenseFile { get; set; }
        public string SelfNonLicenseFile { get; set; }
        public string SalaryDescription { get; set; }
        public string FaceReportFile { get; set; }
        public string FieldReportFile { get; set; }
        public string LoanDetailReportFile { get; set; }

        //以下 2-16-06-16 第一次测试之后新增
        public int? IsCoBorrower { get; set; }
        public string Warranty { get; set; }
        //个人征信报告
        public string IndividualFile { get; set; }
        //银行流水
        public string BankFlowFile { get; set; }
        //其他证明
        public string OtherFile { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
        public virtual BaseAudit BaseAudit { get; set; }
        public virtual ICollection<ContactAudit> ContactAudits { get; set; }
        public virtual ICollection<AddressAudit> AddressAudits { get; set; }
        public virtual ICollection<RelationEnterpriseAudit> RelationEnterpriseAudits { get; set; }
        public virtual ICollection<EmergencyContactAudit> EmergencyContactAudits { get; set; }
    }
}
