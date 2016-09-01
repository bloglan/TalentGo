using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using TalentGo.EntityFramework;
using TalentGo.Identity;
using TalentGo.Recruitment;
using TalentGo.ViewModels;
using TalentGoWebApp.Models;

namespace TalentGoWebApp.Controllers
{
	[Authorize(Roles = "InternetUser,QJYC\\招聘登记员,QJYC\\招聘管理员")]
    public class RecruitmentController : Controller
    {
        TargetUserManager targetUserManager;
        RecruitmentManager recruitManager;
        EnrollmentManager enrollmentManager;
        ArchiveManager archiveManager;
		TalentGoDbContext database;

        public RecruitmentController()
        {
            this.database = new TalentGoDbContext();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            this.targetUserManager = new TargetUserManager(requestContext.HttpContext);
            this.recruitManager = new RecruitmentManager(requestContext.HttpContext);
            this.enrollmentManager = new EnrollmentManager(requestContext.HttpContext);
            this.archiveManager = new ArchiveManager(requestContext.HttpContext);
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    base.OnException(filterContext);
        //    //?

        //}

        /// <summary>
        /// 显示招聘首页，招聘首页应显示可用的报名计划以及相关操作按钮。
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            if (this.targetUserManager.IsAssignedTargetUser)
            {
                //ApplicationUser user = await this.targetUserManager.GetTargetUser();
                return View(await this.recruitManager.GetAvariableRecruitPlan());
            }

            //若没有已绑定用户，则转到绑定用户页面。
            IEnumerable<TargetUser> allAvriableTargetUsers = await this.targetUserManager.GetAvaiableTargetUsers();
            if (!allAvriableTargetUsers.Any())
            {
                return RedirectToAction("CreateUser", "TargetUser");
            }
            return RedirectToAction("AssignTargetUser", "TargetUser");
        }

        public async Task<ActionResult> Detail(int id)
        {
            var recruitmentPlanSet = await this.recruitManager.GetAvariableRecruitPlan();
            var current = recruitmentPlanSet.SingleOrDefault(e => e.id == id);
            if (current == null)
                return HttpNotFound();

            return View(current);
        }


        /// <summary>
        /// 报名（填写和编辑报名表）
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Enroll(int? id)
        {
            //如果有传入参数ID，则指示了要选中的招聘计划。
            if (id.HasValue)
            {
                try
                {
                    await this.recruitManager.SelectRecruitmentPlanByID(id.Value);
                }
                catch (Exception ex)
                {
                    return View("OperationResult", new OperationResult(ResultStatus.Failure, "找不到指定的招聘计划。", this.Url.Action("Index"), 3));
                }
            }

            if (!this.targetUserManager.IsAssignedTargetUser)
            {
                return RedirectToAction("AssignTargetUser", "TargetUser");
            }

            if (this.recruitManager.SelectedRecruitPlan == null)
            {
                return RedirectToAction("Index");
            }
            var GetEnrollmentTask = this.enrollmentManager.GetAvaiableOrDefault();

            //准备下拉框及相关数据
            this.InitModelSelectionData(this.ViewData);

            EnrollmentData data = await GetEnrollmentTask;
            if (data.WhenCommited.HasValue)
            {
                return RedirectToAction("PreviewEnrollment");
                //return View("OperationResult", new OperationResult(ResultStatus.Warning, "您的报名资料已提交，不能重复提交。您可以查看您所提交的报名资料。", this.Url.Action("PreviewEnrollment"), 5));
            }
            return View(await GetEnrollmentTask);
        }

