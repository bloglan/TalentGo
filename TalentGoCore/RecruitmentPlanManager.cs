using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace TalentGo
{
    /// <summary>
    /// 招聘计划管理器
    /// </summary>
    public class RecruitmentPlanManager
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Store"></param>
        public RecruitmentPlanManager(IRecruitmentPlanStore Store)
        {
            this.Store = Store;
        }

        /// <summary>
        /// Gets or sets store
        /// </summary>
        protected IRecruitmentPlanStore Store { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<RecruitmentPlan> Plans => this.Store.Plans;

        /// <summary>
        /// Gets or sets notification service for recruitment plan management.
        /// </summary>
        public IRecruitmentPlanNotificationService NotificationService { get; set; }

        /// <summary>
        /// 创建招聘计划。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task CreateAsync(RecruitmentPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            await this.Store.CreateAsync(plan);
        }

        /// <summary>
        /// 更新招聘计划。更新招聘不能改变招聘计划的状态。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task UpdateAsync(RecruitmentPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }
            //只有State处于Created的计划可以被修改。
            //可修改的字段包括Title, Recruitment, IsPublic, ExpirationDate, EnrollExpirationDate
            //

            if (plan.EnrollExpirationDate < DateTime.Now)
                throw new ArgumentException("报名截止日期早于当前时间。");

            await this.Store.UpdateAsync(plan);
        }

        /// <summary>
        /// 删除招聘计划。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task DeleteAsync(RecruitmentPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            var toRemove = await this.FindByIdAsync(plan.Id);
            if (plan.WhenPublished.HasValue)
                throw new InvalidOperationException("已发布的招聘计划无法删除。");

            await this.Store.DeleteAsync(plan);
        }

        /// <summary>
        /// 发布招聘计划。招聘计划发布后，将可被所有符合条件的用户看到。
        /// </summary>
        /// <returns></returns>
        public async Task PublishAsync(RecruitmentPlan plan, DateTime enrollExpirationTime)
        {
            if (plan == null)
                throw new ArgumentNullException(nameof(plan));
            //当State处于Created时，将其设置为Normal.
            //当State处于Normal时，不做任何操作。
            //
            if (plan.WhenPublished.HasValue)
                throw new InvalidOperationException("招聘计划已发布");

            if (enrollExpirationTime <= DateTime.Now)
                throw new ArgumentException("报名截止时间早于当前时间。");

            plan.WhenPublished = DateTime.Now;
            //所指定的截止日期当日全天都算作有效。
            plan.EnrollExpirationDate = enrollExpirationTime;

            await this.Store.UpdateAsync(plan);
        }

        /// <summary>
        /// 根据指定的ID查找招聘计划。若未找到指定的招聘计划，则返回默认值null。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RecruitmentPlan> FindByIdAsync(int id)
        {
            return await this.Store.FindByIdAsync(id);
        }

        /// <summary>
        /// 完成审核。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task CompleteAuditAsync(RecruitmentPlan plan)
        {
            if (plan == null)
                throw new ArgumentNullException(nameof(plan));

            if (DateTime.Now < plan.EnrollExpirationDate)
                throw new ArgumentException("审核的提交早于报名截止日期。");

            if (plan.WhenAuditCommited.HasValue)
                return;

            using (TransactionScope transScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var job in plan.Jobs)
                {
                    foreach (var form in job.ApplicationForms)
                    {
                        if (form.FileReviewAccepted.HasValue && !form.FileReviewAccepted.Value)
                        {
                            form.AuditFlag = false;
                            form.AuditMessage = "资料审查未通过";
                            form.WhenAudit = DateTime.Now;
                            form.AuditBy = "自动资格审核程序";
                        }

                        //设置审核完成时间，以指示审核已完成。
                        form.WhenAuditComplete = DateTime.Now;
                    }
                }

                //每项提交完成后，修改currentPlan的标记，表示已提交审核。
                plan.CompleteAudit();

                await this.Store.UpdateAsync(plan);

                transScope.Complete();
            }

            //开始每项提交并发送短信。
            if (this.NotificationService != null)
                await this.NotificationService.NotifyAuditCompleteAsync(plan);
        }
    }
}
