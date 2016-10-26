using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Com.HSJF.Infrastructure.ExtendTools
{
	public class EmailTool
	{
		/// <summary>
		/// 发送邮件
		/// </summary>
		/// <param name="strto">接收邮件地址</param>
		/// <param name="strSubject">主题</param>
		/// <param name="strBody">内容</param>
		public static void Send(string to, string subject, string content)
		{
			var prop = Properties.Settings.Default;

			SmtpClient client = new SmtpClient(prop.EmailServer);
			client.UseDefaultCredentials = false;
			client.Credentials = new NetworkCredential(prop.EmailSender, prop.EmailPassword);
			client.DeliveryMethod = SmtpDeliveryMethod.Network;

			MailMessage message = new MailMessage(prop.EmailSender, to, subject, content);
			message.BodyEncoding = Encoding.GetEncoding("GB2312");
			message.IsBodyHtml = true;

			client.Send(message);
		}
	}
}
