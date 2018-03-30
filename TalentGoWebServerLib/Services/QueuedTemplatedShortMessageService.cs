using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace TalentGo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class QueuedTemplatedShortMessageService : ITemplatedShortMessageService
    {
        ISMSQueueStore store;
        Task sendingProcess;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        public QueuedTemplatedShortMessageService(ISMSQueueStore store)
        {
            this.store = store;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="templateId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task SendAsync(string[] to, string templateId, params object[] args)
        {
            await this.store.EnqueueAsync(new TemplatedShortMessage
            {
                To = to,
                TemplateId = templateId,
                Args = args,
            });

            //通知后台启动发送过程（如果未启动的话）。
            if (this.sendingProcess == null)
            {
                this.sendingProcess = Task.Run(BeginSendProcess).ContinueWith((task) =>
                {
                    this.sendingProcess = null;
                });
                
            }
        }

        async Task BeginSendProcess()
        {
            var item = await this.store.PeekAsync();
        }
    }
}
