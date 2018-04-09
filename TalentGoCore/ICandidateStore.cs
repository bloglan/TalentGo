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
        /// <param name="personId"></param>
        /// <param name="examId"></param>
        /// <returns></returns>
        Task<Candidate> FindByIdAsync(Guid personId, int examId);

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
