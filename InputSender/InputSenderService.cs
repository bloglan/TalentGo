using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputSender
{
    class InputSenderService
    {
        Queue<char> q;
        Task sendTask;

        public InputSenderService()
        {
            this.q = new Queue<char>();
        }

        public Task SendAsync(char input)
        {
            this.q.Enqueue(input);
            if (this.sendTask == null)
            {
                Trace.TraceInformation("没有任务，尝试启动任务。");
                this.sendTask = Task.Run(SendProc).ContinueWith((task) =>
                {
                    this.sendTask = null;
                    Trace.TraceInformation("任务完成，移除引用。");

                });
            }
            return Task.FromResult(0);
        }

        async Task SendProc()
        {
            while (this.q.Count > 0)
            {
                await Task.Delay(1000);
                Console.Write(this.q.Dequeue());
                Trace.TraceInformation("队列里还有" + this.q.Count.ToString() + "个字符未发送。");
            }
        }
    }
}
