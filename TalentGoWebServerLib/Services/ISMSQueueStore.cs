using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Services
{
    public interface ISMSQueueStore
    {
        Task<QueuedSMSMessage> DequeueAsync();

        Task EnqueueAsync(QueuedSMSMessage item);

        Task EnqueueRangeAsync(IEnumerable<QueuedSMSMessage> messages);

        int Length { get; }

        Task ClearQueueAsync();
    }
}
