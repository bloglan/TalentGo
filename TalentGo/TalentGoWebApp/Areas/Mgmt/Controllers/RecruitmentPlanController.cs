using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TalentGo.Recruitment;
using TalentGo.ViewModels;
using TalentGoWebApp.Areas.Mgmt.Models;

namespace TalentGoWebApp.Areas.Mgmt.Controllers
{
    [Authorize(Roles = "QJYC\\招聘管理员,QJYC\\招聘监督人")]
    public class RecruitmentPlanController : Controller
    {
        RecruitmentPlanManager recruitmentManager;
        ArchiveCategoryManager archiveCategoryManager;

        public RecruitmentPlanController(RecruitmentPlanManager recruitmentManager, ArchiveCategoryManager archiveCategoryManager)
        {
            this.recruitmentManager = recruitmentManager;
            this.archiveCategoryManager = archiveCategoryManager;
        }


        // GET: Mgmt/RecruitmentPlan
        public async Task<ActionResult> Index(int? id)
        {
            //id: Year of recruitment plan.
            var plans = this.recruitmentManager.AllRecruitmentPlans;
            if (id.HasValue)
            {
                return View(plans.Where(p => p.Year == id.Value));
            }
            return View(plans);
        }

        /// <summary>
        /// 查看详情。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Detail(int? id)
        {
            if (!id.HasValue)
                return HttpNotFound();

            return View(await this.recruitmentManager.FindByIDAsync(id.Value));
        }

