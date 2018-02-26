using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentGo;

namespace TalentGoWebApp.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        IJobStore store;

        public JobController(IJobStore jobStore)
        {
            this.store = jobStore;
        }

        // GET: Job
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult EnrollableJobs()
        {
            return View(this.store.Jobs.EnrollableJobs());
        }
    }
}