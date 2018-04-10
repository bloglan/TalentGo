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
                Notes = model.Notes,
            };
            try
            {
                await this.examManager.CreateAsync(plan);
                return RedirectToAction("Detail", new { id = plan.Id });
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
                Notes = plan.Notes,
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
            plan.Notes = model.Notes;
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

        public ActionResult Delete(int id)
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            var plan = await this.examManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            try
            {
                await this.examManager.DeleteAsync(plan);
                return View("_OperationSuccess");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View();
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
                return RedirectToAction("Detail", new { id });
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

            var subject = new ExaminationSubject()
            {
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

            var model = new SubjectEditViewModel
            {
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
        /// <param name="examid">ExaminationPlan Id</param>
        /// <returns></returns>
        public async Task<ActionResult> ImportFromRecruitmentPlan(int examid)
        {
            var examPlan = await this.examManager.FindByIdAsync(examid);
            if (examPlan == null)
                return HttpNotFound();

            this.ViewData["RecruitmentPlanList"] = this.GetRecruitmentPlanList();
            var model = new ImportFromRecruitmentPlanEditModel();
            return View(model);

        }

        [HttpPost]
        public async Task<ActionResult> ImportFromRecruitmentPlan(int examid, ImportFromRecruitmentPlanEditModel model)
        {
            this.ViewData["RecruitmentPlanList"] = this.GetRecruitmentPlanList();

            if (!this.ModelState.IsValid)
                return View(model);

            var examPlan = await this.examManager.FindByIdAsync(examid);
            var recruitmentPlan = await this.recruitmentPlanStore.FindByIdAsync(model.SelectedRecruitmentPlanId);
            await this.candidateManager.ImportFromRecruitmentPlanAsync(examPlan, recruitmentPlan);
            return RedirectToAction("Detail", new { id = examid });
        }

        public ActionResult ImportFromJob(int examid)
        {
            return this.FeatureNotImplemented();
        }

        public ActionResult ImportFromExaminationPlan(int examid)
        {
            return this.FeatureNotImplemented();
        }

        IEnumerable<SelectListItem> GetRecruitmentPlanList()
        {
            foreach (var plan in this.recruitmentPlanStore.Plans.Where(r => r.WhenAuditCommited.HasValue))
            {
                yield return new SelectListItem
                {
                    Value = plan.Id.ToString(),
                    Text = plan.Title
                };
            }
        }

        IEnumerable<SelectListItem> GetJobList()
        {
            foreach (var job in this.jobStore.Jobs.Where(j => j.Plan.WhenAuditCommited.HasValue))
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

        public ActionResult EditAdmissionTickets(int id)
        {
            var candidates = this.candidateManager.Candidates.AttendanceConfirmed().Where(c => c.ExamId == id).OrderBy(c => c.Person.IDCardNumber);
            return View(candidates);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateTicketNumber(Guid personid, int examid, string data)
        {
            var candidate = await this.candidateManager.FindByIdAsync(personid, examid);
            try
            {
                await this.candidateManager.SetTicketNumberAsync(candidate, data);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UpdateRoom(Guid personid, int examid, string data)
        {
            var candidate = await this.candidateManager.FindByIdAsync(personid, examid);
            var room = candidate.Room;
            var seat = candidate.Seat;
            room = string.IsNullOrEmpty(data) ? null : data;

            try
            {
                await this.candidateManager.SetRoomSeatAsync(candidate, room, seat);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }
        [HttpPost]
        public async Task<JsonResult> UpdateSeat(Guid personid, int examid, string data)
        {
            var candidate = await this.candidateManager.FindByIdAsync(personid, examid);
            var room = candidate.Room;
            var seat = candidate.Seat;
            seat = string.IsNullOrEmpty(data) ? null : data;

            try
            {
                await this.candidateManager.SetRoomSeatAsync(candidate, room, seat);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public ActionResult ReleaseAdmissionTickets(int id)
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ReleaseAdmissionTickets(int id, FormCollection collection)
        {
            var plan = await this.examManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            try
            {
                await this.examManager.ReleaseAdmissionTicket(plan);
                return RedirectToAction("Detail", new { id });
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    }
}