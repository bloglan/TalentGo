using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISMSQueueStore
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<TemplatedShortMessage> DequeueAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<TemplatedShortMessage> PeekAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task EnqueueAsync(TemplatedShortMessage item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        Task EnqueueRangeAsync(IEnumerable<TemplatedShortMessage> messages);

        /// <summary>
        /// 
        /// </summary>
        int Length { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task ClearAsync();

    }
}
