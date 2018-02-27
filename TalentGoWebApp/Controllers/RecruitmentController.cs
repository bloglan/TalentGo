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
using TalentGo.Utilities;

namespace TalentGoWebApp.Controllers
{
    [Authorize]
    public class RecruitmentController : Controller
    {
        RecruitmentPlanManager recruitManager;
        ApplicationFormManager applicationFormManager;
        RecruitmentContextBase recruitmentContext;
        ApplicationUserManager targetUserManager;
        IJobStore jobStore;

        Person user = null;

        public RecruitmentController(RecruitmentPlanManager recruitmentPlanManager, ApplicationFormManager enrollmentManager, ApplicationUserManager userManager, IJobStore jobStore)
        {
            this.recruitManager = recruitmentPlanManager;
            this.applicationFormManager = enrollmentManager;
            this.targetUserManager = userManager;
            this.jobStore = jobStore;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.recruitmentContext = this.HttpContext.GetRecruitmentContext();
            if (this.recruitmentContext.TargetUserId.HasValue)
                user = this.targetUserManager.Users.FirstOrDefault(t => t.Id == this.recruitmentContext.TargetUserId.Value);
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
            return View(this.recruitManager.GetPlansForUser(this.user));
        }

        public async Task<ActionResult> Detail(int id)
        {
            var current = (this.recruitManager.GetPlansForUser(this.user)).First(plan => plan.Id == id);
            if (current == null)
                return HttpNotFound();

            return View(current);
        }


        /// <summary>
        /// 报名（填写和编辑报名表）
        /// </summary>
        /// <param name="id">JobId</param>
        /// <returns></returns>
        public ActionResult Enroll(int id)
        {
            var user = this.CurrentUser();

            var job = this.jobStore.Jobs.EnrollableJobs().FirstOrDefault(j => j.Id == id);
            if (job == null)
                return HttpNotFound();

            if (user.ApplicationForms.Any(a => a.Job.PlanId == job.PlanId))
                return HttpNotFound("您已在此招聘计划中填写过报名表。");


            ChineseIDCardNumber number = ChineseIDCardNumber.Parse(user.IDCardNumber);

            //准备下拉框及相关数据
            this.InitModelSelectionData(job.Plan, this.ViewData);

            EnrollViewModel model = new EnrollViewModel()
            {
                Name = user.DisplayName,
                Sex = number.IsMale ? "男" : "女",
                DateOfBirth = number.DateOfBirth,
                IDCardNumber = number.ToString(),
                Mobile = user.Mobile,
                Resume = "格式：\r\n 高中  1995.07-1998.09  曲靖一中   学生\r\n",
                Accomplishments = "",
            };

            return View(model);
        }

        /// <summary>
        /// 报名的Post
        /// </summary>
        /// <param name="id">JobId</param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Enroll(int id, EnrollViewModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            var job = this.jobStore.Jobs.EnrollableJobs().FirstOrDefault(j => j.Id == id);
            if (job == null)
                return HttpNotFound();

            if (user.ApplicationForms.Any(a => a.Job.PlanId == job.PlanId))
                return HttpNotFound("您已在此招聘计划中填写过报名表。");

            this.InitModelSelectionData(job.Plan, this.ViewData);

            var form = new ApplicationForm(job, user)
            {
                Name = model.Name,
                Sex = model.Sex,
                DateOfBirth = model.DateOfBirth,
                Nationality = model.Nationality,
                PlaceOfBirth = model.PlaceOfBirth,
                Source = model.Source,
                PoliticalStatus = model.PoliticalStatus,
                Health = model.Health,
                Marriage = model.Marriage,
                IDCardNumber = model.IDCardNumber,
                Mobile = model.Mobile,
                School = model.School,
                Major = model.Major,
                YearOfGraduated = model.YearOfGraduated,
                SelectedMajor = model.SelectedMajor,
                EducationBackground = model.EducationBackground,
                Degree = model.Degree,
                Resume = model.Resume,
                Accomplishments = model.Accomplishments,
            };

