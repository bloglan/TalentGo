using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TalentGo.EntityFramework;

namespace DataProc
{
	public class EmailService : IService
	{
		TalentGoDbContext database = new TalentGoDbContext();
		string smsFormatString = "{0}，您好，您所填报的{1}报名资料，经初审通过，请于{2}前登陆网站声明是否参加考试，逾期未声明是否参加考试的将不准予参加考试。谢谢您的合作。\r\n"
			+ "尊敬的求职者，针对近期有人员反映和咨询有关云南省烟草公司曲靖市公司招聘考试培训班的事宜，现郑重声明：云南省烟草公司曲靖市公司一直秉承“公开、公平、公正”和“竞争择优”、“双向选择”的原则开展招聘，与社会考试培训机构无隶属、委托代理关系，任何针对云南省烟草公司曲靖市公司招聘考试的培训行为均与我单位无关，提醒求职者特此注意！";

		public EmailService()
		{

		}

		public void Run()
		{


			var enrollmentSet = (from enroll in this.database.EnrollmentData
								 where enroll.Approved.Value && !enroll.WhenAnnounced.HasValue
								 select enroll).ToList();



			for (int i = 0; i < enrollmentSet.Count; i++)
			{
				using (SmtpClient smtpClient = new SmtpClient("mail.qjyc.cn"))
				{
					smtpClient.UseDefaultCredentials = true;
					MailMessage mail = new MailMessage();
					mail.From = new MailAddress("job@qjyc.cn", "曲靖烟草招聘");
					mail.To.Add(new MailAddress(enrollmentSet[i].Users.Email, enrollmentSet[i].Name));
					mail.Subject = "曲靖烟草招聘报名审核通知";
					mail.Body = string.Format(smsFormatString, enrollmentSet[i].Name, enrollmentSet[i].RecruitmentPlan.Title, enrollmentSet[i].RecruitmentPlan.AnnounceExpirationDate.Value.ToLongDateString());
					smtpClient.Send(mail);
					Console.WriteLine(string.Format("[{1}/{2}]:已向{0}发送了邮件。", enrollmentSet[i].Name, i + 1, enrollmentSet.Count));

					Thread.Sleep(12500);
				}
			}
		}
	}
}
