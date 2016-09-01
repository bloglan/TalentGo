using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Identity
{
	class SmsService : IIdentityMessageService
	{
		SMSSvc.SMSServiceClient smsClient = new SMSSvc.SMSServiceClient();
		public Task SendAsync(IdentityMessage message)
		{
			smsClient.SendMessage(new string[] { message.Destination }, message.Body, null);
			return Task.FromResult(0);
		}
	}
}
