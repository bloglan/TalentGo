using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 
    /// </summary>
    public class ExaminationPlanManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Store"></param>
        public ExaminationPlanManager(IExaminationPlanStore Store)
        {
            this.Store = Store;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual IExaminationPlanStore Store { get; set; }

        /// <summary>
        /// 通知服务。
        /// </summary>
        public virtual IExaminationNotificationService NotificationService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual IQueryable<ExaminationPlan> Plans => this.Store.Plans;

        /// <summary>
        /// Find plan by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExaminationPlan> FindByIdAsync(int id)
        {
            return await this.Store.FindByIdAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(ExaminationPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            if (plan.AttendanceConfirmationExpiresAt.HasValue && plan.AttendanceConfirmationExpiresAt.Value < DateTime.Now)
                throw new ArgumentException("确认参加考试的截止时间早于当前时间。");

            await this.Store.CreateAsync(plan);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(ExaminationPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }
            if (plan.WhenPublished.HasValue)
                throw new InvalidOperationException("Can not update plan if published.");

            plan.WhenChanged = DateTime.Now;

            await this.Store.UpdateAsync(plan);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(ExaminationPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }
            if (plan.WhenPublished.HasValue)
                throw new InvalidOperationException("Cannot delete plan if published.");

            await this.Store.DeleteAsync(plan);
        }

        /// <summary>
        /// 发布考试。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public virtual async Task PublishAsync(ExaminationPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            if (!plan.Candidates.Any())
                throw new InvalidOperationException("没有任何考试候选人。");

            if (plan.AttendanceConfirmationExpiresAt.HasValue)
            {
                if (plan.AttendanceConfirmationExpiresAt < DateTime.Now)
                throw new ArgumentException("确认参加考试的截止时间早于当前时间。");
            }
            else
            {
                //没有设定确认时间，则默认所有考试候选人都参加考试。
                foreach(var candidate in plan.Candidates)
                {
                    candidate.Attendance = true;
                }
            }

            plan.WhenPublished = DateTime.Now;

            await this.Store.UpdateAsync(plan);

            //调用通知服务。
            //如果没有设定确认截止时间，那么发布时不进行通知。
            if(plan.AttendanceConfirmationExpiresAt.HasValue)
            {
                if (this.NotificationService != null)
                {
                    await this.NotificationService.NotifyPlanPublishedAsync(plan);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public virtual async Task ReleaseAdmissionTicket(ExaminationPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }
            if (plan.Candidates.Any(c => c.Attendance.Value && string.IsNullOrEmpty(c.AdmissionNumber)))
                throw new InvalidOperationException("尚未对已确认参加考试的人员编制准考证。");

            plan.WhenAdmissionTicketReleased = DateTime.Now;
            await this.Store.UpdateAsync(plan);

            if (NotificationService != null)
            {
                await this.NotificationService.NotifyAdmissionTicketReleasedAsync(plan);
            }
        }
    }
}
