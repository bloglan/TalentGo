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
using TalentGo.Recruitment;

namespace TalentGoWebApp.Controllers
{
    [Authorize(Roles = "InternetUser,QJYC\\招聘登记员,QJYC\\招聘管理员")]
    public class RecruitmentController : Controller
    {
        TargetUserManager targetUserManager;
        RecruitmentPlanManager recruitManager;
        EnrollmentManager enrollmentManager;
        RecruitmentContextBase recruitmentContext;

        TargetUser user = null;

        public RecruitmentController(TargetUserManager targetUserManager, RecruitmentPlanManager recruitmentPlanManager, EnrollmentManager enrollmentManager)
        {
            this.targetUserManager = targetUserManager;
            this.recruitManager = recruitmentPlanManager;
            this.enrollmentManager = enrollmentManager;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.recruitmentContext = this.HttpContext.GetRecruitmentContext();
            if (this.recruitmentContext.TargetUserId.HasValue)
                user = this.targetUserManager.TargetUsers.FirstOrDefault(t => t.Id == this.recruitmentContext.TargetUserId.Value);
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    base.OnException(filterContext);
        //    //?

        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //验证如果目标用户为null，则跳转绑定目标用户页。
            if (user == null)
            {
                filterContext.Result = RedirectToAction("AssignTargetUser", "TargetUser");
                return;
            }
        }

        /// <summary>
        /// 显示招聘首页，招聘首页应显示可用的报名计划以及相关操作按钮。
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            return View(await this.recruitManager.GetPlansForUser(this.user));
        }

        public async Task<ActionResult> Detail(int id)
        {
            var current = (await this.recruitManager.GetPlansForUser(this.user)).First(plan => plan.id == id);
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
            RecruitmentPlan plan = null;
            if (id.HasValue)
            {
                plan = (await this.recruitManager.GetPlansForUser(this.user)).FirstOrDefault(p => p.id == id.Value);
                if (plan == null)
                    return HttpNotFound();

                this.recruitmentContext.SelectedPlanId = plan.id;
            }
            else
            {
                if (!this.recruitmentContext.SelectedPlanId.HasValue)
                    return HttpNotFound();

                plan = (await this.recruitManager.GetPlansForUser(this.user)).FirstOrDefault(p => p.id == this.recruitmentContext.SelectedPlanId.Value);
            }

            //准备下拉框及相关数据
            this.InitModelSelectionData(plan, this.ViewData);

            var enrollment = this.enrollmentManager.Enrollments.FirstOrDefault(e => e.RecruitPlanID == plan.id && e.UserID == this.user.Id);
            if (enrollment == null)
                enrollment = await this.enrollmentManager.NewEnrollment(this.user, plan);

            if (enrollment.WhenCommited.HasValue)
            {
                return RedirectToAction("PreviewEnrollment");
                //return View("OperationResult", new OperationResult(ResultStatus.Warning, "您的报名资料已提交，不能重复提交。您可以查看您所提交的报名资料。", this.Url.Action("PreviewEnrollment"), 5));
            }
            return View(enrollment);
        }

        /// <summary>
        /// 报名的Post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Enroll(int? id, Enrollment model)
        {
            RecruitmentPlan plan = null;
            if (id.HasValue)
            {
                plan = (await this.recruitManager.GetPlansForUser(this.user)).FirstOrDefault(p => p.id == id.Value);
                if (plan == null)
                    return HttpNotFound();

                this.recruitmentContext.SelectedPlanId = plan.id;
            }
            else
            {
                if (!this.recruitmentContext.SelectedPlanId.HasValue)
                    return HttpNotFound();

                plan = (await this.recruitManager.GetPlansForUser(this.user)).FirstOrDefault(p => p.id == this.recruitmentContext.SelectedPlanId.Value);
            }

            if (ModelState.IsValid)
            {
                //如果有传入参数ID，则指示了要选中的招聘计划。
                


                if (this.enrollmentManager.Enrollments.Any(e => e.RecruitPlanID == plan.id && e.UserID == this.user.Id))
                {
                    //It's an existing enrollment and then update it.
                    await this.enrollmentManager.UpdateEnrollment(this.user, plan, model);
                }
                else
                {
                    await this.enrollmentManager.CreateEnrollment(this.user, plan, model);
                }
                return RedirectToAction("UploadArchives");
            }

            //如果出错，重新显示此页。
            this.InitModelSelectionData(plan, this.ViewData);
            return View(model);
        }

        public async Task<ActionResult> UploadArchives()
        {
            //返回报名需求项列表。
            //实际的报名资料项由子方法给出。
            if (!this.recruitmentContext.SelectedPlanId.HasValue)
                throw new NotSupportedException();

            var plan = await this.recruitManager.FindByIDAsync(this.recruitmentContext.SelectedPlanId.Value);
            var archReqSet = await this.recruitManager.GetArchiveRequirements(plan);
            return View(archReqSet);
        }

        [ChildActionOnly]
        public async Task<ActionResult> ArchiveListOfEnrollment(ArchiveRequirement requirement)
        {
            Enrollment enrollmentData = this.enrollmentManager.Enrollments.FirstOrDefault(e => e.UserID == this.user.Id && e.RecruitPlanID == this.recruitmentContext.SelectedPlanId.Value);

            var CurrentUserEnrollmentArchivesByRequired = from arch in await this.enrollmentManager.GetEnrollmentArchives(enrollmentData)
                                                          where arch.ArchiveCategoryID == requirement.ArchiveCategoryID
                                                          select arch;

            this.ViewData["user"] = this.user;
            return PartialView(CurrentUserEnrollmentArchivesByRequired);
        }

