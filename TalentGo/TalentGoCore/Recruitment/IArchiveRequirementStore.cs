using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    public interface IArchiveRequirementStore : IRecruitmentPlanStore, IDisposable
    {
        Task AddArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirement requirement);

        Task UpdateArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirement requirement);

        Task RemoveArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirement requirement);

        Task<IQueryable<ArchiveRequirement>> GetArchiveRequirementsAsync(RecruitmentPlan plan);
    }
}
