using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace TalentGo.Services
{
    /// <summary>
    /// 通知服务。
    /// </summary>
    public class NotificationService : IPersonNotificationService, IApplicationFormNotificationService, IExaminationNotificationService, IRecruitmentPlanNotificationService
    {
        ITemplatedShortMessageService smsService;
        IEmailService emailService;

        /// <summary>
        /// 使用模板短信服务和邮件服务初始化通知服务。
        /// </summary>
        /// <param name="smsService"></param>
        /// <param name="emailService"></param>
        public NotificationService(ITemplatedShortMessageService smsService, IEmailService emailService)
        {
            this.smsService = smsService;
            this.emailService = emailService;
        }

        const string ticketReleasedMessageTemplateId = "3892587";

        /// <summary>
        /// 通知准考证发放。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task NotifyAdmissionTicketReleasedAsync(ExaminationPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            foreach(var candidate in plan.Candidates.Where(c => c.Attendance.HasValue && c.Attendance.Value == true))
            {
                await this.smsService.SendAsync(new string[]{ candidate.Person.Mobile }, ticketReleasedMessageTemplateId, candidate.Person.DisplayName);
            }
        }

        const string completeAuditMessageTemplateId = "3872644";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task NotifyAuditCompleteAsync(RecruitmentPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            foreach (var job in plan.Jobs)
            {
                foreach(var form in job.ApplicationForms)
                {
                    await this.smsService.SendAsync(new string[] { form.Person.Mobile }, completeAuditMessageTemplateId, form.Person.DisplayName, form.Job.Plan.Title, (form.AuditFlag ? "已通过" : "未通过"));
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public Task NotifyFileReviewStateAsync(ApplicationForm form)
        {
            throw new NotImplementedException();
        }

        const string examinationPlanPublisedMessageTemplateId = "4102617";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task NotifyPlanPublishedAsync(ExaminationPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            foreach (var candidate in plan.Candidates)
            {
                await this.smsService.SendAsync(new string[] { candidate.Person.Mobile }, examinationPlanPublisedMessageTemplateId, candidate.Person.DisplayName, "近期", candidate.Plan.Address);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public Task NotifyRealIdValidationStateAsync(Person person)
        {
            throw new NotImplementedException();
        }
    }
}
