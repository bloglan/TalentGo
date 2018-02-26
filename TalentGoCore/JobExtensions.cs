using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// extension for queryable job collection.
    /// </summary>
    public static class JobExtensions
    {
        /// <summary>
        /// 获取可报名的职位。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<Job> EnrollableJobs(this IQueryable<Job> source)
        {
            var now = DateTime.Now;
            //计划已发布，且处于报名有效期内。
            return source.Where(j => j.Plan.WhenPublished.HasValue && j.Plan.EnrollExpirationDate > now);
        }
    }
}
