using System;
using System.DirectoryServices;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;
using TalentGo.EntityFramework;
using TalentGo.Identity;
using TalentGo.Recruitment;
using TalentGoWebApp.Models;

namespace TalentGoWebApp.Controllers
{
	public class HomeController : Controller
	{

        TargetUserManager targetUserManager;
		TalentGoDbContext database;

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

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.targetUserManager = new TargetUserManager(requestContext.HttpContext);
			this.database = TalentGoDbContext.FromContext(requestContext.HttpContext);
        }

        public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";
            ViewBag.aaaa = "afdasf";

			return View();
		}

		[ChildActionOnly]
		public ActionResult EnrollmentStatePartial()
		{
			EnrollmentData enrollment = this.database.EnrollmentData.FirstOrDefault(e => e.UserID == this.targetUserManager.TargetUser.Id);
			return PartialView("EnrollmentStatePartial", enrollment);
		}

		[ChildActionOnly]
        public ActionResult LoginPartial()
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
                //不是Windows登陆
                var claimsidentity = this.User.Identity as ClaimsIdentity;
                if (claimsidentity == null)
                    throw new InvalidOperationException("操作错误，不支持的Identity类型。");

                //
                model.DisplayName = this.targetUserManager.TargetUser.DisplayName;
                model.TargetUser = this.targetUserManager.TargetUser;

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

                if (this.targetUserManager.IsAssignedTargetUser)
                    model.TargetUser = this.targetUserManager.TargetUser;
            }

            return PartialView("_LoginPartial", model);
        }

		[ChildActionOnly]
		public ActionResult HomeLoginPartial()
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
					model.DisplayName = this.targetUserManager.TargetUser.DisplayName;
					model.TargetUser = this.targetUserManager.TargetUser;

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

					if (this.targetUserManager.IsAssignedTargetUser)
						model.TargetUser = this.targetUserManager.TargetUser;
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