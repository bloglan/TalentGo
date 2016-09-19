using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    public interface IRecruitmentPlanStore : IDisposable
    {
        IQueryable<RecruitmentPlan> RecruitmentPlans { get; }

        Task<RecruitmentPlan> FindByIdAsync(int Id);

        Task CreateAsync(RecruitmentPlan Plan);

        Task UpdateAsync(RecruitmentPlan Plan);

        Task DeleteAsync(RecruitmentPlan Plan);
    }
}
