using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Backend
{
    public abstract class SMSServiceBase
    {
        ISMSMessageStore store;
        public SMSServiceBase(ISMSMessageStore store)
        {

        }
        public abstract Task QueueForSendAsync(SMSMessage message);

        Queue<SMSMessageBag> ToBeSentQueue
        {
            get
            {
                return new Queue<SMSMessageBag>(this.store.SMSMessages.Where(sms => !sms.SentTime.HasValue).AsEnumerable());
            }
        }



        public abstract Task SendAsync(SMSMessage message);
    }
}
