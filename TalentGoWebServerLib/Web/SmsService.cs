using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace TalentGo.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class SmsService : IIdentityMessageService
	{
		SMSSvc.SMSServiceClient smsClient = new SMSSvc.SMSServiceClient();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendAsync(IdentityMessage message)
		{
			smsClient.SendMessage(new string[] { message.Destination }, message.Body, null);
			return Task.FromResult(0);
		}
	}
}
