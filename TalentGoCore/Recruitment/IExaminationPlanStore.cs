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
        /// <summary>
        /// 
        /// </summary>
        IQueryable<ExaminationPlan> ExaminationPlans { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ExaminationPlan> FindByIdAsync(int Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        Task CreateAsync(ExaminationPlan plan);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        Task UpdateAsync(ExaminationPlan plan);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        Task DeleteAsync(ExaminationPlan plan);
    }
}
