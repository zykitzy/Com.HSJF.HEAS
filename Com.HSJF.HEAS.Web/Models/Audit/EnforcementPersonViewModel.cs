using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Audit
{
    public class EnforcementPersonViewModel
    {
        public string ID { get; set; }
        
        public string PersonID { get; set; }
        
        public string PersonIDText { get; set; }

        [Display(Name = "全国被执行网情况")]
        public string EnforcementWeb { get; set; }

        [Display(Name = "法院开庭记录")]
        public string TrialRecord { get; set; }
        
        [Display(Name = "汇法网")]
        public string LawXP { get; set; }
        
        [Display(Name = "失信网失信记录")]
        public string ShiXin { get; set; }
        
        [Display(Name = "社会负面新闻")]
        public string BadNews { get; set; }

        /// <summary>
        /// 审核ID
        /// </summary>
        public string BaseAuditID { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string AttachmentFile { get; set; }

        /// <summary>
        /// 附件文件
        /// </summary>
        public Dictionary<string,string> AttachmentFileName { get; set; }

        /// <summary>
        /// 审核详细
        /// </summary>
        public virtual BaseAuditViewModel BaseAudit { get; set; }
    }
}