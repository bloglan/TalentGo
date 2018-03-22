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

        /// <summary>
        /// Get application form list.
        /// </summary>
        /// <returns></returns>
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

            //每个招聘计划只允许填写一份报名表。
            if (user.ApplicationForms.Any(a => a.Job.PlanId == job.PlanId))
                return View("OnlyOneFormPerPlanAllowed");


            ChineseIDCardNumber number = ChineseIDCardNumber.Parse(user.IDCardNumber);

            //准备下拉框及相关数据
            this.InitModelSelectionData(job, this.ViewData);

            ApplicationFormEditViewModel model = new ApplicationFormEditViewModel()
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
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Enroll(int id, ApplicationFormEditViewModel model)
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
                SelectedMajor = model.SelectedMajor,
                YearOfGraduated = model.YearOfGraduated,
                EducationalBackground = model.EducationalBackground,
                AcademicCertNumber = model.AcademicCertNumber,
                DegreeCertNumber = model.DegreeCertNumber,
                Degree = model.Degree,
                Resume = model.Resume,
                Accomplishments = model.Accomplishments,
            };

            try
            {
                await this.manager.EnrollAsync(form);
                //转到传送文件。
                return RedirectToAction("Files", new { id = form.Id });
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(model);
                throw;
            }
            
        }

        public ActionResult Edit(int id)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return HttpNotFound();

            this.InitModelSelectionData(form.Job, this.ViewData);

            var model = new ApplicationFormEditViewModel
            {
                Job = form.Job,
                NativePlace = form.NativePlace,
                Source = form.Source,
                PoliticalStatus = form.PoliticalStatus,
                Health = form.Health,
                Marriage = form.Marriage,
                School = form.School,
                Major = form.Major,
                SelectedMajor = form.SelectedMajor,
                YearOfGraduated = form.YearOfGraduated,
                EducationalBackground = form.EducationalBackground,
                AcademicCertNumber = form.AcademicCertNumber,
                Degree = form.Degree,
                DegreeCertNumber = form.DegreeCertNumber,
                Resume = form.Resume,
                Accomplishments = form.Accomplishments,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ApplicationFormEditViewModel model)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return HttpNotFound();

            model.Job = form.Job;
            this.InitModelSelectionData(form.Job, this.ViewData);

            if (!this.ModelState.IsValid)
                return View(model);

            form.NativePlace = model.NativePlace;
            form.Source = model.Source;
            form.PoliticalStatus = model.PoliticalStatus;
            form.Health = model.Health;
            form.Marriage = model.Marriage;
            form.School = model.School;
            form.Major = model.Major;
            form.SelectedMajor = model.SelectedMajor;
            form.YearOfGraduated = model.YearOfGraduated;
            form.EducationalBackground = model.EducationalBackground;
            form.AcademicCertNumber = model.AcademicCertNumber;
            form.Degree = model.Degree;
            form.DegreeCertNumber = model.DegreeCertNumber;
            form.Resume = model.Resume;
            form.Accomplishments = model.Accomplishments;

            await this.manager.ModifyAsync(form);
            return RedirectToAction("Detail", new { id = form.Id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">ApplicationForm Id.</param>
        /// <returns></returns>
        public ActionResult Files(int id)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return HttpNotFound();
            return View(form);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadHeadImage(int id)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return Json(new FileUploadResultModel { Result = 404, Message = "Cannot found application form." });

            if (Request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = this.Request.Files[0];
            using (var st = file.InputStream)
            {
                try
                {
                    await this.manager.UploadHeadImageAsync(form, file.InputStream);

                }
                catch (Exception ex)
                {
                    return Json(new FileUploadResultModel { Result = 500, Message = "File Exists." + ex.Message });
                    throw;
                }
            }

            return Json(new FileUploadResultModel { Result = 0, FileId = form.HeadImageFile });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> RemoveHeadImage(int id)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return Json("Application form not found.");

            await this.manager.RemoveHeadImageAsync(form);
            return Json(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadAcademicCertFile(int id)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return Json(new { result = -1, message = "Application form not found." }, "text/plain");

            try
            {
                var fileId = await this.manager.UploadAcademicCertFileAsync(form, this.Request.Files[0].InputStream);
                return Json(new { result = 0, category = "academiccert", src = Url.Action("Thumbnail", "File", new { id = fileId }), formid = form.Id, fileid = fileId }, "text/plain");
            }
            catch (Exception ex)
            {
                return Json(new { result = -1, message = ex.Message }, "text/plain");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> RemoveAcademicCertFile(int id, string fileId)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return Json(new { result = -1, message = "Application form not found." }, "text/plain");

            try
            {
                await this.manager.RemoveAcademicFileAsync(form, fileId);
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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadDegreeCertFile(int id)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return Json(new { result = -1, message = "Application form not found." }, "text/plain");

            try
            {
                var fileId = await this.manager.UploadDegreeCertFileAsync(form, this.Request.Files[0].InputStream);
                return Json(new { result = 0, category = "degreecert", src = Url.Action("Thumbnail", "File", new { id = fileId }), formid = form.Id, fileid = fileId }, "text/plain");
            }
            catch (Exception ex)
            {
                return Json(new { result = -1, message = ex.Message }, "text/plain");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> RemoveDegreeCertFile(int id, string fileId)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return Json(new { result = -1, message = "Application form not found." }, "text/plain");

            try
            {
                await this.manager.RemoveDegreeCertFileAsync(form, fileId);
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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadOtherFile(int id)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return Json(new { result = -1, message = "Application form not found." }, "text/plain");

            try
            {
                var fileId = await this.manager.UploadOtherFileAsync(form, this.Request.Files[0].InputStream);
                return Json(new { result = 0, category = "other", src = Url.Action("Thumbnail", "File", new { id = fileId }), formid = form.Id, fileid = fileId }, "text/plain");
            }
            catch (Exception ex)
            {
                return Json(new { result = -1, message = ex.Message }, "text/plain");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> RemoveOtherFile(int id, string fileId)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return Json(new { result = -1, message = "Application form not found." }, "text/plain");

            try
            {
                await this.manager.RemoveOtherFileAsync(form, fileId);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// 获取可编辑的报名表。
        /// 如果找不到报名表，返回null，如果找到报名表，但已提交，返回null.
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        ApplicationForm GetEditableFormOfLoginUser(int formId)
        {
            var person = this.CurrentUser();
            var form = person.ApplicationForms.FirstOrDefault(a => a.Id == formId);
            if (form == null)
                return null;

            if (form.WhenCommited.HasValue)
                return null;

            return form;
        }

        /// <summary>
        /// Show Detail of user's application form.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(int id)
        {
            var person = this.CurrentUser();

            var form = person.ApplicationForms.FirstOrDefault(f => f.Id == id);
            if (form == null)
                return HttpNotFound();

            return View(form);
        }

        /// <summary>
        /// Commit application form.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Commit(int id)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return HttpNotFound();

            return View();
        }

        /// <summary>
        /// POST action for commit application form.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Commit(int id, FormCollection collection)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return HttpNotFound();

            await this.manager.CommitAsync(form);
            return View("CommitSuccess", form.Id);
        }


        /// <summary>
        /// Initialize dropdownlist and other data for enroll or edit application form.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="ViewData"></param>
        void InitModelSelectionData(Job job, ViewDataDictionary ViewData)
        {
            //学历选择表
            var eduSet = job.EducationBackgroundRequirement.Trim().Trim('\r', '\n').Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);


            ViewData["EducationBackgroundTable"] = from e in eduSet
                                                   select new SelectListItem() { Text = e, Value = e };

            var degreeSet = job.DegreeRequirement.Trim().Trim('\r', '\n').Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            //学位选择表
            ViewData["DegreeTable"] = from e in degreeSet
                                      select new SelectListItem { Text = e, Value = e };

            //专业选择表
            var majorSet = job.MajorRequirement.Trim().Trim('\r', '\n').Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            ViewData["MajorTable"] = from e in majorSet
                                     select new SelectListItem { Text = e, Value = e };

            //年份选择表
            List < SelectListItem > GraduatedYears = new List<SelectListItem>();
            GraduatedYears.Add(new SelectListItem() { Value = DateTime.Now.Year.ToString(), Text = DateTime.Now.Year.ToString() });
            GraduatedYears.Add(new SelectListItem() { Value = (DateTime.Now.Year - 1).ToString(), Text = (DateTime.Now.Year - 1).ToString() });
            ViewData["GraduatedYears"] = GraduatedYears;
        }

        public ActionResult Delete(int id)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return HttpNotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {
            var form = this.GetEditableFormOfLoginUser(id);
            if (form == null)
                return HttpNotFound();

            await this.manager.DeleteAsync(form);
            return RedirectToAction("Index");
        }
    }
}