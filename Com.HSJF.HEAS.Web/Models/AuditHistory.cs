using System;

namespace Com.HSJF.HEAS.Web.Models
{
    public class AuditHistory
    {
        public string CreatUser { get; set; }
       
        public DateTime? CreateTime { get; set; }
       
        public string CaseStatusTest { get; set; }
       
        public string Action { get; set; }
       
        /// <summary>
        /// 审批意见
        /// </summary>
        public string Description { get; set; }
       
        /// <summary>
        /// 拒绝理由
        /// </summary>
        public string RejectType { get; set; }
    }
}