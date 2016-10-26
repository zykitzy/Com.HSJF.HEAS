using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.ExtendTools.MessageExtend.Model
{
    public class MessageLog
    {
        public Guid ID { get; set; }
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
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 发送的系统
        /// </summary>
        public string SYSCode { get; set; }
        /// <summary>
        /// 发送状态  (0：未发送，1：已发送，2：发送失败)
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public int Category { get; set; }
    }
}
