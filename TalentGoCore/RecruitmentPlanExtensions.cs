using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    public static class RecruitmentPlanExtensions
    {
        public static IQueryable<RecruitmentPlan> Published(this IQueryable<RecruitmentPlan> source)
        {
            return source.Where(r => r.WhenPublished.HasValue);
        }
    }
}
