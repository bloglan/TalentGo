using Microsoft.AspNet.Identity;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TalentGo.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailService : IIdentityMessageService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendAsync(IdentityMessage message)
        {
            using (SmtpClient smtpClient = new SmtpClient("EXCH13SRV.qjyc.cn"))
            {
				smtpClient.UseDefaultCredentials = true;

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress("job@qjyc.cn", "曲靖烟草招聘")
                };
                mail.To.Add(new MailAddress(message.Destination));
                mail.Subject = message.Subject;
                mail.Body = message.Body;
                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
					//出错
					throw ex;
                    //Console.WriteLine("邮件发送失败："+ex.ToString());
                }
            }
            return Task.FromResult(0);
        }
    }
}
