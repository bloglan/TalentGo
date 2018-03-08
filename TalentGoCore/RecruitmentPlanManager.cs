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
        IRecruitmentPlanStore store;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Store"></param>
        public RecruitmentPlanManager(IRecruitmentPlanStore Store)
        {
            this.store = Store;
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<RecruitmentPlan> RecruitmentPlans
        {
            get { return this.store.RecruitmentPlans; }
        }

        /// <summary>
        /// 创建招聘计划。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task CreateAsync(RecruitmentPlan plan)
        {
            //必须有明确的Publisher
            //设置WhenCreated为当前时间
            //设置State为Created
            //有效期晚于当前日期
            //报名截止日期晚于当前日期
            //其他必填字段验证
            //

            //if (plan.EnrollExpirationDate < DateTime.Now)
            //	throw new ArgumentException("报名截止日期早于当前时间。");

            //if (plan.EnrollExpirationDate > plan.ExpirationDate)
            //	throw new ArgumentException("报名的截止日期晚于招聘计划设定的有效期。");

            //plan.State = RecruitmentPlanState.Created.ToString();

            await this.store.CreateAsync(plan);
        }

        /// <summary>
        /// 更新招聘计划。更新招聘不能改变招聘计划的状态。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task UpdateAsync(RecruitmentPlan plan)
        {
            //只有State处于Created的计划可以被修改。
            //可修改的字段包括Title, Recruitment, IsPublic, ExpirationDate, EnrollExpirationDate
            //

            if (plan.EnrollExpirationDate < DateTime.Now)
                throw new ArgumentException("报名截止日期早于当前时间。");

            await this.store.UpdateAsync(plan);
        }

        /// <summary>
        /// 删除招聘计划。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task DeleteRecruitmentPlan(RecruitmentPlan plan)
        {
            var toRemove = await this.FindByIdAsync(plan.Id);
            if (plan.WhenPublished.HasValue)
                throw new InvalidOperationException("已发布的招聘计划无法删除。");

            await this.store.DeleteAsync(plan);
        }

        /// <summary>
        /// 发布招聘计划。招聘计划发布后，将可被所有符合条件的用户看到。
        /// </summary>
        /// <returns></returns>
        public async Task PublishRecruitmentPlan(int PlanID, DateTime EnrollExpirationDate)
        {
            //当State处于Created时，将其设置为Normal.
            //当State处于Normal时，不做任何操作。
            //
            RecruitmentPlan plan = await this.FindByIdAsync(PlanID);
            if (plan == null)
                return;

            if (!plan.WhenPublished.HasValue)
            {
                //plan.State = RecruitmentPlanState.Normal.ToString();
                plan.WhenPublished = DateTime.Now;

                //所指定的截止日期当日全天都算作有效。
                plan.EnrollExpirationDate = EnrollExpirationDate;
            }

            await this.store.UpdateAsync(plan);
        }

        /// <summary>
        /// 根据指定的ID查找招聘计划。若未找到指定的招聘计划，则返回默认值null。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RecruitmentPlan> FindByIdAsync(int id)
        {
            return await this.store.FindByIdAsync(id);
        }

        /// <summary>
        /// 完成审核。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task CompleteAudit(RecruitmentPlan plan)
        {
            if (plan == null)
                throw new ArgumentNullException(nameof(plan));

            if (DateTime.Now < plan.EnrollExpirationDate)
                throw new ArgumentException("审核的提交早于报名截止日期。");

            if (plan.WhenAuditCommited.HasValue)
                return;

            var forms = new HashSet<ApplicationForm>();
            foreach(var job in plan.Jobs)
            {
                forms.UnionWith(job.ApplicationForms);
            }
            if (forms.Any(e => !e.Approved.HasValue))
                throw new InvalidOperationException("操作失败，还有未设置审核标记的报名表。");

            

            using (TransactionScope transScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (ApplicationForm data in forms)
                {
                    //若没有提交，或提交日期晚于报名截止日期的，直接设定为不通过。
                    if (!data.WhenCommited.HasValue || data.WhenCommited > plan.EnrollExpirationDate)
                    {
                        data.Approved = false;
                        data.WhenAudit = DateTime.Now;
                        data.AuditMessage = "未在指定的报名截止时间内提交";
                    }
                }


                //每项提交完成后，修改currentPlan的标记，表示已提交审核。
                plan.CompleteAudit();

                //设定考试时间和地点
                //currentplan.ExamStartTime = ExamStartTime;
                //currentplan.ExamEndTime = ExamEndTime;
                //currentplan.ExamLocation = ExamLocation;

                await this.store.UpdateAsync(plan);

                transScope.Complete();
            }

            //开始每项提交并发送短信。

        }
    }
}
