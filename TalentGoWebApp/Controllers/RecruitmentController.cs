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
                return HttpNotFound();

            return View(this.recruitmentManager.RecruitmentPlans.Enrollable());
        }

        public async Task<ActionResult> Detail(int id)
        {
            var current = await this.recruitmentManager.FindByIdAsync(id);
            if (!current.WhenPublished.HasValue)
                return HttpNotFound();

            return View(current);
        }



        //public async Task<ActionResult> UploadArchives()
        //{
        //    //返回报名需求项列表。
        //    //实际的报名资料项由子方法给出。
        //    if (!this.recruitmentContext.SelectedPlanId.HasValue)
        //        throw new NotSupportedException();

        //    var plan = await this.recruitManager.FindByIdAsync(this.recruitmentContext.SelectedPlanId.Value);
        //    var archReqSet = await this.recruitManager.GetArchiveRequirements(plan);
        //    return View(archReqSet);
        //}

        //[ChildActionOnly]
        //public async Task<ActionResult> ArchiveListOfEnrollment(ArchiveRequirement requirement)
        //{
        //    ApplicationForm enrollmentData = this.applicationFormManager.ApplicationForms.FirstOrDefault(e => e.PersonId == this.user.Id && e.JobId == this.recruitmentContext.SelectedPlanId.Value);

        //    var CurrentUserEnrollmentArchivesByRequired = from arch in await this.applicationFormManager.GetEnrollmentArchives(enrollmentData)
        //                                                  where arch.ArchiveCategoryID == requirement.ArchiveCategoryID
        //                                                  select arch;

        //    this.ViewData["user"] = this.user;
        //    return PartialView(CurrentUserEnrollmentArchivesByRequired);
        //}

        /// <summary>
        /// 浏览待提交的报名表，并执行提交。
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> CommitEnrollment(int id)
        {
            //
            var enrollment = await this.applicationFormManager.FindByIdAsync(id);
            if (enrollment == null)
                return HttpNotFound();

            return View(enrollment);
        }

        /// <summary>
        /// Post执行提交动作。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CommitEnrollment(int id, bool Agreement)
        {
            var enrollment = await this.applicationFormManager.FindByIdAsync(id);
            try
            {

                await this.applicationFormManager.CommitAsync(enrollment);
                //导航到报名已完成。
                OperationResult reslt = new OperationResult(ResultStatus.Success, "报名已成功提交。您的报名资料将等待初步审核。敬请时常留意本网站通知公告，及时了解您的报名审核结果。", this.Url.Action("Index"), 10);
                return View("OperationResult", reslt);
            }
            catch (CommitEnrollmentException ceex)
            {

                StringBuilder sb = new StringBuilder();
                foreach (string msg in ceex.ArchiveRequirementErrMsg)
                {
                    ModelState.AddModelError("", msg);
                }
                ModelState.AddModelError("", "请返回到“管理照片资料”按要求上传资料。");
                if (enrollment.WhenCommited.HasValue)
                    return View("OperationResult", new OperationResult(ResultStatus.Warning, "您的报名资料已提交，不能重复提交。您可以查看您所提交的报名资料。", this.Url.Action("PreviewEnrollment", new { id = enrollment.JobId }), 5));
                return View(enrollment);
            }
            catch (InvalidOperationException invalidOperationEx)
            {
                return View("OperationResult", new OperationResult(ResultStatus.Failure, invalidOperationEx.Message, this.Url.Action("Index"), 5));
            }
            catch (Exception ex)
            {
                return View("OperationResult", new OperationResult(ResultStatus.Failure, ex.Message, this.Url.Action("Index"), 5));
            }
        }

        /// <summary>
        /// 浏览报名资料。若没有，则跳转到操作警告。
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PreviewEnrollment(int? id)
        {
            if (!id.HasValue)
                return HttpNotFound();

            var plan = await this.recruitmentManager.FindByIdAsync(id.Value);
            var user = this.CurrentUser();

            var enrollment = this.applicationFormManager.ApplicationForms.First(e => e.PersonId == user.Id && e.JobId == plan.Id);
            return View(enrollment);

        }

        /// <summary>
        /// 声明是否参加考试。
        /// </summary>
        /// <param name="id">报名表</param>
        /// <returns></returns>
        public async Task<ActionResult> AnnounceForExam(int id)
        {
            var form = await this.applicationFormManager.FindByIdAsync(id);

            return View(form);
        }

        /// <summary>
        /// 提交声明是否参加考试。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AnnounceForExam(int id, bool IsTakeExam)
        {
            try
            {
                var form = await this.applicationFormManager.FindByIdAsync(id);
                await this.applicationFormManager.AnnounceForExamAsync(form, IsTakeExam);
            }
            catch (Exception ex)
            {
                return View("OperationResult", new OperationResult(ResultStatus.Failure, ex.Message, this.Url.Action("Index"), 3));
            }
            return View("OperationResult", new OperationResult(ResultStatus.Success, "操作成功。", this.Url.Action("Index"), 3));

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

        #region 帮助方法


        #endregion
    }
}