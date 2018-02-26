using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 
    /// </summary>
    public interface IArchiveRequirementStore : IRecruitmentPlanStore, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        Task AddArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirement requirement);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        Task UpdateArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirement requirement);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        Task RemoveArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirement requirement);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        Task<IQueryable<ArchiveRequirement>> GetArchiveRequirementsAsync(RecruitmentPlan plan);
    }
}
