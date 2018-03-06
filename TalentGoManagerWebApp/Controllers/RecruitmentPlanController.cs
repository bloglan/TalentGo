using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TalentGo;
using TalentGo.Models;
using TalentGoManagerWebApp.Models;

namespace TalentGoManagerWebApp.Controllers
{
    [Authorize(Roles = "QJYC\\招聘管理员,QJYC\\招聘监督人")]
    public class RecruitmentPlanController : Controller
    {
        RecruitmentPlanManager planManager;
        ApplicationFormManager formManager;

        public RecruitmentPlanController(RecruitmentPlanManager planManager, ApplicationFormManager formManager)
        {
            this.planManager = planManager;
            this.formManager = formManager;
        }


        // GET: Mgmt/RecruitmentPlan
        public ActionResult Index()
        {
            //id: Year of recruitment plan.
            var plans = this.planManager.RecruitmentPlans;
            return View(plans);
        }

        /// <summary>
        /// 查看详情。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Detail(int? id)
        {
            if (!id.HasValue)
                return HttpNotFound();

            return View(await this.planManager.FindByIdAsync(id.Value));
        }

        public ActionResult Create()
        {
            return View(new RecruitmentPlanPrimaryViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(RecruitmentPlanPrimaryViewModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);
            RecruitmentPlan newplan = new RecruitmentPlan()
            {
                Title = model.Title,
                Recruitment = model.Recruitment,
                Publisher = model.Publisher
            };

            await this.planManager.CreateAsync(newplan);
            return RedirectToAction("ArchiveRequirements", new { id = newplan.Id });
        }

        public async Task<ActionResult> Delete(int id)
        {

            var current = await this.planManager.FindByIdAsync(id);
            if (current == null)
            {
                return RedirectToAction("Index");
            }

            await this.planManager.DeleteRecruitmentPlan(current);
            return RedirectToAction("Index");

        }

        public async Task<ActionResult> Edit(int id)
        {
            RecruitmentPlan plan = await this.planManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            RecruitmentPlanPrimaryViewModel vmodel = new RecruitmentPlanPrimaryViewModel()
            {
                Title = plan.Title,
                Recruitment = plan.Recruitment,
                Publisher = plan.Publisher
            };
            return View(vmodel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, RecruitmentPlanPrimaryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var plan = await this.planManager.FindByIdAsync(id);
                plan.Title = model.Title;
                plan.Recruitment = model.Recruitment;
                plan.Publisher = model.Publisher;


                await this.planManager.UpdateAsync(plan);
                return RedirectToAction("ArchiveRequirements", new { id = id });
            }
            return View(model);
        }

        public async Task<ActionResult> Publish(int id)
        {
            PublishRecruitmentPlanViewModel model = new PublishRecruitmentPlanViewModel()
            {
                Plan = await this.planManager.FindByIdAsync(id)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Publish(int id, PublishRecruitmentPlanViewModel model)
        {
            await this.planManager.PublishRecruitmentPlan(id, model.EnrollExpirationDate);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CommitAudit(int id)
        {
            var plan = await this.planManager.FindByIdAsync(id);
            if (plan == null)
            {
                return View("OperationResult", new OperationResult(ResultStatus.Failure, "找不到计划。", this.Url.Action("Index"), 3));
            }
            // bool isPass = plan.EnrollExpirationDate < DateTime.Now ? true : false;
            if (!(plan.EnrollExpirationDate < DateTime.Now))
            {
                return View();
            }
            DateTime examSampleTime = DateTime.Now.AddDays(15); //15天后考试
            CommitAuditViewModel model = new CommitAuditViewModel()
            {
                Plan = plan,
                AnnounceExpirationDate = DateTime.Now.AddDays(7),
                ExamStartTime = new DateTime(examSampleTime.Year, examSampleTime.Month, examSampleTime.Day, 9, 0, 0),
                ExamEndTime = new DateTime(examSampleTime.Year, examSampleTime.Month, examSampleTime.Day, 11, 0, 0),
                ExamLocation = "云南省烟草公司曲靖市公司"
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CommitAudit(int id, CommitAuditViewModel model)
        {
            var plan = await this.planManager.FindByIdAsync(id);
            if (plan == null)
            {
                return View("OperationResult", new OperationResult(ResultStatus.Failure, "找不到计划。", this.Url.Action("Index"), 3));
            }
            model.Plan = plan;
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            //验证合规性。
            List<string> Errors = new List<string>();
            //考试结束时间应大于考试开始时间，
            if (model.ExamEndTime <= model.ExamStartTime)
                Errors.Add("考试结束时间不能早于开始时间。");
            if (model.ExamStartTime <= model.AnnounceExpirationDate.AddDays(1))
                Errors.Add("考试开始时间不能早于声明参加考试截止时间的次日");
            if (model.AnnounceExpirationDate <= DateTime.Now.AddDays(1))
                Errors.Add("声明参加考试截止日期至少需要1天以上。");

            if (Errors.Count != 0)
            {
                foreach (string err in Errors)
                {
                    this.ModelState.AddModelError("", err);
                }
                return View(model);
            }

            //开始提交。
            try
            {
                plan.AnnounceExpirationDate = model.AnnounceExpirationDate;
                plan.ExamStartTime = model.ExamStartTime;
                plan.ExamEndTime = model.ExamEndTime;
                plan.ExamLocation = model.ExamLocation;
                //await this.recruitmentManager.CommitAudit(plan, model.AnnounceExpirationDate, model.ExamStartTime, model.ExamEndTime, model.ExamLocation);
                await this.planManager.CompleteAudit(plan);
                //return RedirectToAction("Index");
                return View("OperationResult", new OperationResult(ResultStatus.Success, "该计划已成功结束审核，审核结果将自动通过短信和邮件顺次通知应聘者本人。接下来，应聘者将提交是否参加考试的声明。在声明截止时间过后，您将能获得本次招聘计划参加考试的人员名单及统计信息。", this.Url.Action("Index"), 20));
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        #region ChildActions


        #endregion
    }
}