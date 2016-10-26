using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Framework.DAL.CaseALL.Model
{
    public class CaseAll
    {
        public string ID { get; set; }
        public string NewCaseNum { get; set; }
        public string SalesGroupID { get; set; }
        public string BorrowerName { get; set; }
        /// <summary>
        /// 案件模式
        /// </summary>
        public string CaseMode { get; set; }

        /// <summary>
        /// 第三方资金
        /// </summary>
        public string ThirdParty { get; set; }
        /// <summary>
        /// 审批金额
        /// </summary>
        public decimal? AuditAmount { get; set; }

        /// <summary>
        /// 审批贷款期限
        /// </summary>
        public string AuditTerm { get; set; }

        public string CaseStatus { get; set; }
        /// <summary>
        /// 实际放款日
        /// </summary>
        public DateTime? LendTime { get; set; }

    }
}
