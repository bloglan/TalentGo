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
        /// 
        /// </summary>
        public virtual IQueryable<ExaminationPlan> Plans => this.Store.ExaminationPlans;

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

            if (plan.AttendanceConfirmationExpiresAt < DateTime.Now)
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
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public virtual async Task Publish(ExaminationPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            if (plan.AttendanceConfirmationExpiresAt < DateTime.Now)
                throw new ArgumentException("确认参加考试的截止时间早于当前时间。");

            plan.WhenPublished = DateTime.Now;

            await this.Store.UpdateAsync(plan);
        }
    }
}
