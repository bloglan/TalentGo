using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TalentGo;
using TalentGoWebApp.Models;

namespace TalentGoWebApp.Controllers
{
    [Authorize]
    public class ExaminationController : Controller
    {
        CandidateManager manager;

        public ExaminationController(CandidateManager manager)
        {
            this.manager = manager;
        }

        // GET: Examination
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult UserExaminations()
        {
            var userId = this.CurrentUser().Id;
            var candidates = this.manager.Candidates.Where(c => c.Plan.WhenPublished.HasValue && c.PersonId == userId);
            return PartialView("_UserExaminationList", candidates);
        }

        [ChildActionOnly]
        public ActionResult CandidateList()
        {
            var user = this.CurrentUser();
            var candidates = this.manager.Candidates.AvailableForUser(user.Id);
            return PartialView("_CandidateList", candidates);
        }

        [ChildActionOnly]
        public ActionResult AdmissionTicketList()
        {
            var userId = this.CurrentUser().Id;
            var candidates = this.manager.Candidates.Where(c => c.Plan.WhenAdmissionTicketReleased.HasValue && c.PersonId == userId);
            return PartialView("_TicketList", candidates);
        }

        public ActionResult AttendanceConfirm(int id)
        {
            return View(new AttendanceConfirmModel { Attend = true });
        }

        [HttpPost]
        public async Task<ActionResult> AttendanceConfirm(int id, AttendanceConfirmModel model)
        {
            var userid = this.CurrentUser().Id;
            var candidate = await this.manager.FindByIdAsync(userid, id);
            if (candidate == null)
                return HttpNotFound();

            if (candidate.WhenConfirmed.HasValue)
            {
                this.ModelState.AddModelError("", "您已提交过声明。");
                return View(model);
            }

            try
            {
                await this.manager.ConfirmAttendance(candidate, model.Attend);
                return View("_OperationSuccess");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Examination Id</param>
        /// <returns></returns>
        public async Task<ActionResult> Ticket(int id)
        {
            var candidate = await this.manager.FindByIdAsync(this.CurrentUser().Id, id);
            if (candidate == null)
                return HttpNotFound();

            if (!candidate.Plan.WhenAdmissionTicketReleased.HasValue)
                return HttpNotFound();

            return View(candidate);
        }
    }
}