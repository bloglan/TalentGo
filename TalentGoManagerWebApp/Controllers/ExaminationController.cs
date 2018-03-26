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
        ExaminationManager manager;

        public ExaminationController(ExaminationManager manager)
        {
            this.manager = manager;
        }

        // GET: Examination
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult ExaminationPlans()
        {
            var plans = this.manager.Exams;
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

            var plan = new Examination(model.Title, model.AttendanceConfirmationExpiresAt);
            try
            {
                await this.manager.CreateAsync(plan);
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
            var plan = await this.manager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            var model = new ExaminationPlanEditViewModel()
            {
                Title = plan.Title,
                AttendanceConfirmationExpiresAt = plan.AttendanceConfirmationExpiresAt,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, ExaminationPlanEditViewModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            var plan = await this.manager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();
            plan.Title = model.Title;
            plan.AttendanceConfirmationExpiresAt = model.AttendanceConfirmationExpiresAt;
            try
            {
                await this.manager.UpdateAsync(plan);
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
            var plan = await this.manager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            if (plan.WhenPublished.HasValue)
                return HttpNotFound();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Publish(int id, FormCollection collection)
        {
            var plan = await this.manager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            if (plan.WhenPublished.HasValue)
                return HttpNotFound();

            try
            {
                await this.manager.PublishAsync(plan);
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
            var plan = await this.manager.FindByIdAsync(planId);
            if (plan == null)
                return HttpNotFound();

            var model = new SubjectEditViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddSubject(int planId, SubjectEditViewModel model)
        {
            var plan = await this.manager.FindByIdAsync(planId);
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
                await this.manager.UpdateAsync(plan);
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
            var plan = await this.manager.FindByIdAsync(planId);
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
            var plan = await this.manager.FindByIdAsync(planId);
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
                await this.manager.UpdateAsync(plan);
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
            var plan = await this.manager.FindByIdAsync(planId);
            if (plan == null)
                return Json("Plan not found");

            var subject = plan.Subjects.FirstOrDefault(s => s.Id == id);
            if (subject == null)
                return Json("subject not found");

            plan.Subjects.Remove(subject);

            try
            {
                await this.manager.UpdateAsync(plan);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }

        public async Task<ActionResult> Detail(int id)
        {
            var plan = await this.manager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            return View(plan);
        }
    }
}