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

namespace TalentGoManagerWebApp.Controllers
{
    public class ApplicationFormController : Controller
    {
        ApplicationFormManager applicationFormManager;
        RecruitmentPlanManager recruitmentPlanManager;
        ApplicationUserManager targetUserManager;

        public ApplicationFormController(ApplicationFormManager applicationFormManager, RecruitmentPlanManager recruitmentPlanManager, ApplicationUserManager userManager)
        {
            this.applicationFormManager = applicationFormManager;
            this.recruitmentPlanManager = recruitmentPlanManager;
            this.targetUserManager = userManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PendingFileReviewList()
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
                form = this.applicationFormManager.ApplicationForms.PendingFileReview().OrderBy(a => a.WhenCommited).FirstOrDefault();

            if (form == null)
                return View("NoPendingFileReview");

            return View(form);
        }


        public async Task<ActionResult> FileReview(int? id, bool accepted, bool next)
        {
            ApplicationForm form;
            if (id.HasValue)
            {
                form = this.applicationFormManager.ApplicationForms.PendingFileReview().FirstOrDefault(a => a.Id == id.Value);
                if (form == null)
                    return HttpNotFound();
            }
            else
                form = this.applicationFormManager.ApplicationForms.PendingFileReview().OrderBy(a => a.WhenCommited).FirstOrDefault();

            if (form == null)
                return View("NoPendingFileReview");

            await this.applicationFormManager.FileReviewAsync(form, accepted);

            if (next)
                return RedirectToAction("FileReview");

            return RedirectToAction("FileReviewComplete");
        }

        /// <summary>
        /// 获取审核列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ActionResult> EnrollmentList(int id, EnrollmentListViewModel model)
        {
            var recruitmentPlan = await this.recruitmentPlanManager.FindByIdAsync(id);
            if (recruitmentPlan == null)
                return View("OperationResult", new OperationResult(ResultStatus.Failure, "找不到报名计划。", this.Url.Action("Index", "RecruitmentPlan"), 3));

            if (model == null)
            {
                model = new EnrollmentListViewModel()
                {
                    PageIndex = 0
                };
            }
            model.RecruitmentPlanID = recruitmentPlan.Id;
            model.RecruitmentPlanTitle = recruitmentPlan.Title;
            model.IsAudit = recruitmentPlan.WhenAuditCommited.HasValue;

            model.EnrollmentList = this.applicationFormManager.GetCommitedEnrollmentData(id, model.AuditFilter, model.AnnounceFilter, model.Keywords, model.OrderColumn, model.DownDirection, model.PageIndex, model.PageSize, out int allCount);
            model.AllCount = allCount;
            return View(model);
        }

