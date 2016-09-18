using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    /// <summary>
    /// An interface to support read/write examination plan data from database.
    /// </summary>
    public interface IExaminationPlanStore : IDisposable
    {
        IQueryable<ExaminationPlan> ExaminationPlans { get; }

        Task<ExaminationPlan> FindByIdAsync(int Id);

        Task CreateAsync(ExaminationPlan plan);

        Task UpdateAsync(ExaminationPlan plan);

        Task DeleteAsync(ExaminationPlan plan);
    }
}
