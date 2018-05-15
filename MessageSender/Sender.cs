using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentGo;
using TalentGo.EntityFramework;
using TalentGo.Services;

namespace MessageSender
{
    class Sender
    {
        ICandidateStore store;
        EaseIMTemplatedShortMessageService svc;
        string templateId = "3872897";
        //各位考生，你好！曲靖市烟草专卖局(公司)毕业生招聘理论考试定于2018年5月5日上午9一11点在云南师范大学商学院(昆明五华区商院路1号)慧宇楼举行，敬请各位考生及时登录招聘系统自行打印准考证。

        public Sender()
        {
            this.store = new CandidateStore(new TalentGoDbContext());
            this.svc = new EaseIMTemplatedShortMessageService(new MemorySMSQueueStore());
        }

        public async Task SendAsync()
        {

            

            try
            {
                await this.svc.SendAsync(new string[] { "15987452005" }, templateId, "测试", "2018年5月16日上午8:30", "曲靖市烟草公司办公楼508会议室");
                await this.svc.SendAsync(new string[] { "15188087877", "13887171990" }, templateId, "测试", "2018年5月16日上午8:30", "曲靖市烟草公司办公楼508会议室");

                var candidates = this.store.Candidates.Where(c => c.ExamId == 1003);
                var count = candidates.Count();
                foreach (var candidate in candidates)
                {
                    Console.Write("正在向{0}[{1}]发送短信...", candidate.Person.DisplayName, candidate.Person.Mobile);
                    await this.svc.SendAsync(new string[] { candidate.Person.Mobile }, templateId, candidate.Person.DisplayName, "2018年5月16日上午8:30", "曲靖市烟草公司办公楼508会议室");
                    Console.WriteLine("已提交！");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }


    }
}
