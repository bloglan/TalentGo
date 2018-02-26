using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRecruitmentPlanStore : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<RecruitmentPlan> RecruitmentPlans { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<RecruitmentPlan> FindByIdAsync(int Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Plan"></param>
        /// <returns></returns>
        Task CreateAsync(RecruitmentPlan Plan);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Plan"></param>
        /// <returns></returns>
        Task UpdateAsync(RecruitmentPlan Plan);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Plan"></param>
        /// <returns></returns>
        Task DeleteAsync(RecruitmentPlan Plan);
    }
}
