using System.Web;

namespace TalentGo.Web
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpContextExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static RecruitmentContextBase GetRecruitmentContext(this HttpContextBase httpContext)
        {
            return new HttpRecruitmentContext(httpContext);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly string RecruitmentContextKey = "RecruitmentContextKey";
    }
}