        /// <summary>
        /// 浏览待提交的报名表，并执行提交。
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> CommitEnrollment()
        {
            //
            var enrollment = this.enrollmentManager.Enrollments.FirstOrDefault(e => e.UserID == this.user.Id && e.RecruitPlanID == this.recruitmentContext.SelectedPlanId.Value);
            if (enrollment == null)
                return HttpNotFound();


            if (enrollment.WhenCommited.HasValue)
                return View("OperationResult", new OperationResult(ResultStatus.Warning, "您的报名资料已提交，不能重复提交。您可以查看您所提交的报名资料。", this.Url.Action("PreviewEnrollment"), 5));
            return View(enrollment);
        }

        /// <summary>
        /// Post执行提交动作。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CommitEnrollment(bool Agreement)
        {
            var plan = await this.recruitManager.FindByIDAsync(this.recruitmentContext.SelectedPlanId.Value);
            var enrollment = this.enrollmentManager.Enrollments.First(e => e.UserID == this.user.Id && e.RecruitPlanID == plan.id);
            try
            {
                
                await this.enrollmentManager.CommitEnrollment(this.user, plan, enrollment);
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
                    return View("OperationResult", new OperationResult(ResultStatus.Warning, "您的报名资料已提交，不能重复提交。您可以查看您所提交的报名资料。", this.Url.Action("PreviewEnrollment"), 5));
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
        public async Task<ActionResult> PreviewEnrollment()
        {
            if (!this.recruitmentContext.SelectedPlanId.HasValue)
                return HttpNotFound();

            var plan = this.recruitManager.FindByIDAsync(this.recruitmentContext.SelectedPlanId.Value);

            var enrollment = this.enrollmentManager.Enrollments.First(e => e.UserID == this.user.Id && e.RecruitPlanID == plan.Id);
            return View(enrollment);

        }

        /// <summary>
        /// 声明是否参加考试。
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AnnounceForExam()
        {
            if (!this.recruitmentContext.SelectedPlanId.HasValue)
                return HttpNotFound();

            var plan = this.recruitManager.FindByIDAsync(this.recruitmentContext.SelectedPlanId.Value);

            var enrollment = this.enrollmentManager.Enrollments.First(e => e.UserID == this.user.Id && e.RecruitPlanID == plan.Id);
            return View(enrollment);
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
                var plan = await this.recruitManager.FindByIDAsync(this.recruitmentContext.SelectedPlanId.Value);
                await this.enrollmentManager.AnnounceForExam(this.user, plan, IsTakeExam);
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
            viewModel.HasEnrollment = true;
            viewModel.Enrollment = this.enrollmentManager.Enrollments.First(e => e.UserID == this.user.Id && e.RecruitPlanID == plan.id);

            return PartialView(viewModel);
        }

        #endregion

        #region 帮助方法

        void InitModelSelectionData(RecruitmentPlan plan, ViewDataDictionary ViewData)
        {
            //学历选择表
            var eduSet = new List<EducationBackground>()
            {
                new EducationBackground()
                {
                    nid = "全日制博士研究生",
                    IsPublic = true,
                },
                new EducationBackground()
                {
                    nid = "全日制硕士研究生",
                    IsPublic = true,
                },
                new EducationBackground()
                {
                    nid = "全日制一本",
                    IsPublic = true,
                },
                new EducationBackground()
                {
                    nid = "全日制二本",
                    IsPublic = true,
                },
                new EducationBackground()
                {
                    nid = "全日制三本",
                    IsPublic = true,
                },
                new EducationBackground()
                {
                    nid = "全日制专升本",
                    IsPublic = true,
                }
            };

            if (plan.IsPublic)
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
            ViewData["DegreeTable"] = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "学士",
                    Value = "学士"
                },
                new SelectListItem()
                {
                    Text = "硕士研究生",
                    Value = "硕士研究生"
                },
                new SelectListItem()
                {
                    Text = "博士研究生",
                    Value = "博士研究生"
                }
            };

            //报考专业类别选择表
            ViewData["MajorTable"] = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "财务会计",
                    Value = "财务会计"
                },
                new SelectListItem()
                {
                    Text = "计算机",
                    Value = "计算机"
                },
                new SelectListItem()
                {
                    Text = "农学",
                    Value = "农学"
                },
                new SelectListItem()
                {
                    Text = "综合",
                    Value = "综合"
                }
            };

            //年份选择表
            List<SelectListItem> GraduatedYears = new List<SelectListItem>();
            GraduatedYears.Add(new SelectListItem() { Value = DateTime.Now.Year.ToString(), Text = DateTime.Now.Year.ToString() });
            if (!plan.IsPublic)
                GraduatedYears.Add(new SelectListItem() { Value = (DateTime.Now.Year - 1).ToString(), Text = (DateTime.Now.Year - 1).ToString() });
            ViewData["GraduatedYears"] = GraduatedYears;

            //民族列表
#warning 需要再实现。
            ViewData["Nationality"] = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "汉",
                    Value = "汉"
                },
                new SelectListItem()
                {
                    Text = "其他",
                    Value = "其他"
                }
            };
        }

        #endregion
    }
}