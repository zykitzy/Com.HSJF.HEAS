
namespace Com.HSJF.HEAS.BLL.Audit.Dto
{
    public class EnforcementPersonDto
    {
        public string ID { get; set; }

        public string PersonID { get; set; }

        public string PersonIDText { get; set; }

        /// <summary>
        /// 全国被执行网情况
        /// </summary>
        public string EnforcementWeb { get; set; }

        /// <summary>
        /// 法院开庭记录
        /// </summary>
        public string TrialRecord { get; set; }

        /// <summary>
        /// 汇法网记录
        /// </summary>
        public string LawXP { get; set; }

        /// <summary>
        /// 失信网失信记录
        /// </summary>
        public string ShiXin { get; set; }

        /// <summary>
        /// 社会负面新闻
        /// </summary>
        public string BadNews { get; set; }

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
