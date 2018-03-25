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
    public static class ExamineeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="planId"></param>
        /// <returns></returns>
        public static IQueryable<Examinee> OfPlan(this IQueryable<Examinee> source, int planId)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(e => e.PlanId == planId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<Examinee> AttendanceConfirmed(this IQueryable<Examinee> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            return source.Where(e => e.AttendanceConfirmed.Value);
        }
    }
}
