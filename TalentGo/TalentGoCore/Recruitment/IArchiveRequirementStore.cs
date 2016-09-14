using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    public interface IArchiveRequirementStore : IRecruitmentPlanStore
    {
        Task AddArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirements requirement);

        Task UpdateArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirements requirement);

        Task RemoveArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirements requirement);

        Task<IQueryable<ArchiveRequirements>> GetArchiveRequirementsAsync(RecruitmentPlan plan);
    }
}
