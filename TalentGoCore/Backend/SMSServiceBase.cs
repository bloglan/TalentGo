using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Backend
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SMSServiceBase
    {
        ISMSMessageStore store;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        public SMSServiceBase(ISMSMessageStore store)
        {
            this.store = store;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract Task QueueForSendAsync(SMSMessage message);

        Queue<SMSMessageBag> ToBeSentQueue
        {
            get
            {
                return new Queue<SMSMessageBag>(this.store.SMSMessages.Where(sms => !sms.SentTime.HasValue).AsEnumerable());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract Task SendAsync(SMSMessage message);
    }
}
