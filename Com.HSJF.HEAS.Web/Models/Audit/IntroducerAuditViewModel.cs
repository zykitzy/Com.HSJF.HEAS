using System.ComponentModel.DataAnnotations;

namespace Com.HSJF.HEAS.Web.Models.Audit
{
    public class IntroducerAuditViewModel
    {
        /// <summary>
        /// 介绍人Id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 介绍人姓名
        /// </summary>
        [Display(Name = "介绍人名称")]
        public string Name { get; set; }

        /// <summary>
        /// 介绍人联系电话
        /// </summary>
        [Display(Name = "介绍人联系电话")]
        public string Contract { get; set; }

        /// <summary>
        /// 返利金额
        /// </summary>
        [Display(Name = "返利金额")]
        public decimal? RebateAmmount { get; set; }

        /// <summary>
        /// 返利百分比
        /// </summary>
        [Display(Name = "返利百分比")]
        public decimal? RebateRate { get; set; }

        /// <summary>
        /// 介绍人账户
        /// </summary>
        [Display(Name = "介绍人账户")]
        public string Account { get; set; }

        /// <summary>
        /// 介绍人开户行
        /// </summary>
        [Display(Name = "介绍人开户行")]
        public string AccountBank { get; set; }

        /// <summary>
        /// 审核案件Id
        /// </summary>
        public string AuditID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }

        public BaseAuditViewModel BaseAudit { get; set; }
    }
}