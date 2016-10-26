using System;
using System.Collections.Generic;

namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    public class RelationPersonAuditDto
    {
        public string ID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string RelationType { get; set; }

        /// <summary>
        /// 类型Text
        /// </summary>
        public string RelationTypeText { get; set; }

        /// <summary>
        /// 与借款人关系
        /// </summary>
        public string BorrowerRelation { get; set; }

        /// <summary>
        /// 与借款人关系
        /// </summary>
        public string BorrowerRelationText { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public string IdentificationType { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>
        public string IdentificationTypeText { get; set; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdentificationNumber { get; set; }

        /// <summary>
        /// 证件有效期
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 身份证复印件
        /// </summary>
        public string IdentificationFile { get; set; }

        /// <summary>
        /// 身份证复印件文件结合
        /// </summary>
        public Dictionary<string, string> IdentificationFileName { get; set; }

        //[Display(Name = "婚否")]
        //public Nullable<int> IsMarried { get; set; }
        //public string IsMarriedText { get; set; }

        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string MaritalStatus { get; set; }

        /// <summary>
        /// 婚姻状况text
        /// </summary>
        public string MaritalStatusText { get; set; }

        /// <summary>
        /// 结婚证
        /// </summary>
        public string MarryFile { get; set; }

        /// <summary>
        /// 结婚证文件集合
        /// </summary>
        public Dictionary<string, string> MarryFileName { get; set; }

        /// <summary>
        /// 单身证明
        /// </summary>
        public string SingleFile { get; set; }

        /// <summary>
        /// 单身证明文件
        /// </summary>
        public Dictionary<string, string> SingleFileName { get; set; }

        /// <summary>
        /// 出生证
        /// </summary>
        public string BirthFile { get; set; }

        public Dictionary<string, string> BirthFileName { get; set; }

        /// <summary>
        /// 户口本
        /// </summary>
        public string AccountFile { get; set; }

        public Dictionary<string, string> AccountFileName { get; set; }

       /// <summary>
        /// 收入证明(授薪人士)
       /// </summary>
        public string SalaryPersonFile { get; set; }

        public Dictionary<string, string> SalaryPersonFileName { get; set; }

        /// <summary>
        /// 收入证明(自雇有执照)
        /// </summary>
        public string SelfLicenseFile { get; set; }

        public Dictionary<string, string> SelfLicenseFileName { get; set; }

        /// <summary>
        /// 收入证明(自雇无执照)
        /// </summary>
        public string SelfNonLicenseFile { get; set; }

        public Dictionary<string, string> SelfNonLicenseFileName { get; set; }

        /// <summary>
        /// 收入证明说明情况
        /// </summary>
        public string SalaryDescription { get; set; }

        /// <summary>
        /// 是否共同借款人
        /// </summary>
        public int? IsCoBorrower { get; set; }

        public string IsCoBorrowerText { get; set; }

        /// <summary>
        /// 担保方式
        /// </summary>
        public string Warranty { get; set; }

        /// <summary>
        /// 个人征信报告
        /// </summary>
        public string IndividualFile { get; set; }

        public Dictionary<string, string> IndividualFileName { get; set; }

        /// <summary>
        /// 银行流水
        /// </summary>
        public string BankFlowFile { get; set; }

        public Dictionary<string, string> BankFlowFileName { get; set; }

        /// <summary>
        /// 其他证明
        /// </summary>
        public string OtherFile { get; set; }

        public Dictionary<string, string> OtherFileName { get; set; }

        /// <summary>
        /// 审核ID
        /// </summary>
        public string AuditID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public virtual IEnumerable<ContactAuditDto> ContactAudits { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public virtual IEnumerable<AddressAuditDto> AddressAudits { get; set; }

        /// <summary>
        /// 相关企业
        /// </summary>
        public virtual IEnumerable<RelationEnterpriseAuditDto> RelationEnterpriseAudits { get; set; }

        /// <summary>
        /// 紧急联系人
        /// </summary>
        public virtual IEnumerable<RelationEnterpriseAuditDto> EmergencyContactAudits { get; set; }
    }
}
