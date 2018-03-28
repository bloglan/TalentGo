using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// An interface to support read/write examination plan data from database.
    /// </summary>
    public interface IExaminationPlanStore
    {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<ExaminationPlan> Plans { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ExaminationPlan> FindByIdAsync(int Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task CreateAsync(ExaminationPlan item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task UpdateAsync(ExaminationPlan item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task DeleteAsync(ExaminationPlan item);
    }
}
