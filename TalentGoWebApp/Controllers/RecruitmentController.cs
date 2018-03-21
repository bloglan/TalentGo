using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using TalentGo.Web;
using TalentGo.Identity;
using TalentGoWebApp.Models;
using TalentGo;
using TalentGo.Models;

namespace TalentGoWebApp.Controllers
{
    [Authorize]
    public class RecruitmentController : Controller
    {
        RecruitmentPlanManager recruitmentManager;
        ApplicationFormManager applicationFormManager;
        ApplicationUserManager targetUserManager;
        IJobStore jobStore;

        public RecruitmentController(RecruitmentPlanManager recruitmentPlanManager, ApplicationFormManager enrollmentManager, ApplicationUserManager userManager, IJobStore jobStore)
        {
            this.recruitmentManager = recruitmentPlanManager;
            this.applicationFormManager = enrollmentManager;
            this.targetUserManager = userManager;
            this.jobStore = jobStore;
        }

        /// <summary>
        /// 显示招聘首页，招聘首页应显示可用的报名计划以及相关操作按钮。
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var person = this.CurrentUser();
            if (!person.RealIdValid.HasValue || !person.RealIdValid.Value)
                return View("RealIdRequired");

            return View(this.recruitmentManager.RecruitmentPlans.Enrollable());
        }
    }
}