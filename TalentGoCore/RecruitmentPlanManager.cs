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
        public IQueryable<RecruitmentPlan> AllRecruitmentPlans
        {
            get { return this.store.RecruitmentPlans; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<RecruitmentPlan> AvailableRecruitmentPlans
        {
            get
            {
                return this.AllRecruitmentPlans.Where(plan => plan.WhenPublished.HasValue && plan.ExpirationDate > DateTime.Now);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public IQueryable<RecruitmentPlan> GetPlansForUser(Person User)
        {
            return this.AllRecruitmentPlans.Where(plan => plan.IsPublic && plan.WhenPublished.HasValue);
        }

        /// <summary>
        /// 获取可用的、与用户相关联的招聘计划。
        /// </summary>
        /// <returns></returns>
        public IQueryable<RecruitmentPlan> GetAvariableRecruitPlan(Person user)
        {

            /////如果已存在报名资料，则返回报名资料对应的Plan
            /////
            //TargetUser currentuser = context.TargetUser;

            ////查找该用户的报名表
            //var enrollmentOfUserSet = from enrollment in this.database.EnrollmentData
            //                          where enrollment.UserID == currentuser.Id
            //                          select enrollment;

            //if (enrollmentOfUserSet.Any())
            //{
            //    List<RecruitmentPlan> plans = new List<RecruitmentPlan>();
            //    plans.Add(enrollmentOfUserSet.First().RecruitmentPlan);
            //    return plans.AsEnumerable();
            //}

            //招聘计划状态为Normal
            //招聘年度与DateTime.Now.Year
            //根据用户的RegisterationDelegate决定过滤是否公开的招聘计划
            //		如果IIdentity是ClaimsIdentity，说明为外部用户，选择公开招聘计划。
            //		如果IIdentity是WindowsIdentity，说明是内部用户，根据其SID从用户表中选取模拟用户（其子女）///
            //招聘计划处于ExpirationDate指定的有效期内。
            //


            //确定是否内外网的代码段，还需斟酌。
            //采用绑定目标用户的RegisterationDelegate将不依赖于IIdentity的类型，也就意味着其与网站具体的验证方式无关。
            //但是否会造成由于TargetUser设置不当的风险？需要在TargetUserManager中绑定目标用户时进行验证和保护。
            bool IsPublicFlag = true;


            int year = DateTime.Now.Year;
            var recruitmentPlanSet = from recruitment in this.store.RecruitmentPlans
                                     where recruitment.WhenPublished.HasValue &&
                                        recruitment.Year == DateTime.Now.Year &&
                                        recruitment.EnrollExpirationDate > DateTime.Now &&
                                        recruitment.IsPublic == IsPublicFlag
                                     orderby recruitment.WhenCreated descending
                                     select recruitment;

            return recruitmentPlanSet;
        }

        /// <summary>
        /// 创建招聘计划。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task CreateRecruitmentPlan(RecruitmentPlan plan)
        {
            //必须有明确的Publisher
            //设置WhenCreated为当前时间
            //设置State为Created
            //有效期晚于当前日期
            //报名截止日期晚于当前日期
            //其他必填字段验证
            //
            if (plan.ExpirationDate < DateTime.Now)
                throw new ArgumentException("招聘计划的有效期早于当前时间。");

            //if (plan.EnrollExpirationDate < DateTime.Now)
            //	throw new ArgumentException("报名截止日期早于当前时间。");

            //if (plan.EnrollExpirationDate > plan.ExpirationDate)
            //	throw new ArgumentException("报名的截止日期晚于招聘计划设定的有效期。");

            plan.Year = DateTime.Now.Year;
            plan.WhenCreated = DateTime.Now;
            //plan.State = RecruitmentPlanState.Created.ToString();

            await this.store.CreateAsync(plan);
        }

        /// <summary>
        /// 更新招聘计划。更新招聘不能改变招聘计划的状态。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task UpdateRecruitmentPlan(RecruitmentPlan plan)
        {
            //只有State处于Created的计划可以被修改。
            //可修改的字段包括Title, Recruitment, IsPublic, ExpirationDate, EnrollExpirationDate
            //
            RecruitmentPlan current = this.store.RecruitmentPlans.SingleOrDefault(e => e.Id == plan.Id && e.WhenPublished.HasValue);
            if (current == null)
                throw new ArgumentException("找不到计划或指定的计划不能修改");

            //检查有效期
            if (plan.ExpirationDate < DateTime.Now)
                throw new ArgumentException("招聘计划的有效期早于当前时间。");

            if (plan.EnrollExpirationDate > DateTime.Now)
                throw new ArgumentException("报名截止日期早于当前时间。");

            if (plan.EnrollExpirationDate > plan.ExpirationDate)
                throw new ArgumentException("报名的截止日期晚于招聘计划设定的有效期。");


            current.Title = plan.Title;
            current.Recruitment = plan.Recruitment;
            current.IsPublic = plan.IsPublic;
            current.ExpirationDate = plan.ExpirationDate;
            current.EnrollExpirationDate = plan.EnrollExpirationDate;

            await this.store.UpdateAsync(current);
        }

        /// <summary>
        /// 删除招聘计划。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task DeleteRecruitmentPlan(RecruitmentPlan plan)
        {
            var toRemove = await this.FindByIDAsync(plan.Id);
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
            RecruitmentPlan plan = await this.FindByIDAsync(PlanID);
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
        /// <param name="PlanID"></param>
        /// <returns></returns>
        public async Task<RecruitmentPlan> FindByIDAsync(int PlanID)
        {
            return await this.store.FindByIdAsync(PlanID);
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
