using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TalentGo;
using TalentGo.Models;
using TalentGo.Web;
using System;
using TalentGoManagerWebApp.Models;
using System.Linq.Dynamic;

namespace TalentGoManagerWebApp.Controllers
{
    public class ApplicationFormController : Controller
    {
        ApplicationFormManager applicationFormManager;
        RecruitmentPlanManager recruitmentPlanManager;

        public ApplicationFormController(ApplicationFormManager applicationFormManager, RecruitmentPlanManager recruitmentPlanManager)
        {
            this.applicationFormManager = applicationFormManager;
            this.recruitmentPlanManager = recruitmentPlanManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult FileReviewOperationPanel()
        {
            this.ViewBag.PendingFileReviewCount = this.applicationFormManager.ApplicationForms.PendingFileReview().Count();
            return PartialView("_FileReviewOperationPanel");
        }


        public ActionResult PendingFileReview()
        {
            return View(this.applicationFormManager.ApplicationForms.PendingFileReview().OrderByDescending(a => a.WhenCommited));
        }

        public ActionResult FileReview(int? id)
        {
            ApplicationForm form;
            if (id.HasValue)
            {
                form = this.applicationFormManager.ApplicationForms.PendingFileReview().FirstOrDefault(a => a.Id == id.Value);
                if (form == null)
                    return HttpNotFound();
            }
            else
            {
                var forms = this.applicationFormManager.ApplicationForms.PendingFileReview().OrderBy(a => a.WhenCommited);
                var pendingCount = forms.Count();
                if (pendingCount == 0)
                    return View("NoPendingFileReview");
                if (pendingCount > 10)
                    pendingCount = 10;
                form = forms.Skip(new Random().Next(pendingCount)).FirstOrDefault();
            }

            if (form == null)
                return View("NoPendingFileReview");

            var model = new ReviewFileModel
            {
                FormId = form.Id,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> FileReview(int? id, ReviewFileModel model)
        {
            var form = await this.applicationFormManager.FindByIdAsync(model.FormId);
            if (form == null)
                return View("NoPendingFileReview");

            try
            {
                await this.applicationFormManager.FileReviewAsync(form, model.Accepted, this.DomainUser().DisplayName, model.FileReviewMessage);
                if (!model.Accepted)
                {
                    //退回报名表。
                    if (model.ReturnBackToUserIfRefused)
                        await this.applicationFormManager.ReturnBackAsync(form);
                }
            }
            catch (FileReviewException)
            {
                //审查异常，表明可能已经被审查。
                //DoNothing here.
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            if (model.Next)
                return RedirectToAction("FileReview", routeValues: null);

            return View("FileReviewComplete");
        }

        [ChildActionOnly]
        public async Task<ActionResult> FileReviewPart(int id)
        {
            var form = await this.applicationFormManager.FindByIdAsync(id);
            return PartialView("_FileReviewPart", form);
        }

        [ChildActionOnly]
        public ActionResult AuditOperationPanel()
        {
            this.ViewBag.PendingAuditCount = this.applicationFormManager.ApplicationForms.Auditable().Count();
            return PartialView("_AuditOperationPanel");
        }

        public ActionResult Search(string q)
        {
            if (string.IsNullOrEmpty(q))
                return View();

            IQueryable<ApplicationForm> forms;

            if (int.TryParse(q, out int intQ))
                forms = this.applicationFormManager.ApplicationForms.Commited().Where(f => f.Id == intQ);
            else
                forms = this.applicationFormManager.ApplicationForms.Commited().Where(f => f.Person.DisplayName.StartsWith(q));

            if (forms.Count() == 1)
            {
                var form = forms.First();
                return RedirectToAction("Detail", new { id = form.Id });
            }
            return View(forms);
        }

        public async Task<ActionResult> Detail(int id)
        {
            var form = await this.applicationFormManager.FindByIdAsync(id);
            if (form == null || !form.WhenCommited.HasValue)
                return HttpNotFound();

            return View(form);
        }


        /// <summary>
        /// 获取审核列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult AuditList(int? plan, string q, bool? audit, string order, int? page)
        {
            var forms = this.applicationFormManager.ApplicationForms.Auditable();
            if (plan.HasValue)
                forms = forms.OfPlan(plan.Value);

            if (!string.IsNullOrEmpty(q))
            {
                if (int.TryParse(q, out int intKey))
                    forms = forms.Where(f => f.Id == intKey);
                else
                    forms = forms.Where(f => f.Person.DisplayName.StartsWith(q)
                    || f.Person.IDCardNumber.StartsWith(q)
                    || f.Person.Mobile.StartsWith(q)
                    || f.School.StartsWith(q)
                    || f.Major.StartsWith(q)
                    || f.SelectedMajor.StartsWith(q)
                    || f.Tags.Contains(q));
            }
            if (audit.HasValue)
            {
                forms = forms.Where(f => f.AuditFlag == audit.Value);
            }

            var count = forms.Count();
            this.ViewBag.AllCount = count;

            if (!string.IsNullOrEmpty(order))
                forms = forms.OrderBy(order);
            else
                forms = forms.OrderByDescending(f => f.WhenFileReviewed);

            var pageIndex = page ?? 0;
            if (count > 0 && count <= pageIndex * 30) //纠正因搜索结果导致页码超范围
                pageIndex = (int)Math.Ceiling((double)count / 30) - 1;
            return View(forms.Skip(pageIndex * 30).Take(30));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">planid</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ActionResult> Export(int? plan, string q, bool? audit, string order)
        {
            var forms = this.applicationFormManager.ApplicationForms.Auditable();
            if (plan.HasValue)
                forms = forms.OfPlan(plan.Value);

            if (!string.IsNullOrEmpty(q))
            {
                if (int.TryParse(q, out int intKey))
                    forms = forms.Where(f => f.Id == intKey);
                else
                    forms = forms.Where(f => f.Person.DisplayName.StartsWith(q)
                    || f.Person.IDCardNumber.StartsWith(q)
                    || f.Person.Mobile.StartsWith(q)
                    || f.School.StartsWith(q)
                    || f.Major.StartsWith(q)
                    || f.SelectedMajor.StartsWith(q)
                    || f.Tags.Contains(q));
            }
            if (audit.HasValue)
            {
                forms = forms.Where(f => f.AuditFlag == audit.Value);
            }

            var count = forms.Count();
            this.ViewBag.AllCount = count;

            if (!string.IsNullOrEmpty(order))
                forms = forms.OrderBy(order);
            else
                forms = forms.OrderByDescending(f => f.WhenFileReviewed);


            MemoryStream ms = new MemoryStream();

            StreamWriter sw = new StreamWriter(ms, Encoding.Unicode);

            //书写标题
            sw.WriteLine("计划\t姓名\t性别\t出生日期\t民族\t籍贯\t现居地\t政治面貌\t健康状况\t婚姻状况\t身份证号\t手机号\t毕业学校\t专业\t毕业年份\t应聘职位\t学历\t学位\t创建日期\t修改日期\t提交日期\t审核通过\t审核消息");

            foreach (ApplicationForm data in forms)
            {
                sw.Write(data.Job.Plan.Title + "\t");
                sw.Write(data.Person.DisplayName + "\t");
                sw.Write((data.Person.Sex == Sex.Male ? "男" : "女") + "\t");
                sw.Write(data.Person.DateOfBirth.ToShortDateString() + "\t");
                sw.Write(data.Person.Ethnicity + "\t");
                sw.Write(data.NativePlace + "\t");
                sw.Write(data.Source + "\t");
                sw.Write(data.PoliticalStatus + "\t");
                sw.Write(data.Health + "\t");
                sw.Write(data.Marriage + "\t");
                sw.Write("=\"" + data.Person.IDCardNumber + "\"" + "\t");
                sw.Write("=\"" + data.Person.Mobile + "\"" + "\t");
                sw.Write(data.School + "\t");
                sw.Write(data.Major + "\t");
                sw.Write(data.YearOfGraduated.ToString() + "\t");
                sw.Write(data.Job.Name + "\t");
                sw.Write(data.EducationalBackground + "\t");
                sw.Write(data.Degree + "\t");
                sw.Write(data.WhenCreated + "\t");
                sw.Write((data.WhenChanged.HasValue ? data.WhenChanged.Value.ToString() : "N/A") + "\t");
                sw.Write((data.WhenCommited.HasValue ? data.WhenCommited.Value.ToString() : "N/A") + "\t");
                sw.Write((data.AuditFlag ? "是" : "否") + "\t");
                sw.Write((string.IsNullOrEmpty(data.AuditMessage) ? "" : data.AuditMessage) + "\r\n");
            }

            sw.Flush();
            ms.Position = 0;

            return File(ms, "text/csv", "报名表.csv");


            //return File(new byte[0], "text/csv");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="userid"></param>
        /// <param name="audit"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SetAuditFlag(int formId, bool audit)
        {
            var form = await this.applicationFormManager.FindByIdAsync(formId);
            if (form == null)
            {
                return Json("找不到报名表");
            }

            try
            {
                await this.applicationFormManager.AuditAsync(form, audit, null, this.DomainUser().DisplayName);

                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SetTags(int formId, string tags)
        {
            var form = await this.applicationFormManager.FindByIdAsync(formId);
            if (form == null)
            {
                return Json("找不到报名表");
            }

            form.Tags = string.IsNullOrEmpty(tags) ? null : tags;
            try
            {
                await this.applicationFormManager.UpdateAsync(form);
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
        /// <param name="id">application form id.</param>
        /// <returns></returns>
        public async Task<ActionResult> ReturnBack(int id)
        {
            var form = await this.applicationFormManager.FindByIdAsync(id);
            if (form == null)
                return HttpNotFound();
            if (!form.WhenCommited.HasValue)
                return HttpNotFound();
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ReturnBack(int id, FormCollection collection)
        {
            if (!this.ModelState.IsValid)
                return View();

            var form = await this.applicationFormManager.FindByIdAsync(id);
            if (form == null)
                return HttpNotFound();
            if (!form.WhenCommited.HasValue)
                return HttpNotFound();

            try
            {
                await this.applicationFormManager.ReturnBackAsync(form);
                return View("ReturnBackSuccess");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    }
}