        /// <summary>
        /// 报名的Post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Enroll(int? id, EnrollmentData model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await this.enrollmentManager.CreateOrUpdate(model);
                    return RedirectToAction("UploadArchives");
                }
                catch (Exception ex)
                {
                    this.ModelState.AddModelError("", ex.Message);
                }
            }

            //如果出错，重新显示此页。
            this.InitModelSelectionData(this.ViewData);
            return View(model);
        }

        public async Task<ActionResult> UploadArchives()
        {
            //返回报名需求项列表。
            //实际的报名资料项由子方法给出。
            var archReqSet = await this.archiveManager.GetArchiveRequirements();
            return View(archReqSet);
        }

        [ChildActionOnly]
        public async Task<ActionResult> ArchiveListOfEnrollment(ArchiveRequirements requirement)
        {
            EnrollmentData enrollmentData = await this.enrollmentManager.GetAvaiableOrDefault();

            var CurrentUserEnrollmentArchivesByRequired = from arch in enrollmentData.EnrollmentArchives
														  where arch.ArchiveCategoryID == requirement.ArchiveCategoryID
                                                          select arch;
			
            this.ViewData["user"] = this.targetUserManager.TargetUser;
            return PartialView(CurrentUserEnrollmentArchivesByRequired);
        }

        /// <summary>
        /// 浏览待提交的报名表，并执行提交。
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> CommitEnrollment()
        {
            if (this.enrollmentManager.HasEnrollmentData)
            {
                EnrollmentData data = await this.enrollmentManager.GetAvaiableOrDefault();
                if (data.WhenCommited.HasValue)
                    return View("OperationResult", new OperationResult(ResultStatus.Warning, "您的报名资料已提交，不能重复提交。您可以查看您所提交的报名资料。", this.Url.Action("PreviewEnrollment"), 5));
                return View(data);
            }
            return View("OperationResult", new OperationResult(ResultStatus.Failure, "找不到报名表数据", this.Url.Action("Index"), 3));
        }

        /// <summary>
        /// Post执行提交动作。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CommitEnrollment(bool Agreement)
        {
            try
            {
                await this.enrollmentManager.CommitEnrollment();
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
                EnrollmentData data = await this.enrollmentManager.GetAvaiableOrDefault();
                if (data.WhenCommited.HasValue)
                    return View("OperationResult", new OperationResult(ResultStatus.Warning, "您的报名资料已提交，不能重复提交。您可以查看您所提交的报名资料。", this.Url.Action("PreviewEnrollment"), 5));
                return View(data);
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
        public async Task<ActionResult> PreviewEnrollment()
        {
            if (this.enrollmentManager.HasEnrollmentData)
            {
                var enrollmentData = await this.enrollmentManager.GetAvaiableOrDefault();
                return View(enrollmentData);
            }

            return View("OperationResult", new OperationResult(ResultStatus.Failure, "没有找到报名信息。", this.Url.Action("Index"), 3));
        }

        /// <summary>
        /// 声明是否参加考试。
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AnnounceForExam()
        {
            if (this.enrollmentManager.HasEnrollmentData)
            {
                var enrollmentData = await this.enrollmentManager.GetAvaiableOrDefault();

                if (!enrollmentData.WhenAudit.HasValue)
                {
                    return View("OperationResult", new OperationResult(ResultStatus.Failure, "报名信息尚未被审核。", this.Url.Action("Index"), 3));
                }

                if (!enrollmentData.Approved.Value)
                {
                    return View("OperationResult", new OperationResult(ResultStatus.Failure, "报名信息审核没有通过。", this.Url.Action("Index"), 3));
                }

                return View(enrollmentData);
            }



            return View("OperationResult", new OperationResult(ResultStatus.Failure, "没有找到报名信息。", this.Url.Action("Index"), 3));
        }

        /// <summary>
        /// 提交声明是否参加考试。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AnnounceForExam(bool IsTakeExam)
        {
            try
            {
                await this.enrollmentManager.AnnounceForExam(IsTakeExam);
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
        public async Task<ActionResult> RecruitmentPanel(RecruitmentPlan plan)
        {
            RecruitmentPanelStateModel viewModel = new RecruitmentPanelStateModel();
            viewModel.Plan = plan;
            if (this.enrollmentManager.HasEnrollmentData)
            {
                viewModel.HasEnrollment = true;
                viewModel.Enrollment = await this.enrollmentManager.GetAvaiableOrDefault();
            }

            return PartialView(viewModel);
        }

        #endregion

        #region 帮助方法

        void InitModelSelectionData(ViewDataDictionary ViewData)
        {
            //学历选择表
            var eduSet = from edu in this.database.EducationBackground
                         orderby edu.PRI
                         select edu;

            if (this.recruitManager.SelectedRecruitPlan.IsPublic)
            {
                ViewData["EducationBackgroundTable"] = from e in eduSet
                                                       where e.IsPublic
                                                       select new SelectListItem() { Text = e.nid, Value = e.nid };
            }
            else
            {
                ViewData["EducationBackgroundTable"] = from e in eduSet
                                                       select new SelectListItem() { Text = e.nid, Value = e.nid };
            }

            //学位选择表
            ViewData["DegreeTable"] = from degree in this.database.Degree
                                      orderby degree.PRI
                                      select new SelectListItem() { Value = degree.nid, Text = degree.nid };

            //报考专业类别选择表
            ViewData["MajorTable"] = from cata in this.database.MajorCategory
                                     orderby cata.PRI
                                     select new SelectListItem() { Text = cata.nid, Value = cata.nid };

            //年份选择表
            List<SelectListItem> GraduatedYears = new List<SelectListItem>();
            GraduatedYears.Add(new SelectListItem() { Value = DateTime.Now.Year.ToString(), Text = DateTime.Now.Year.ToString() });
            if (!this.recruitManager.SelectedRecruitPlan.IsPublic)
                GraduatedYears.Add(new SelectListItem() { Value = (DateTime.Now.Year - 1).ToString(), Text = (DateTime.Now.Year - 1).ToString() });
            ViewData["GraduatedYears"] = GraduatedYears;

            //民族列表
            ViewData["Nationality"] = from nation in this.database.Nationality
                                      orderby nation.code
                                      select new SelectListItem() { Text = nation.Name, Value = nation.Name };
        }

        #endregion
    }
}