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
    public class RecruitmentPlanController : Controller
    {
        RecruitmentPlanManager planManager;

        public RecruitmentPlanController(RecruitmentPlanManager planManager)
        {
            this.planManager = planManager;
        }


        // GET: Mgmt/RecruitmentPlan
        public ActionResult Index()
        {
            //id: Year of recruitment plan.
            var plans = this.planManager.Plans;
            return View(plans);
        }

        /// <summary>
        /// 查看详情。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Detail(int id)
        {
            var plan = await this.planManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            return View(plan);
        }

        public ActionResult Create()
        {
            var model = new RecruitmentPlanEditViewModel
            {
                ExpirationDate = DateTime.Now.AddMonths(1),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(RecruitmentPlanEditViewModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            RecruitmentPlan plan = new RecruitmentPlan(model.Title, model.Recruitment, model.ExpirationDate);

            await this.planManager.CreateAsync(plan);
            return RedirectToAction("Detail", new { id = plan.Id });
        }

        public async Task<ActionResult> Delete(int id)
        {
            var plan = await this.planManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {

            var plan = await this.planManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            try
            {
                await this.planManager.DeleteAsync(plan);
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View();
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var plan = await this.planManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            var model = new RecruitmentPlanEditViewModel()
            {
                Title = plan.Title,
                Recruitment = plan.Recruitment,
                ExpirationDate = plan.EnrollExpirationDate,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, RecruitmentPlanEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var plan = await this.planManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            plan.Title = model.Title;
            plan.Recruitment = model.Recruitment;
            plan.EnrollExpirationDate = model.ExpirationDate;
            try
            {
                await this.planManager.UpdateAsync(plan);
                return RedirectToAction("Detail", new { id = plan.Id });
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
        /// <param name="planid">Recruitment plan id.</param>
        /// <returns></returns>
        public async Task<ActionResult> CreateJob(int planid)
        {
            var plan = await this.planManager.FindByIdAsync(planid);
            if (plan == null)
                return HttpNotFound();

            var model = new JobEditViewModel
            {
                WorkLocation = "聘用时分配",
                EducationBakcgroundRequirement = "本科\r\n硕士研究生\r\n博士研究生",
                DegreeRequirement = "学士\r\n硕士\r\n博士\r\n",
                MajorRequirement = "不限",
            };
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planid">Recruitment Plan Id.</param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateJob(int planid, JobEditViewModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            var plan = await this.planManager.FindByIdAsync(planid);
            if (plan == null)
                return HttpNotFound();

            var job = new Job
            {
                Name = model.Name,
                Description = model.Description,
                WorkLocation = model.WorkLocation,
                EducationBackgroundRequirement = model.EducationBakcgroundRequirement,
                DegreeRequirement = model.DegreeRequirement,
                MajorRequirement = model.MajorRequirement,
            };

            plan.Jobs.Add(job);
            await this.planManager.UpdateAsync(plan);

            return RedirectToAction("Detail", new { id = plan.Id });
        }

        public async Task<ActionResult> EditJob(int planid, int id)
        {
            var plan = await this.planManager.FindByIdAsync(planid);
            if (plan == null)
                return HttpNotFound();
            var job = plan.Jobs.FirstOrDefault(j => j.Id == id);
            if (job == null)
                return HttpNotFound();

            var model = new JobEditViewModel
            {
                Name = job.Name,
                Description = job.Description,
                WorkLocation = job.WorkLocation,
                EducationBakcgroundRequirement = job.EducationBackgroundRequirement,
                DegreeRequirement = job.DegreeRequirement,
                MajorRequirement = job.MajorRequirement,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditJob(int planid, int id, JobEditViewModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            var plan = await this.planManager.FindByIdAsync(planid);
            if (plan == null)
                return HttpNotFound();

            var job = plan.Jobs.FirstOrDefault(j => j.Id == id);
            if (job == null)
                return HttpNotFound();

            job.Name = model.Name;
            job.Description = model.Description;
            job.WorkLocation = model.WorkLocation;
            job.EducationBackgroundRequirement = model.EducationBakcgroundRequirement;
            job.DegreeRequirement = model.DegreeRequirement;
            job.MajorRequirement = model.MajorRequirement;

            await this.planManager.UpdateAsync(plan);

            return RedirectToAction("Detail", new { id = planid });
        }

        public async Task<ActionResult> DeleteJob(int id, int planid)
        {
            var plan = await this.planManager.FindByIdAsync(planid);
            if (plan == null)
                return HttpNotFound();

            var job = plan.Jobs.FirstOrDefault(j => j.Id == id);
            if (job == null)
                return HttpNotFound();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteJob(int id, int planid, FormCollection collection)
        {
            var plan = await this.planManager.FindByIdAsync(planid);
            if (plan == null)
                return HttpNotFound();

            var job = plan.Jobs.FirstOrDefault(j => j.Id == id);
            if (job == null)
                return HttpNotFound();

            plan.Jobs.Remove(job);
            await this.planManager.UpdateAsync(plan);
            return RedirectToAction("Detail", new { id = planid });
        }

        public async Task<ActionResult> Publish(int id)
        {
            var plan = await this.planManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            RecruitmentPlanPublishViewModel model = new RecruitmentPlanPublishViewModel()
            {
                PlanId = plan.Id,
                EnrollExpirationDate = DateTime.Now.AddMonths(1),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Publish(int id, RecruitmentPlanPublishViewModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);
            var plan = await this.planManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            try
            {
                await this.planManager.PublishAsync(plan, model.EnrollExpirationDate);
                return RedirectToAction("Detail", new { id = plan.Id });
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public async Task<ActionResult> CompleteAudit(int id)
        {
            var plan = await this.planManager.FindByIdAsync(id);
            if (plan == null)
            {
                return HttpNotFound();
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CompleteAudit(int id, FormCollection collection)
        {
            if (!this.ModelState.IsValid)
                return View(collection);
            var plan = await this.planManager.FindByIdAsync(id);
            if (plan == null)
                return HttpNotFound();

            //开始提交。
            try
            {
                await this.planManager.CompleteAuditAsync(plan);
                return View("_OperationSuccess");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(collection);
            }
        }
    }
}