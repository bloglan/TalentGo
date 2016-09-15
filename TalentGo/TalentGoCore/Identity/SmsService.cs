using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    public class SmsService : IIdentityMessageService
	{
		SMSSvc.SMSServiceClient smsClient = new SMSSvc.SMSServiceClient();
		public Task SendAsync(IdentityMessage message)
		{
			smsClient.SendMessage(new string[] { message.Destination }, message.Body, null);
			return Task.FromResult(0);
		}
	}
}
