
namespace Com.HSJF.HEAS.BLL.DataStatistics.Dto
{
    public class EmailRequest
    {
        /// <summary>
        /// 发送人
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        public string Recipient { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 发送的系统
        /// </summary>
        public string SYSCode { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 是否为HTML格式
        /// </summary>
        public int IsHtml { get; set; }
    }
}