        public ActionResult Create()
        {
            return View(new RecruitmentPlanPrimaryViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> Create(RecruitmentPlanPrimaryViewModel model)
        {
            if (ModelState.IsValid)
            {
                RecruitmentPlan newplan = new RecruitmentPlan()
                {
                    Title = model.Title,
                    Recruitment = model.Recruitment,
                    IsPublic = model.IsPublic,
                    ExpirationDate = model.ExpirationDate,
                    Publisher = model.Publisher
                };

                await this.recruitmentManager.CreateRecruitmentPlan(newplan);
                return RedirectToAction("ArchiveRequirements", new { id = newplan.id });
            }
            return View(model);

        }

        public async Task<ActionResult> Delete(int id)
        {

            var current = await this.recruitmentManager.FindByIDAsync(id);
            if (current == null)
            {
                return RedirectToAction("Index");
            }

            await this.recruitmentManager.DeleteRecruitmentPlan(current);
            return RedirectToAction("Index");

        }

        public async Task<ActionResult> Edit(int id)
        {
            RecruitmentPlan plan = await this.recruitmentManager.FindByIDAsync(id);
            if (plan == null)
                return HttpNotFound();

            RecruitmentPlanPrimaryViewModel vmodel = new RecruitmentPlanPrimaryViewModel()
            {
                Title = plan.Title,
                Recruitment = plan.Recruitment,
                ExpirationDate = plan.ExpirationDate,
                IsPublic = plan.IsPublic,
                Publisher = plan.Publisher
            };
            return View(vmodel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, RecruitmentPlanPrimaryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var plan = await this.recruitmentManager.FindByIDAsync(id);
                plan.Title = model.Title;
                plan.Recruitment = model.Recruitment;
                plan.ExpirationDate = model.ExpirationDate;
                plan.IsPublic = model.IsPublic;
                plan.Publisher = model.Publisher;


                await this.recruitmentManager.UpdateRecruitmentPlan(plan);
                return RedirectToAction("ArchiveRequirements", new { id = id });
            }
            return View(model);
        }

        public async Task<ActionResult> ArchiveRequirements(int id)
        {
            //构建ArchiveRequirementsViewModel的列表
            var plan = await this.recruitmentManager.FindByIDAsync(id);
            var planreqs = await this.recruitmentManager.GetArchiveRequirements(plan);
            var allarch = this.archiveCategoryManager.ArchiveCategories;


            var modelset = from arch in allarch
                           select new ArchiveRequirementsViewModel()
                           {
                               ArchiveCategory = arch,
                               Enabled = planreqs.Any(pr => pr.ArchiveCategoryID == arch.id),
                               RequirementType = planreqs.Any(pr => pr.ArchiveCategoryID == arch.id) ? planreqs.First(pr => pr.ArchiveCategoryID == arch.id).Requirements : "One"
                           };

            //    var archreqSet = from arch in this.database.ArchiveCategory
            //join archreq in this.database.ArchiveRequirements on new { archid = arch.id, planid = id } equals new { archid = archreq.ArchiveCategoryID, planid = archreq.RecruitmentPlanID } into tmp
            //from mm in tmp.DefaultIfEmpty()
            //select new ArchiveRequirementsViewModel()
            //{
            // ArchiveCategory = arch,
            // Enabled = mm != null,
            // RequirementType = mm == null ? "One" : mm.Requirements
            //};

            //构建下拉列表框
            //ViewData["ArchiveReqTypeTable"] = Enum.GetNames(typeof(RequirementType));
            List<SelectListItem> reqs = new List<SelectListItem>();
            reqs.Add(new SelectListItem()
            {
                Text = "一个",
                Value = RequirementType.One.ToString()
            });
            reqs.Add(new SelectListItem()
            {
                Text = "一个或多个",
                Value = RequirementType.OneOrMore.ToString()
            });
            reqs.Add(new SelectListItem()
            {
                Text = "零个或多个",
                Value = RequirementType.ZeroOrMore.ToString()
            });
            reqs.Add(new SelectListItem()
            {
                Text = "零个或一个",
                Value = RequirementType.ZeroOrOne.ToString()
            });
            ViewData["ArchiveReqTypeTable"] = reqs;

            return View(modelset);
        }

        [HttpPost]
        public async Task<ActionResult> ArchiveRequirements(int id, List<ArchiveRequirementsViewModel> model)
        {
            //顺次轮询，
            var plan = await this.recruitmentManager.FindByIDAsync(id);
            var archreqSet = await this.recruitmentManager.GetArchiveRequirements(plan);

            //var archreqSet = from archreq in this.database.ArchiveRequirements
            //				 where archreq.RecruitmentPlanID == id
            //				 select archreq;

            List<ArchiveRequirements> orginialReqList = new List<ArchiveRequirements>(archreqSet);

            foreach (ArchiveRequirementsViewModel item in model)
            {
                ///顺次轮询返回的需求设定，
                ///如果原始表中未发现项，则新建。
                ///若发现项，但需求有变，则更新。
                ///
                var orgselected = orginialReqList.Find(m => m.ArchiveCategoryID == item.ArchiveCategory.id);
                if (item.Enabled)
                {
                    //如果一项被启用，则从原始表查找，若没有发现，则新建，若存在，则更新。
                    if (orgselected == null)
                        await this.recruitmentManager.AddArchiveRequirement(plan, new ArchiveRequirements()
                        {
                            ArchiveCategoryID = item.ArchiveCategory.id,
                            RecruitmentPlanID = id,
                            Requirements = item.RequirementType
                        });
                    else
                    {
                        orgselected.Requirements = item.RequirementType;
                        await this.recruitmentManager.UpdateArchiveRequirement(plan, orgselected);
                    }
                }
                else
                {
                    //如果一项被禁用，则从原始表查找，若发现，则删除。
                    if (orgselected != null)
                        await this.recruitmentManager.RemoveArchiveRequirement(plan, orgselected);
                }

            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Publish(int id)
        {
            PublishRecruitmentPlanViewModel model = new PublishRecruitmentPlanViewModel()
            {
                Plan = await this.recruitmentManager.FindByIDAsync(id)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Publish(int id, PublishRecruitmentPlanViewModel model)
        {
            await this.recruitmentManager.PublishRecruitmentPlan(id, model.EnrollExpirationDate);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CommitAudit(int id)
        {
            var plan = await this.recruitmentManager.FindByIDAsync(id);
            if (plan == null)
            {
                return View("OperationResult", new OperationResult(ResultStatus.Failure, "找不到计划。", this.Url.Action("Index"), 3));
            }
            // bool isPass = plan.EnrollExpirationDate < DateTime.Now ? true : false;
            if (!(plan.EnrollExpirationDate < DateTime.Now))
            {
                return View();
            }
            DateTime examSampleTime = DateTime.Now.AddDays(15); //15天后考试
            CommitAuditViewModel model = new CommitAuditViewModel()
            {
                Plan = plan,
                AnnounceExpirationDate = DateTime.Now.AddDays(7),
                ExamStartTime = new DateTime(examSampleTime.Year, examSampleTime.Month, examSampleTime.Day, 9, 0, 0),
                ExamEndTime = new DateTime(examSampleTime.Year, examSampleTime.Month, examSampleTime.Day, 11, 0, 0),
                ExamLocation = "云南省烟草公司曲靖市公司"
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CommitAudit(int id, CommitAuditViewModel model)
        {
            var plan = await this.recruitmentManager.FindByIDAsync(id);
            if (plan == null)
            {
                return View("OperationResult", new OperationResult(ResultStatus.Failure, "找不到计划。", this.Url.Action("Index"), 3));
            }
            model.Plan = plan;
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            //验证合规性。
            List<string> Errors = new List<string>();
            //考试结束时间应大于考试开始时间，
            if (model.ExamEndTime <= model.ExamStartTime)
                Errors.Add("考试结束时间不能早于开始时间。");
            if (model.ExamStartTime <= model.AnnounceExpirationDate.AddDays(1))
                Errors.Add("考试开始时间不能早于声明参加考试截止时间的次日");
            if (model.AnnounceExpirationDate <= DateTime.Now.AddDays(1))
                Errors.Add("声明参加考试截止日期至少需要1天以上。");

            if (Errors.Count != 0)
            {
                foreach (string err in Errors)
                {
                    this.ModelState.AddModelError("", err);
                }
                return View(model);
            }

            //开始提交。
            try
            {
                await this.recruitmentManager.CommitAudit(plan, model.AnnounceExpirationDate, model.ExamStartTime, model.ExamEndTime, model.ExamLocation);
                //return RedirectToAction("Index");
                return View("OperationResult", new OperationResult(ResultStatus.Success, "该计划已成功结束审核，审核结果将自动通过短信和邮件顺次通知应聘者本人。接下来，应聘者将提交是否参加考试的声明。在声明截止时间过后，您将能获得本次招聘计划参加考试的人员名单及统计信息。", this.Url.Action("Index"), 20));
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        #region ChildActions

        [ChildActionOnly]
        public ActionResult SmartStatistics(TalentGo.Recruitment.RecruitmentPlan plan)
        {
            EnrollmentStatisticsViewModel model = new EnrollmentStatisticsViewModel()
            {
                CommitedEnrollmentCount = plan.EnrollmentData.Count(e => e.WhenCommited.HasValue),
                ApprovedEnrollmentCount = plan.EnrollmentData.Count(e => e.Approved.HasValue && e.Approved.Value),
                AnnouncedTakeExamCount = plan.EnrollmentData.Count(e => e.IsTakeExam.HasValue && e.IsTakeExam.Value)
            };
            return PartialView(model);
        }

        #endregion
    }
}