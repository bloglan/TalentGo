﻿using System;
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

        /// <summary>
        /// 通知准考证发放。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public Task NotifyAdmissionTicketReleasedAsync(ExaminationPlan plan)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public Task NotifyAuditCompleteAsync(RecruitmentPlan plan)
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public Task NotifyPlanPublishedAsync(ExaminationPlan plan)
        {
            throw new NotImplementedException();
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