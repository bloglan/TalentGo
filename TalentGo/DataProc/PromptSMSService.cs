using System;
using System.Linq;
using TalentGo.EntityFramework;

namespace DataProc
{
    public class PromptSMSService : IService
	{
		TalentGoDbContext database = new TalentGoDbContext();
		string smsFormatString = "尊敬的求职者，针对近期有人员反映和咨询有关云南省烟草公司曲靖市公司招聘考试培训班的事宜，现郑重声明：云南省烟草公司曲靖市公司一直秉承“公开、公平、公正”和“竞争择优”、“双向选择”的原则开展招聘，与社会考试培训机构无隶属、委托代理关系，任何针对云南省烟草公司曲靖市公司招聘考试的培训行为均与我单位无关，提醒求职者特此注意！";

		public PromptSMSService()
		{

		}

		public void Run()
		{

			var NotAnnouncedSet = (from enrollment in this.database.EnrollmentData
								   where enrollment.Approved.Value
								   select enrollment).ToList();

			for (int i = 0; i < NotAnnouncedSet.Count; i++)
			{
				using (SMSSvc.SMSServiceClient smsClient = new SMSSvc.SMSServiceClient())
				{
					string smsMsg = string.Format(smsFormatString, NotAnnouncedSet[i].Name, NotAnnouncedSet[i].RecruitmentPlan.Title, NotAnnouncedSet[i].RecruitmentPlan.AnnounceExpirationDate.Value.ToLongDateString());

					smsClient.SendMessage(new string[] { NotAnnouncedSet[i].Mobile }, smsMsg, new SMSSvc.SendMessageOption());
					Console.WriteLine(string.Format("[{1}/{2}]:已向{0}发送了短信", NotAnnouncedSet[i].Name, i + 1, NotAnnouncedSet.Count));
				}
			}

		}
	}
}
