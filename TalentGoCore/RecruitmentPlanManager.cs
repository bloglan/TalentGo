using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task<IQueryable<ArchiveRequirement>> GetArchiveRequirements(RecruitmentPlan plan)
        {
            var arStore = this.store as IArchiveRequirementStore;
            if (arStore == null)
                throw new NotSupportedException("不支持。请为传入的存储实现IArchiveRequirementStore接口。");

            return await arStore.GetArchiveRequirementsAsync(plan);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        public async Task AddArchiveRequirement(RecruitmentPlan plan, ArchiveRequirement requirement)
        {
            var arStore = this.store as IArchiveRequirementStore;
            if (arStore == null)
                throw new NotSupportedException("不支持。请为传入的存储实现IArchiveRequirementStore接口。");

            await arStore.AddArchiveRequirementAsync(plan, requirement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        public async Task UpdateArchiveRequirement(RecruitmentPlan plan, ArchiveRequirement requirement)
        {
            var arStore = this.store as IArchiveRequirementStore;
            if (arStore == null)
                throw new NotSupportedException("不支持。请为传入的存储实现IArchiveRequirementStore接口。");

            await arStore.UpdateArchiveRequirementAsync(plan, requirement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        public async Task RemoveArchiveRequirement(RecruitmentPlan plan, ArchiveRequirement requirement)
        {
            var arStore = this.store as IArchiveRequirementStore;
            if (arStore == null)
                throw new NotSupportedException("不支持。请为传入的存储实现IArchiveRequirementStore接口。");

            await arStore.RemoveArchiveRequirementAsync(plan, requirement);
        }
    }
}
