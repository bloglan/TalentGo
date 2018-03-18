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

        public async Task<ActionResult> Detail(int id)
        {
            var current = await this.recruitmentManager.FindByIdAsync(id);
            if (!current.WhenPublished.HasValue)
                return HttpNotFound();

            return View(current);
        }

        #region 分部视图方法
        /// <summary>
        /// 根据RecruitPlan，ApplicationUser来加载操作面板分部视图
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult RecruitmentPanel(RecruitmentPlan plan)
        {
            RecruitmentPanelStateModel viewModel = new RecruitmentPanelStateModel();
            viewModel.Plan = plan;
            var user = this.CurrentUser();
            var enrollment = this.applicationFormManager.ApplicationForms.FirstOrDefault(e => e.PersonId == user.Id && e.JobId == plan.Id);
            if (enrollment != null)
            {
                viewModel.HasEnrollment = true;
                viewModel.Enrollment = enrollment;
            }


            return PartialView(viewModel);
        }

        #endregion

    }
}