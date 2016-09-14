using System.Web;
using TalentGo.Core;

namespace TalentGo.Web
{
    public static class HttpContextExtension
    {
        public static RecruitmentContextBase GetRecruitmentContext(this HttpContextBase httpContext)
        {
            return new HttpRecruitmentContext(httpContext);
        }


        public static readonly string RecruitmentContextKey = "RecruitmentContextKey";
    }
}