        public async Task<ActionResult> ExportAuditList(int id, EnrollmentListViewModel model)
        {
            var recruitmentPlan = await this.recruitmentPlanManager.FindByIdAsync(id);
            if (recruitmentPlan == null)
                return View("OperationResult", new OperationResult(ResultStatus.Failure, "找不到报名计划。", this.Url.Action("Index", "RecruitmentPlan"), 3));

            if (model == null)
            {
                model = new EnrollmentListViewModel()
                {
                    PageIndex = 0
                };
            }

            model.EnrollmentList = this.applicationFormManager.GetCommitedEnrollmentData(id, model.AuditFilter, model.AnnounceFilter, model.Keywords, model.OrderColumn, true, out int allCount);
            model.AllCount = allCount;

            MemoryStream ms = new MemoryStream();

            StreamWriter sw = new StreamWriter(ms, Encoding.Unicode);

            //书写标题
            sw.WriteLine("计划\t姓名\t性别\t出生日期\t民族\t籍贯\t现居地\t政治面貌\t健康状况\t婚姻状况\t身份证号\t手机号\t毕业学校\t专业\t毕业年份\t应聘职位\t学历\t学位\t创建日期\t修改日期\t提交日期\t审核日期\t审核通过\t审核消息\t声明日期\t是否参加考试\t证件照ID\t身份证正面\t身份证背面\t准考证号");

            foreach (ApplicationForm data in model.EnrollmentList)
            {
                sw.Write(recruitmentPlan.Title + "\t");
                sw.Write(data.Person.DisplayName + "\t");
                sw.Write(data.Person.Sex + "\t");
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
                sw.Write(data.EducationBackground + "\t");
                sw.Write(data.Degree + "\t");
                sw.Write(data.WhenCreated + "\t");
                sw.Write((data.WhenChanged.HasValue ? data.WhenChanged.Value.ToString() : "N/A") + "\t");
                sw.Write((data.WhenCommited.HasValue ? data.WhenCommited.Value.ToString() : "N/A") + "\t");
                sw.Write((data.WhenAudit.HasValue ? data.WhenAudit.Value.ToString() : "N/A") + "\t");
                sw.Write((data.Approved.HasValue ? (data.Approved.Value ? "是" : "否") : "N/A") + "\t");
                sw.Write((string.IsNullOrEmpty(data.AuditMessage) ? "" : data.AuditMessage) + "\t");
                sw.Write((data.WhenAnnounced.HasValue ? data.WhenAnnounced.Value.ToString() : "N/A") + "\t");
                sw.Write((data.IsTakeExam.HasValue ? (data.IsTakeExam.Value ? "是" : "否") : "N/A") + "\t");

                //证件照、身份证等数据
                sw.Write("\t");
                sw.Write("\t");
                sw.Write("\t");

                //准考证号
                sw.Write("0\r\n");

            }

            sw.Flush();
            ms.Position = 0;

            return File(ms, "text/csv", "审核列表.csv");


            //return File(new byte[0], "text/csv");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="userid"></param>
        /// <param name="Audit"></param>
        /// <returns></returns>
        public async Task<ActionResult> SetAuditFlag(int formId, bool? Audit)
        {
            SetAuditResult result = new SetAuditResult(formId);

            var form = await this.applicationFormManager.FindByIdAsync(formId);
            if (form == null)
            {
                result.Code = 404;
                result.Message = "找不到报名表";
                return Json(result, "text/plain", JsonRequestBehavior.AllowGet);
            }

            await this.applicationFormManager.SetAuditFlagAsync(form, Audit);

            //更新统计
            await this.UpdateStatistics(result);

            return Json(result, "text/plain", JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="planid"></param>
        /// <param name="userid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ActionResult> SetAuditMessage(int formId, string message)
        {
            SetAuditResult result = new SetAuditResult(formId);
            

            var enrollment = await this.applicationFormManager.FindByIdAsync(formId);
            if (enrollment == null)
            {
                result.Code = 404;
                result.Message = "找不到报名表";
                return Json(result, "text/plain", JsonRequestBehavior.AllowGet);
            }

            await this.applicationFormManager.SetAuditMessageAsync(enrollment, message);

            return Json(result, "text/plain", JsonRequestBehavior.AllowGet);
        }

        async Task UpdateStatistics(SetAuditResult result)
        {
            var form = await this.applicationFormManager.FindByIdAsync(result.FormId);
            result.Statistics = await this.GetStatistics(form.Job.PlanId);
        }

        async Task<EnrollmentStatisticsViewModel> GetStatistics(int PlanID)
        {
            var plan = await this.recruitmentPlanManager.FindByIdAsync(PlanID);
            var enrollmentSet = from enroll in this.applicationFormManager.ApplicationForms
                                where enroll.JobId == PlanID && enroll.WhenCommited.HasValue
                                select enroll;

            EnrollmentStatisticsViewModel model = new EnrollmentStatisticsViewModel()
            {
                CommitedEnrollmentCount = enrollmentSet.Count(),
                ApprovedEnrollmentCount = enrollmentSet.Count(e => e.Approved.Value),
                RejectiveEnrollmentCount = enrollmentSet.Count(e => !e.Approved.Value),
                NotAuditEnrollmentCount = enrollmentSet.Count(e => !e.Approved.HasValue && e.WhenCommited.HasValue),
                NotAnnouncedCount = enrollmentSet.Count(e => !e.WhenAnnounced.HasValue && e.Approved.Value),
                AnnouncedTakeExamCount = enrollmentSet.Count(e => e.IsTakeExam.HasValue && e.IsTakeExam.Value),
                AnnouncedNotTakeExamCount = enrollmentSet.Count(e => e.IsTakeExam.HasValue && !e.IsTakeExam.Value)
            };
            return model;
        }


        public async Task<ActionResult> Detail(int id)
        {
            var form = await this.applicationFormManager.FindByIdAsync(id);
            if (form == null)
                return HttpNotFound();

            return View(form);
        }
    }
}