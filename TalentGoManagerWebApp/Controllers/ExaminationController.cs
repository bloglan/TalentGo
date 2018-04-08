using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TalentGo;
using TalentGoManagerWebApp.Models;

namespace TalentGoManagerWebApp.Controllers
{
    public class ExaminationController : Controller
    {
        ExaminationPlanManager examManager;
        CandidateManager candidateManager;
        IRecruitmentPlanStore recruitmentPlanStore;
        IJobStore jobStore;

        public ExaminationController(ExaminationPlanManager examManager, CandidateManager candidateManager, IRecruitmentPlanStore recruitmentPlanStore, IJobStore jobStore)
        {
            this.examManager = examManager;
            this.candidateManager = candidateManager;
            this.recruitmentPlanStore = recruitmentPlanStore;
            this.jobStore = jobStore;
        }

        // GET: Examination
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult ExaminationPlans()
        {
            var plans = this.examManager.Plans;
            return PartialView("_ExaminationPlanList", plans);
        }

        public ActionResult Create()
        {
            var model = new ExaminationPlanEditViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ExaminationPlanEditViewModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            var plan = new ExaminationPlan(model.Title, model.Address)
            {
                AttendanceConfirmationExpiresAt = model.AttendanceConfirmationExpiresAt,
            };
            try
            {
                await this.examManager.CreateAsync(plan);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var plan = await this.examManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            var model = new ExaminationPlanEditViewModel()
            {
                Title = plan.Title,
                Address = plan.Address,
                AttendanceConfirmationExpiresAt = plan.AttendanceConfirmationExpiresAt,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, ExaminationPlanEditViewModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            var plan = await this.examManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();
            plan.Title = model.Title;
            plan.Address = model.Address;
            plan.AttendanceConfirmationExpiresAt = model.AttendanceConfirmationExpiresAt;
            try
            {
                await this.examManager.UpdateAsync(plan);
                return RedirectToAction("Detail", new { id });
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public async Task<ActionResult> Publish(int id)
        {
            var plan = await this.examManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            if (plan.WhenPublished.HasValue)
                return HttpNotFound();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Publish(int id, FormCollection collection)
        {
            var plan = await this.examManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            if (plan.WhenPublished.HasValue)
                return HttpNotFound();

            try
            {
                await this.examManager.PublishAsync(plan);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public async Task<ActionResult> AddSubject(int planId)
        {
            var plan = await this.examManager.FindByIdAsync(planId);
            if (plan == null)
                return HttpNotFound();

            var model = new SubjectEditViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddSubject(int planId, SubjectEditViewModel model)
        {
            var plan = await this.examManager.FindByIdAsync(planId);
            if (plan == null)
                return HttpNotFound();

            if (!this.ModelState.IsValid)
                return View(model);

            var subject = new ExaminationSubject() {
                Subject = model.Subject,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
            };

            plan.Subjects.Add(subject);
            try
            {
                await this.examManager.UpdateAsync(plan);
                return RedirectToAction("Detail", new { id = planId });
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public async Task<ActionResult> EditSubject(int id, int planId)
        {
            var plan = await this.examManager.FindByIdAsync(planId);
            if (plan == null)
                return HttpNotFound();

            var subject = plan.Subjects.FirstOrDefault(s => s.Id == id);
            if (subject == null)
                return HttpNotFound();

            var model = new SubjectEditViewModel {
                Subject = subject.Subject,
                StartTime = subject.StartTime,
                EndTime = subject.EndTime,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditSubject(int id, int planId, SubjectEditViewModel model)
        {
            var plan = await this.examManager.FindByIdAsync(planId);
            if (plan == null)
                return HttpNotFound();

            var subject = plan.Subjects.FirstOrDefault(s => s.Id == id);
            if (subject == null)
                return HttpNotFound();

            if (!this.ModelState.IsValid)
                return View(model);

            subject.Subject = model.Subject;
            subject.StartTime = model.StartTime;
            subject.EndTime = model.EndTime;

            try
            {
                await this.examManager.UpdateAsync(plan);
                return RedirectToAction("Detail", new { id = planId });
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<JsonResult> RemoveSubject(int planId, int id)
        {
            var plan = await this.examManager.FindByIdAsync(planId);
            if (plan == null)
                return Json("Plan not found");

            var subject = plan.Subjects.FirstOrDefault(s => s.Id == id);
            if (subject == null)
                return Json("subject not found");

            plan.Subjects.Remove(subject);

            try
            {
                await this.examManager.UpdateAsync(plan);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">ExaminationPlan Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ImportFromRecruitmentPlan(int id)
        {
            var examPlan = await this.examManager.FindByIdAsync(id);
            if (examPlan == null)
                return HttpNotFound();

            this.ViewData["RecruitmentPlanList"] = this.GetRecruitmentPlanList();
            var model = new ImportFromRecruitmentPlanEditModel();
            return View(model);

        }

        [HttpPost]
        public async Task<ActionResult> ImportFromRecruitmentPlan(int id, ImportFromRecruitmentPlanEditModel model)
        {
            this.ViewData["RecruitmentPlanList"] = this.GetRecruitmentPlanList();

            if (!this.ModelState.IsValid)
                return View(model);

            var examPlan = await this.examManager.FindByIdAsync(id);
            var recruitmentPlan = await this.recruitmentPlanStore.FindByIdAsync(model.SelectedRecruitmentPlanId);
            await this.candidateManager.ImportFromRecruitmentPlanAsync(examPlan, recruitmentPlan);
            return View("_OperationSuccess");
        }

        IEnumerable<SelectListItem> GetRecruitmentPlanList()
        {
            foreach(var plan in this.recruitmentPlanStore.Plans.Where(r => r.WhenAuditCommited.HasValue))
            {
                yield return new SelectListItem {
                    Value = plan.Id.ToString(),
                    Text = plan.Title
                };
            }
        }

        IEnumerable<SelectListItem> GetJobList()
        {
            foreach(var job in this.jobStore.Jobs.Where(j => j.Plan.WhenAuditCommited.HasValue))
            {
                yield return new SelectListItem
                {
                    Value = job.Id.ToString(),
                    Text = job.Name + "|" + job.Plan.Title,
                };
            }
        }

        public async Task<ActionResult> Detail(int id)
        {
            var plan = await this.examManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            return View(plan);
        }
    }
}