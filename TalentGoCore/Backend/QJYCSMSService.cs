using System;
using System.Threading.Tasks;

namespace TalentGo.Backend
{
    /// <summary>
    /// QJYC SMS Service
    /// </summary>
    public class QJYCSMSService : SMSServiceBase
    {
        /// <summary>
        /// Initialize SMS Service using SMSMessage Store.
        /// </summary>
        /// <param name="Store"></param>
        public QJYCSMSService(ISMSMessageStore Store)
            : base(Store)
        { }

        /// <summary>
        /// Queue a message for send.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Task QueueForSendAsync(SMSMessage message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Task SendAsync(SMSMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
