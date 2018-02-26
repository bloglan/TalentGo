using System;
using System.DirectoryServices;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using TalentGo;
using TalentGo.Web;
using TalentGoWebApp.Models;

namespace TalentGoWebApp.Controllers
{
    public class HomeController : Controller
	{

        EnrollmentManager enrollmentManager;
        RecruitmentContextBase recruitmentContext;

        public HomeController(EnrollmentManager enrollmentManager)
        {
            this.enrollmentManager = enrollmentManager;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.recruitmentContext = this.HttpContext.GetRecruitmentContext();
        }

        public ActionResult Index()
		{
			if (this.User.Identity is WindowsIdentity)
			{
				if (!this.User.IsInRole("QJYC\\招聘登记员")
					&& !this.User.IsInRole("QJYC\\招聘管理员")
					&& !this.User.IsInRole("QJYC\\招聘监督人")
					&& !this.User.IsInRole("InternetUser"))
					return View("AccessDeny");
			}

			return View();
		}

        public ActionResult About()
		{

			return View();
		}

		[ChildActionOnly]
		public ActionResult EnrollmentStatePartial()
		{
			Enrollment enrollment = this.enrollmentManager.Enrollments.FirstOrDefault(e => e.UserID == this.recruitmentContext.TargetUserId.Value);
			return PartialView("EnrollmentStatePartial", enrollment);
		}

		[ChildActionOnly]
        public async Task<ActionResult> LoginPartial()
        {
            LoginPartialViewModel model = new LoginPartialViewModel();

            if (!this.User.Identity.IsAuthenticated)
            {
                return PartialView("_LoginPartial", model);
            }


            model.identity = this.User.Identity;

            var winidentity = this.User.Identity as WindowsIdentity;
            if (winidentity == null)
            {

                //
                var user = await this.targetUserManager.FindByIdAsync(this.recruitmentContext.TargetUserId.Value);
                model.DisplayName = user.DisplayName;
                model.TargetUser = user;

            }
            else
            {
                //是Windows登陆
                DirectorySearcher searcher = new DirectorySearcher();
                searcher.Filter = "(objectSid=" + winidentity.User.ToString() + ")";
                SearchResult result = searcher.FindOne();
                DirectoryEntry entry = result.GetDirectoryEntry();
                if (entry.Properties["displayName"].Value != null)
                {
                    model.DisplayName = entry.Properties["displayName"].Value.ToString();
                }
                else
                    model.DisplayName = winidentity.Name;

                if (this.recruitmentContext.TargetUserId.HasValue)
                    model.TargetUser = await this.targetUserManager.FindByIdAsync(this.recruitmentContext.TargetUserId.Value);
            }

            return PartialView("_LoginPartial", model);
        }

		[ChildActionOnly]
		public async Task<ActionResult> HomeLoginPartial()
		{
			LoginPartialViewModel model = new LoginPartialViewModel();

			if (!this.User.Identity.IsAuthenticated)
			{
				return PartialView("BeforeAuthPartial");
			}
			else
			{
				model.identity = this.User.Identity;

				var winidentity = this.User.Identity as WindowsIdentity;
				if (winidentity == null)
				{
					//不是Windows登陆
					var claimsidentity = this.User.Identity as ClaimsIdentity;
					if (claimsidentity == null)
						throw new InvalidOperationException("操作错误，不支持的Identity类型。");

                    //
                    var user = await this.targetUserManager.FindByIdAsync(this.recruitmentContext.TargetUserId.Value);
					model.DisplayName = user.DisplayName;
					model.TargetUser = user;

				}
				else
				{
					//是Windows登陆
					DirectorySearcher searcher = new DirectorySearcher();
					searcher.Filter = "(objectSid=" + winidentity.User.ToString() + ")";
					SearchResult result = searcher.FindOne();
					DirectoryEntry entry = result.GetDirectoryEntry();
					if (entry.Properties["displayName"].Value != null)
					{
						model.DisplayName = entry.Properties["displayName"].Value.ToString();
					}
					else
						model.DisplayName = winidentity.Name;

					if (this.recruitmentContext.TargetUserId.HasValue)
						model.TargetUser = await this.targetUserManager.FindByIdAsync(this.recruitmentContext.TargetUserId.Value);
				}

				if (this.User.Identity is WindowsIdentity)
				{
					return PartialView("WindowsAuthPartial", model);
				}
				else
					return PartialView("FormsAuthPartial", model);
			}
		}

    }
}