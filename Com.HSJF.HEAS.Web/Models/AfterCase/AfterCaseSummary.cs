using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.HSJF.HEAS.Web.Models.AfterCase
{
    /// <summary>
    /// 贷后案件摘要信息
    /// </summary>
    public class AfterCaseSummary
    {
        /// <summary>
        /// 案件ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 业务编号
        /// </summary>
        public string CaseNum { get; set; }
    }
}