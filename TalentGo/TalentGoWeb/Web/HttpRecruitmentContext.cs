using System.Security.Principal;
using System.Web;
using TalentGo.Recruitment;

namespace TalentGo.Web
{
    public class HttpRecruitmentContext : RecruitmentContextBase
    {
        HttpContextBase httpContext;

        public HttpRecruitmentContext(HttpContextBase HttpContext)
        {
            this.httpContext = HttpContext;
        }



        public override IPrincipal LoginUser
        {
            get
            {
                return this.httpContext.User;
            }
        }

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
