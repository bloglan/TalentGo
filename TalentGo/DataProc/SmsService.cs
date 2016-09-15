using System;
using System.Linq;
using TalentGo.Utilities;

namespace DataProc
{
    public class SmsService : IService
	{
		TalentGoDbContext database = new TalentGoDbContext();
		string smsFormatString = "{0}，您好，您所填报的{1}报名资料，经初审通过，请于{2}前登陆网站声明是否参加考试，逾期未声明是否参加考试的将不准予参加考试。谢谢您的合作。";

		public SmsService()
		{

		}

		public void Run()
		{
			using (SMSSvc.SMSServiceClient smsClient = new SMSSvc.SMSServiceClient())
			{
				var NotAnnouncedSet = (from enrollment in this.database.EnrollmentData
									  where enrollment.Approved.Value && !enrollment.WhenAnnounced.HasValue
									  select enrollment).ToList();

				for (int i = 0; i < NotAnnouncedSet.Count; i++)
				{
					string smsMsg = string.Format(smsFormatString, NotAnnouncedSet[i].Name, NotAnnouncedSet[i].RecruitmentPlan.Title, NotAnnouncedSet[i].RecruitmentPlan.AnnounceExpirationDate.Value.ToLongDateString());

					smsClient.SendMessage(new string[] { NotAnnouncedSet[i].Mobile }, smsMsg, new SMSSvc.SendMessageOption());
					Console.WriteLine(string.Format("[{1}/{2}]:已向{0}发送了短信。发送进度。", NotAnnouncedSet[i].Name, i + 1, NotAnnouncedSet.Count));
				}
			}
				
		}
	}
}