            await this.applicationFormManager.CreateAsync(form);

            return RedirectToAction("UploadArchives");

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
            ApplicationForm enrollmentData = this.applicationFormManager.ApplicationForms.FirstOrDefault(e => e.UserId == this.user.Id && e.JobId == this.recruitmentContext.SelectedPlanId.Value);

            var CurrentUserEnrollmentArchivesByRequired = from arch in await this.applicationFormManager.GetEnrollmentArchives(enrollmentData)
                                                          where arch.ArchiveCategoryID == requirement.ArchiveCategoryID
                                                          select arch;

            this.ViewData["user"] = this.user;
            return PartialView(CurrentUserEnrollmentArchivesByRequired);
        }

        /// <summary>
        /// 浏览待提交的报名表，并执行提交。
        /// </summary>
        /// <returns></returns>
        public ActionResult CommitEnrollment()
        {
            //
            var enrollment = this.applicationFormManager.ApplicationForms.FirstOrDefault(e => e.UserId == this.user.Id && e.JobId == this.recruitmentContext.SelectedPlanId.Value);
            if (enrollment == null)
                return HttpNotFound();


            if (enrollment.HasCommited)
                return View("OperationResult", new OperationResult(ResultStatus.Warning, "您的报名资料已提交，不能重复提交。您可以查看您所提交的报名资料。", this.Url.Action("PreviewEnrollment", new { id = enrollment.JobId }), 5));
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
            var enrollment = this.applicationFormManager.ApplicationForms.First(e => e.UserId == this.user.Id && e.JobId == plan.Id);
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

            var plan = await this.recruitManager.FindByIDAsync(id.Value);

            var enrollment = this.applicationFormManager.ApplicationForms.First(e => e.UserId == this.user.Id && e.JobId == plan.Id);
            return View(enrollment);

        }

        /// <summary>
        /// 声明是否参加考试。
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AnnounceForExam(int? id)
        {
            if (!id.HasValue)
                return HttpNotFound();

            var plan = await this.recruitManager.FindByIDAsync(id.Value);

            var enrollment = this.applicationFormManager.ApplicationForms.First(e => e.UserId == this.user.Id && e.JobId == plan.Id);
            return View(enrollment);
        }

        /// <summary>
        /// 提交声明是否参加考试。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> AnnounceForExam(int? id, bool IsTakeExam)
        {
            try
            {
                if (!id.HasValue)
                    return HttpNotFound();

                var plan = await this.recruitManager.FindByIDAsync(id.Value);
                await this.applicationFormManager.AnnounceForExamAsync(this.user, plan, IsTakeExam);
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
            var enrollment = this.applicationFormManager.ApplicationForms.FirstOrDefault(e => e.UserId == this.user.Id && e.JobId == plan.Id);
            if (enrollment != null)
            {
                viewModel.HasEnrollment = true;
                viewModel.Enrollment = enrollment;
            }


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

            List<string> nationalityStrList = new List<string>()
            {
                "汉", "蒙古", "回", "藏", "维吾尔", "苗"
,               "彝", "壮", "布依", "朝鲜", "满", "侗"
,               "瑶", "白", "土家", "哈尼", "哈萨克", "傣"
,               "黎", "傈僳", "佤", "畲", "高山", "拉祜"
,               "水", "东乡", "纳西", "景颇", "柯尔克孜", "土", "达翰尔"
,               "仫佬", "羌", "布朗", "撒拉", "毛南", "仡佬", "锡伯"
,               "阿昌", "普米", "塔吉克", "怒", "乌孜别克", "俄罗斯", "鄂温克"
,               "德昂", "保安", "裕固", "京", "塔塔尔", "独龙", "鄂伦春"
,               "赫哲", "门巴", "珞巴", "基诺", "其他"
            };
            //民族列表
            ViewData["Nationality"] = from nat in nationalityStrList
                                      select new SelectListItem() { Text = nat, Value = nat };
        }

        #endregion
    }
}