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
    public static class CandidateExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        public static IQueryable<Candidate> AvailableForUser(this IQueryable<Candidate> source, Guid personId)
        {
            return source.Where(c => c.Plan.WhenPublished.HasValue && c.PersonId == personId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<Candidate> AttendanceConfirmed(this IQueryable<Candidate> source)
        {
            return source.Where(c => c.Attendance.Value);
        }
    }
}
