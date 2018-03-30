using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TalentGo.Services;

namespace QueuedSMSSenderServiceRunner
{
    class ServiceRunner
    {
        Timer timer;
        ISMSQueueStore store;

        public ServiceRunner(ISMSQueueStore store)
        {
            this.timer = new Timer(10000);
            this.timer.AutoReset = false;
            this.timer.Elapsed += Timer_Elapsed;
            this.store = store;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //调用发送例程并等待完毕
            var task = Task.Run(SendProc);
            task.Wait();
            //启用Timer
            this.timer.Start();
        }

        public void Start()
        {
            this.timer.Start();
        }

        async Task SendProc()
        {
            while(this.store.Length > 0)
            {
                var msg = await this.store.PeekAsync();
                
            }
        }
    }
}
