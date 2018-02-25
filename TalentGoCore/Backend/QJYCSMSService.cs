using System;
using System.Threading.Tasks;

namespace TalentGo.Backend
{
    public class QJYCSMSService : SMSServiceBase
    {
        public QJYCSMSService(ISMSMessageStore Store)
            : base(Store)
        { }

        public override Task QueueForSendAsync(SMSMessage message)
        {
            throw new NotImplementedException();
        }

        public override Task SendAsync(SMSMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
