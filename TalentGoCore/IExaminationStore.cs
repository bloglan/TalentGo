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
    public interface IExaminationStore
    {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<Examination> Exams { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<Examination> FindByIdAsync(int Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        Task CreateAsync(Examination exam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        Task UpdateAsync(Examination exam);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        Task DeleteAsync(Examination exam);
    }
}
