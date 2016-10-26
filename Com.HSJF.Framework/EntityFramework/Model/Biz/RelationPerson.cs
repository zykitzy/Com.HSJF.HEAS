using Com.HSJF.Infrastructure.DoMain;
using System;
using System.Collections.Generic;

namespace Com.HSJF.Framework.EntityFramework.Model.Biz
{
    public partial class RelationPerson : EntityModel
    {
        public RelationPerson()
        {
            this.Contacts = new List<Contact>();
            this.Addresses = new List<Address>();
            this.RelationEnterprises = new List<RelationEnterprise>();
            this.EmergencyContacts = new List<EmergencyContact>();
        }

        public string ID { get; set; }
        public string CaseID { get; set; }
        public string RelationType { get; set; }
        public string BorrowerRelation { get; set; }
        public string Name { get; set; }
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? Birthday { get; set; }
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

        // 以下 2-16-06-16 第一次测试之后新增
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
        public virtual BaseCase BaseCase { get; set; }

        /// <summary>
        /// 是否锁住
        /// yanminchun 2016-10-19
        /// </summary>
        public bool? IsLocked { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<RelationEnterprise> RelationEnterprises { get; set; }
        public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; }
    }
}
