using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 招聘计划的扩展方法。
    /// </summary>
    public static class RecruitmentPlanExtensions
    {
        /// <summary>
        /// 获取已发布的招聘计划。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<RecruitmentPlan> Published(this IQueryable<RecruitmentPlan> source)
        {
            return source.Where(r => r.WhenPublished.HasValue);
        }

        /// <summary>
        /// 可报名的招聘计划。(已发布的，且不超过报名截止日期）
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<RecruitmentPlan> Enrollable(this IQueryable<RecruitmentPlan> source)
        {
            var now = DateTime.Now;
            return source.Where(r => r.WhenPublished.HasValue && r.EnrollExpirationDate > now);
        }
    }
}
