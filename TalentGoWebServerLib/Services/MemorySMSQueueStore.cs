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
    public class MemorySMSQueueStore : ISMSQueueStore
    {
        Queue<TemplatedShortMessage> innerQueue;

        /// <summary>
        /// 
        /// </summary>
        public MemorySMSQueueStore()
        {
            this.innerQueue = new Queue<TemplatedShortMessage>();
        }

        /// <summary>
        /// 
        /// </summary>
        public int Length => this.innerQueue.Count;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task ClearAsync()
        {
            this.innerQueue.Clear();
            return Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TemplatedShortMessage> DequeueAsync()
        {
            return Task.FromResult(this.innerQueue.Dequeue());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task EnqueueAsync(TemplatedShortMessage item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.innerQueue.Enqueue(item);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public Task EnqueueRangeAsync(IEnumerable<TemplatedShortMessage> messages)
        {
            foreach(var item in messages)
            {
                this.innerQueue.Enqueue(item);
            }
            return Task.FromResult(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<TemplatedShortMessage> PeekAsync()
        {
            return Task.FromResult(this.innerQueue.Peek());
        }
    }
}
