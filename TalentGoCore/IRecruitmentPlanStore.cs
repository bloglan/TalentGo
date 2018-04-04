using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRecruitmentPlanStore
    {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<RecruitmentPlan> Plans { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RecruitmentPlan> FindByIdAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        Task CreateAsync(RecruitmentPlan plan);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        Task UpdateAsync(RecruitmentPlan plan);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        Task DeleteAsync(RecruitmentPlan plan);
    }
}
