using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICandidateStore
    {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<Candidate> Candidates { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="examId"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        Task<Candidate> FindByIdAsync(int examId, Guid personId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task CreateAsync(Candidate item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task UpdateAsync(Candidate item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task DeleteAsync(Candidate item);
    }
}
