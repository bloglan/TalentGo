using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace TalentGo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class EaseIMTemplatedShortMessageService : ITemplatedShortMessageService
    {
        ISMSQueueStore store;
        Task sendingProcess;
        JsonSerializer serializer;
        object syncObj = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        public EaseIMTemplatedShortMessageService(ISMSQueueStore store)
        {
            this.store = store;
            this.serializer = new JsonSerializer();
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

            lock(syncObj)
            {
                //通知后台启动发送过程（如果未启动的话）。
                if (this.sendingProcess == null)
                {
                    this.sendingProcess = Task.Run(BeginSendProcess).ContinueWith((task) =>
                    {
                        this.sendingProcess = null;
                    });
                }
            }
        }


        async Task BeginSendProcess()
        {
            while(this.store.Length > 0)
            {
                var item = await this.store.DequeueAsync();

                var nonce = this.GenerateNonce();
                var ticks = this.GetTicks();
                var request = WebRequest.Create(SendSMSUrl);
                request.Method = "POST";
                //构造请求头。
                request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                request.Headers.Add("AppKey", AppKey);
                request.Headers.Add("Nonce", nonce);
                request.Headers.Add("CurTime", ticks.ToString());
                request.Headers.Add("CheckSum", this.CalculateCheckSum(AppSecret, nonce, ticks));

                //构造请求消息体。
                StringBuilder sb = new StringBuilder();
                sb.Append("templateid=" + item.TemplateId);
                sb.Append("&mobiles=" + JsonConvert.SerializeObject(item.To));
                sb.Append("&params=" + JsonConvert.SerializeObject(item.Args));
                Trace.TraceInformation("HTTP Body:{0}", sb.ToString());

                using (var writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(sb);
                    writer.Flush();
                }

                var response = request.GetResponse();
                var remoteResult = this.serializer.Deserialize<dynamic>(new JsonTextReader(new StreamReader(response.GetResponseStream())));

                if ((int)remoteResult.code != 200)
                {
                    //发送出现问题
                    Trace.TraceError("服务方报告操作失败，代码" + ((int)remoteResult.code).ToString());
                    //throw new Exception();
                }
            }
        }

        const string AppKey = "dfd775087ba7da8bb49200fa5d8e6a9e";
        const string AppSecret = "88b4ade07559";
        const string SendSMSUrl = "https://api.netease.im/sms/sendtemplate.action";

        long GetTicks()
        {
            var baseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var duration = DateTime.UtcNow - baseTime;
            return Convert.ToInt64(duration.TotalSeconds);
        }

        string GenerateNonce()
        {
            RandomNumberGenerator generator = RandomNumberGenerator.Create();
            var keydata = new byte[64];
            generator.GetBytes(keydata);
            return ToHexString(keydata, false);
        }

        string CalculateCheckSum(string AppSecret, string Nonce, long TimeTicks)
        {
            SHA1 sha1 = SHA1.Create();
            var plainContent = AppSecret + Nonce + TimeTicks.ToString();
            var contentBytes = Encoding.UTF8.GetBytes(plainContent);
            var hashBytes = sha1.ComputeHash(contentBytes);
            return ToHexString(hashBytes, false);
        }

        string ToHexString(byte[] Data, bool UpperCase)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var b in Data)
            {
                if (UpperCase)
                    sb.Append(b.ToString("X2"));
                else
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
