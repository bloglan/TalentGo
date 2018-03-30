using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentGo;

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
        public ActionResult CandidateList()
        {
            var user = this.CurrentUser();
            var candidates = this.manager.Candidates.AvailableForUser(user.Id);
            return PartialView("_CandidateList", candidates);
        }
    }
}