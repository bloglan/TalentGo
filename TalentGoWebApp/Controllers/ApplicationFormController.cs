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
    public class ApplicationFormController : Controller
    {
        ApplicationFormManager manager;
        IJobStore jobStore;

        public ApplicationFormController(ApplicationFormManager manager, IJobStore jobStore)
        {
            this.manager = manager;
            this.jobStore = jobStore;
        }

        // GET: ApplicationForm
        public ActionResult Index()
        {
            var person = this.CurrentUser();
            return View(this.manager.ApplicationForms.Where(p => p.PersonId == person.Id));
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
            this.InitModelSelectionData(job, this.ViewData);

            EnrollViewModel model = new EnrollViewModel()
            {
                Job = job,
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


            var user = this.CurrentUser();

            var job = this.jobStore.Jobs.EnrollableJobs().FirstOrDefault(j => j.Id == id);
            if (job == null)
                return HttpNotFound();

            model.Job = job;

            this.InitModelSelectionData(job, this.ViewData);
            if (!this.ModelState.IsValid)
                return View(model);

            if (user.ApplicationForms.Any(a => a.Job.PlanId == job.PlanId))
                return HttpNotFound("您已在此招聘计划中填写过报名表。");


            var form = new ApplicationForm(job, user)
            {
                NativePlace = model.NativePlace,
                Source = model.Source,
                PoliticalStatus = model.PoliticalStatus,
                Health = model.Health,
                Marriage = model.Marriage,
                School = model.School,
                Major = model.Major,
                YearOfGraduated = model.YearOfGraduated,
                EducationBackground = model.EducationBackground,
                Degree = model.Degree,
                Resume = model.Resume,
                Accomplishments = model.Accomplishments,
            };

            await this.manager.EnrollAsync(form);

            return RedirectToAction("UploadArchives");

        }
        void InitModelSelectionData(Job job, ViewDataDictionary ViewData)
        {
            //学历选择表
            var eduSet = job.EducationBackgroundRequirement.Trim().Trim('\r', '\n').Split('\r', '\n');


            ViewData["EducationBackgroundTable"] = from e in eduSet
                                                   select new SelectListItem() { Text = e, Value = e };

            var degreeSet = job.DegreeRequirement.Trim().Trim('\r', '\n').Split('\r', '\n');

            //学位选择表
            ViewData["DegreeTable"] = from e in degreeSet
                                      select new SelectListItem { Text = e, Value = e };


            //年份选择表
            List < SelectListItem > GraduatedYears = new List<SelectListItem>();
            GraduatedYears.Add(new SelectListItem() { Value = DateTime.Now.Year.ToString(), Text = DateTime.Now.Year.ToString() });
            GraduatedYears.Add(new SelectListItem() { Value = (DateTime.Now.Year - 1).ToString(), Text = (DateTime.Now.Year - 1).ToString() });
            ViewData["GraduatedYears"] = GraduatedYears;

        }

    }
}