using System.Security.Principal;
using System.Web;

namespace TalentGo.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpRecruitmentContext : RecruitmentContextBase
    {
        HttpContextBase httpContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="HttpContext"></param>
        public HttpRecruitmentContext(HttpContextBase HttpContext)
        {
            this.httpContext = HttpContext;
        }


        /// <summary>
        /// 
        /// </summary>
        public override IPrincipal LoginUser
        {
            get
            {
                return this.httpContext.User;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int? SelectedPlanId
        {
            get
            {
                if (this.httpContext.Session["SelectedPlanId"] != null)
                    return (int)this.httpContext.Session["SelectedPlanId"];
                return null;
            }

            set
            {
                if (value.HasValue)
                    this.httpContext.Session["SelectedPlanId"] = value.Value;
                else
                    this.httpContext.Session["SelectedPlanId"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int? TargetUserId
        {
            get
            {
                if (this.httpContext.Session["TargetUserId"] != null)
                    return (int)this.httpContext.Session["TargetUserId"];
                return null;
            }

            set
            {
                if (value.HasValue)
                    this.httpContext.Session["TargetUserId"] = value.Value;
                else
                    this.httpContext.Session["TargetUserId"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int? CurrentEnrollmentId
        {
            get
            {
                if (this.httpContext.Session["CurrentEnrollmentId"] != null)
                    return (int)this.httpContext.Session["CurrentEnrollmentId"];
                return null;
            }

            set
            {
                if (value.HasValue)
                    this.httpContext.Session["CurrentEnrollmentId"] = value.Value;
                else
                    this.httpContext.Session["CurrentEnrollmentId"] = null;
            }
        }
    }
}
