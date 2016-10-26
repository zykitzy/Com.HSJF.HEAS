
using System.Collections.Generic;

namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    /// <summary>
    /// 相关企业信息
    /// </summary>
    public class RelationEnterpriseAuditDto
    {
        public string ID { get; set; }

        /// <summary>
        /// 企业描述
        /// </summary>
        public string EnterpriseDes { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string EnterpriseName { get; set; }

        /// <summary>
        /// 注册号/统一社会信用代码
        /// </summary>
        public string RegisterNumber { get; set; }

        /// <summary>
        /// 法人代表
        /// </summary>
        public string LegalPerson { get; set; }

        /// <summary>
        /// 股东情况
        /// </summary>
        public string ShareholderDetails { get; set; }

        /// <summary>
        /// 企业地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 注册资金
        /// </summary>
        public decimal? RegisteredCapital { get; set; }

        /// <summary>
        /// 主营业务
        /// </summary>
        public string MainBusiness { get; set; }

        /// <summary>
        /// 企业征信报告
        /// </summary>
        public string IndividualFile { get; set; }

        public Dictionary<string, string> IndividualFileName { get; set; }

        /// <summary>
        /// 银行流水
        /// </summary>
        public string BankFlowFile { get; set; }

        public Dictionary<string, string> BankFlowFileName { get; set; }

        /// <summary>
        /// 联系人ID
        /// </summary>
        public string PersonID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }
    }
}
