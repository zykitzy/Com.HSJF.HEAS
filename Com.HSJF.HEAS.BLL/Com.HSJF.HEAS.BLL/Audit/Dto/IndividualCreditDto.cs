using System.Collections.Generic;

namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    public class IndividualCreditDto
    {
        public string ID { get; set; }

        public string PersonID { get; set; }

        public string PersonIDText { get; set; }

        /// <summary>
        /// 信用卡
        /// </summary>
        public string CreditCard { get; set; }

        /// <summary>
        /// 房贷车贷
        /// </summary>
        public string CreditInfo { get; set; }

        /// <summary>
        /// 其他贷款
        /// </summary>
        public string OtherCredit { get; set; }

        /// <summary>
        /// 逾期情况
        /// </summary>
        public string OverdueInfo { get; set; }

        /// <summary>
        /// 个人征信报告文件
        /// </summary>
        public string IndividualFile { get; set; }

        public Dictionary<string, string> IndividualFileName { get; set; }

        /// <summary>
        /// 银行流水文件
        /// </summary>
        public string BankFlowFile { get; set; }

        public Dictionary<string, string> BankFlowFileName { get; set; }

        /// <summary>
        /// 审核ID
        /// </summary>
        public string BaseAuditID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }
    }
}
