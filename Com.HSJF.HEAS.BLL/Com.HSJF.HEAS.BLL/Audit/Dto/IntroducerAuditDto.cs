
namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    public class IntroducerAuditDto
    {
        /// <summary>
        /// 介绍人Id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 介绍人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 介绍人联系电话
        /// </summary>
        public string Contract { get; set; }

        /// <summary>
        /// 返利金额
        /// </summary>
        public decimal? RebateAmmount { get; set; }

        /// <summary>
        /// 返利百分比
        /// </summary>
        public decimal? RebateRate { get; set; }

        /// <summary>
        /// 介绍人账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 介绍人开户行
        /// </summary>
        public string AccountBank { get; set; }

        /// <summary>
        /// 审核案件Id
        /// </summary>
        public string AuditID { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sequence { get; set; }
    }
}
