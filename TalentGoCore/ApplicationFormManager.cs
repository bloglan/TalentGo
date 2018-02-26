using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalentGo.Linq;
using System.Net.Mail;
using System.Transactions;
using TalentGo.Utilities;

namespace TalentGo
{
    /// <summary>
    /// 表示招聘报名管理器
    /// </summary>
    public class ApplicationFormManager
    {
        RecruitmentPlanManager recruitManager;
        IApplicationFormStore store;

        /// <summary>
        /// 构造函数。使用给定的报名表存储、招聘计划管理器和目标用户管理器初始化报名管理器。
        /// </summary>
        /// <param name="Store"></param>
        /// <param name="recruitmentPlanManager"></param>
		public ApplicationFormManager(IApplicationFormStore Store, RecruitmentPlanManager recruitmentPlanManager)
        {
            this.store = Store;
            this.recruitManager = recruitmentPlanManager;
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<ApplicationForm> ApplicationForms
        {
            get { return this.store.ApplicationForms; }
        }

        /// <summary>
        /// 获取已提交的报名表。
        /// </summary>
        public IQueryable<ApplicationForm> CommitedForms
        {
            get { return this.ApplicationForms.Where(e => e.WhenCommited.HasValue); }
        }

        /// <summary>
        /// 获取隶属于指定招聘计划的报名表。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public IQueryable<ApplicationForm> GetEnrollmentsOfPlan(RecruitmentPlan plan)
        {
            return this.CommitedForms.Where(en => en.JobId == plan.Id);
        }

        /// <summary>
        /// 获取报名对应的上传的文档集合。
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EnrollmentArchive>> GetEnrollmentArchives(ApplicationForm enrollment)
        {
            //获取报名表
            //获取报名表对应的资料
            //
            var enrollmentArchiveStore = this.store as IEnrollmentArchiveStore;
            if (enrollmentArchiveStore == null)
                throw new NotSupportedException("不支持");

            return await enrollmentArchiveStore.GetEnrollmentArchives(enrollment);

        }

        /// <summary>
        /// 根据指定的ArchiveId获取报名表对应的文档。
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EnrollmentArchive FindEnrollmentArchiveByIdAsync(int Id)
        {
            var store = this.store as IEnrollmentArchiveStore;
            if (store == null)
                throw new NotSupportedException();

            return store.EnrollmentArchives.FirstOrDefault(ea => ea.Id == Id);
        }



        #region Operations for enrollment

        /// <summary>
        /// 初始化一个新报名表。
        /// 该操作返回一个初始报名表实例，以便进行填写。
        /// 该操作不会将报名表添加到报名数据库中。若要完成并保存报名表，需要调用CreateEnrollmentAsync方法。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="plan"></param>
        /// <returns></returns>
        public ApplicationForm NewEnrollment(Person user, RecruitmentPlan plan)
        {
            //根据当前需求，不允许存在多个报名表。
            if (this.ApplicationForms.Any(e => e.UserId == user.Id))
                throw new InvalidOperationException("操作失败，指定的用户已存在报名表。");

            ApplicationForm data = new ApplicationForm(plan, user);
            //data.RecruitPlanID = plan.id;
            //data.UserID = user.Id;
            ChineseIDCardNumber cardNumber = ChineseIDCardNumber.CreateNumber(user.IDCardNumber);
            //设置默认值
            data.Name = user.DisplayName;
            //男女
            data.Sex = cardNumber.Gender == Gender.Male ? "男" : "女";
            //出生年月 从IDCardNumber推算
            data.DateOfBirth = cardNumber.DateOfBirth;
            data.IDCardNumber = user.IDCardNumber;
            data.Mobile = user.Mobile;
            data.Resume = "格式：\r\n 高中  1995.07-1998.09  曲靖一中   学生\r\n";
            data.Accomplishments = "";

            return data;
        }

        /// <summary>
        /// 为指定的用户和招聘计划创建报名表。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="plan"></param>
        /// <param name="enrollment"></param>
        /// <returns></returns>
        public async Task CreateEnrollment(Person user, RecruitmentPlan plan, ApplicationForm enrollment)
        {
            //根据要求，一个用户只能参与一个报名。
            if (this.ApplicationForms.Any(e => e.UserId == user.Id))
                throw new InvalidOperationException("操作失败，每个用户只能创建一个报名表。");

            await this.store.CreateAsync(enrollment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="plan"></param>
        /// <param name="enrollment"></param>
        /// <returns></returns>
        public async Task UpdateEnrollment(Person user, RecruitmentPlan plan, ApplicationForm enrollment)
        {
            await this.store.UpdateAsync(enrollment);
        }

        /// <summary>
        /// 删除报名表。
        /// </summary>
        /// <param name="enrollment"></param>
        /// <returns></returns>
        public async Task DeleteEnrollment(ApplicationForm enrollment)
        {
            //如果该报名表已经提交，则不能删除。
            if (enrollment.WhenCommited.HasValue)
                throw new InvalidOperationException("操作失败，已提交的报名表不能删除。");

            await this.store.DeleteAsync(enrollment);
        }

        /// <summary>
        /// 提交报名资料。
        /// </summary>
        /// <returns></returns>
        public async Task CommitEnrollment(Person user, RecruitmentPlan plan, ApplicationForm enrollment)
        {
            //提交报名资料时，对报名资料以及关联的图片文件资料进行检查。
            //提交后不能反向提交。
            //
            if (enrollment.WhenCommited.HasValue)
                throw new InvalidOperationException("报名资料已处于提交状态，不能重复提交。");

            //检查提交文档的符合性。
            //ArchiveCategoryManager archiveMgr = new ArchiveCategoryManager(this.context);

            var archiveReqs = await recruitManager.GetArchiveRequirements(plan);
            var archives = await this.GetEnrollmentArchives(enrollment);
            List<string> failMsg = new List<string>();
            foreach (ArchiveRequirement req in archiveReqs)
            {
                //获得需求标记
                RequirementType reqflag;
                Enum.TryParse<RequirementType>(req.Requirements, out reqflag);

                //查询指定需求的文档
                var result = from arch in archives
                             where arch.ArchiveCategoryID == req.ArchiveCategoryID
                             select arch;

                if (reqflag.IsRequried() && result.Count() == 0)
                {
                    failMsg.Add(string.Format("{0}是需要的，但未提供。", req.ArchiveCategory.Name));
                }
            }

            if (failMsg.Count != 0)
                throw new CommitEnrollmentException("上传的文档不符合要求。", failMsg);
            //检查报名表及关联报名资料是否合格，若不合格，则提示错误。
            //检查报名截止时间，如果该提交在报名截止时间之后，则直接提示未通过。

            if (plan.EnrollExpirationDate < DateTime.Now)
                throw new InvalidOperationException("报名截止时间已过。");

            enrollment.Commit();

            await this.UpdateEnrollment(user, plan, enrollment);
        }

        /// <summary>
        /// 完成审核。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task CompleteAudit(RecruitmentPlan plan)
        {
            if (plan == null)
                throw new ArgumentNullException(nameof(plan));

            if (plan.WhenAuditCommited.HasValue)
                return;

            var enrollments = this.GetEnrollmentsOfPlan(plan);
            if (enrollments.Any(e => !e.Approved.HasValue))
                throw new InvalidOperationException("操作失败，还有未设置审核标记的报名表。");

            //if (AnnounceExamExpirationDate < DateTime.Now)
            //    throw new ArgumentException("声明参考的截止日期不能晚于当前日期。");

            if (DateTime.Now < plan.EnrollExpirationDate)
                throw new ArgumentException("审核的提交早于报名截止日期。");

            //if (AnnounceExamExpirationDate < currentplan.EnrollExpirationDate)
            //    throw new ArgumentException("声明参考的截止日期不能早于报名截止日期。");



            //检查与此招聘计划关联的报名表。
            //如果报名表未提交，则直接设置不通过，附加说明为未在报名截止日期内提交。
            //如果审核状态Approved值未设置，则回退，报告人力资源管理员必须为已提交的报名表设置审核状态。
            //如果以上检查都符合条件，为Plan设置提交日期，为每份报名表设置提交日期，并将审核结果提交短信发送队列进行发送。
            //
            //检查提交日期在规定截止日期内的，尚未设置Approved的报名表。若存在，则审核提交操作无效。
            //var checkSet = from enroll in currentplan.EnrollmentData
            //               where enroll.WhenCommited < currentplan.EnrollExpirationDate && !enroll.Approved.HasValue
            //               select enroll;
            //if (checkSet.Any())
            //    throw new InvalidOperationException("还有未设定审核结果的报名表。请全部设定后，再进行提交。");

            //设置声明考试的截止日期。
            //currentplan.AnnounceExpirationDate = AnnounceExamExpirationDate;


            using (TransactionScope transScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (ApplicationForm data in enrollments)
                {
                    //若没有提交，或提交日期晚于报名截止日期的，直接设定为不通过。
                    if (!data.WhenCommited.HasValue || data.WhenCommited > plan.EnrollExpirationDate)
                    {
                        data.Refuse();
                        data.SetAuditMessage("未在指定的报名截止时间内提交");
                    }

                    data.CompleteAudit();

                    //提交发送短信
                    //string smsMsg;
                    //if (data.Approved.Value)
                    //    smsMsg = string.Format(smsApprovedMsg, data.Name, plan.Title, plan.AnnounceExpirationDate.Value.ToString("yyyy-MM-dd HH:mm"));
                    //else
                    //    smsMsg = string.Format(smsRejectiveMsg, data.Name, plan.Title, plan.AnnounceExpirationDate.Value.ToString("yyyy-MM-dd HH:mm"));

                    //await smsClient.SendMessageAsync(new string[] { data.Mobile }, smsMsg, new SMSSvc.SendMessageOption());

                    //提交发送邮件
                    //if (data.Users.EmailValid)
                    //{
                    //	MailMessage mail = new MailMessage();
                    //	mail.From = new MailAddress("job@qjyc.cn", "曲靖烟草招聘");
                    //	mail.To.Add(new MailAddress(data.Users.Email, data.Name));
                    //	mail.Subject = "曲靖烟草招聘报名审核通知";
                    //	mail.Body = smsMsg;
                    //	await smtpClient.SendMailAsync(mail);
                    //	await Task.Delay(800);
                    //}
                    await this.UpdateEnrollment(null, plan, data);
                }


                //每项提交完成后，修改currentPlan的标记，表示已提交审核。
                plan.CompleteAudit();

                //设定考试时间和地点
                //currentplan.ExamStartTime = ExamStartTime;
                //currentplan.ExamEndTime = ExamEndTime;
                //currentplan.ExamLocation = ExamLocation;

                await this.recruitManager.UpdateRecruitmentPlan(plan);

                transScope.Complete();
            }

            //开始每项提交并发送短信。

        }
        //string smsRejectiveMsg = "{0}，您好，您所填报的{1}报名资料，经初审未通过，感谢您的参与！";
        //string smsApprovedMsg = "{0}，您好，您所填报的{1}报名资料，经初审通过，请于{2}前登陆网站声明是否参加考试，逾期未声明是否参加考试的不予参加考试。谢谢您的合作。";


        /// <summary>
        /// 声明是否参加考试。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="plan"></param>
        /// <param name="IsTakeExam"></param>
        /// <returns></returns>
		public async Task AnnounceForExam(Person user, RecruitmentPlan plan, bool IsTakeExam)
        {
            ApplicationForm data = this.CommitedForms.First(e => e.UserId == user.Id && e.JobId == plan.Id);

            //必须是已提交的，通过审核的，尚未声明的，当前在声明有效期内的。
            if (!data.WhenAudit.HasValue)
                throw new InvalidOperationException("无效的操作，尚未审核。");

            if (!data.Approved.Value)
                throw new InvalidOperationException("无效的操作，审核未通过。");

            if (data.WhenAnnounced.HasValue)
                throw new InvalidOperationException("无效的操作，已进行了声明。不能重复声明。");

            if (data.Job.AnnounceExpirationDate.Value < DateTime.Now)
                throw new InvalidOperationException("无效的操作，已过声明截止时间。");

            data.Announce(IsTakeExam);

            await this.store.UpdateAsync(data);
        }


        #endregion

        #region Method for enrollment archives

        /// <summary>
        /// 为报名表添加文档。
        /// </summary>
        /// <param name="enrollment"></param>
        /// <param name="archive"></param>
        /// <returns></returns>
        public async Task AddEnrollmentArchive(ApplicationForm enrollment, EnrollmentArchive archive)
        {
            var store = this.store as IEnrollmentArchiveStore;
            if (store == null)
                throw new NotSupportedException();

            await store.AddArchiveToEnrollment(enrollment, archive);
        }

        /// <summary>
        /// 为报名表移除文档。
        /// </summary>
        /// <param name="enrollment"></param>
        /// <param name="archive"></param>
        /// <returns></returns>
        public async Task RemoveEnrollmentArchive(ApplicationForm enrollment, EnrollmentArchive archive)
        {
            var store = this.store as IEnrollmentArchiveStore;
            if (store == null)
                throw new NotSupportedException();

            await store.RemoveArchiveFromEnrollment(enrollment, archive);
        }

        #endregion







        #region 管理板块的方法

        /// <summary>
        /// 根据关键字、标记、获取满足条件的报名表。
        /// </summary>
        /// <param name="PlanID">指定的招聘计划。</param>
        /// <param name="MajorCategory">指定的专业大类，若未指定，则获取全部。</param>
        /// <param name="AuditFilter">审核标记，null表示未审核，通过标记为true，false标记为未通过</param>
        /// <param name="AnounceFilter"></param>
        /// <param name="Keywords">关键字，若提供，可对姓名、身份证号码、移动电话号码、籍贯、生源地、学校、填写专业字段进行匹配搜索。否则查询全部。</param>
        /// <returns></returns>
        public IQueryable<ApplicationForm> GetCommitedEnrollmentData(int PlanID, string MajorCategory, AuditFilterType AuditFilter, AnnounceFilterType AnounceFilter, string Keywords)
        {
            //带分页
            //
            //先获得符合初始条件的集合
            var initSet = this.CommitedForms.Where(e => e.JobId == PlanID);

            //根据条件过滤

            if (!string.IsNullOrEmpty(MajorCategory))
                initSet = initSet.Where(e => e.SelectedMajor == MajorCategory);

            if (!string.IsNullOrEmpty(Keywords))
                initSet = initSet.Where(e =>
                    e.Name.StartsWith(Keywords) ||
                    e.IDCardNumber.StartsWith(Keywords) ||
                    e.Mobile.StartsWith(Keywords) ||
                    e.PlaceOfBirth.StartsWith(Keywords) ||
                    e.School.StartsWith(Keywords) ||
                    e.Major.StartsWith(Keywords)
                );

            switch (AuditFilter)
            {
                case AuditFilterType.All:
                    //DoNothing
                    break;
                case AuditFilterType.Approved:
                    initSet = initSet.Where(e => e.Approved.HasValue && e.Approved.Value);
                    break;
                case AuditFilterType.Rejective:
                    initSet = initSet.Where(e => e.Approved.HasValue && !e.Approved.Value);
                    break;
                case AuditFilterType.NotSet:
                    initSet = initSet.Where(e => !e.Approved.HasValue);
                    break;
            }

            switch (AnounceFilter)
            {
                case AnnounceFilterType.All:
                    //Do Nothing
                    break;
                case AnnounceFilterType.TakeExam:
                    initSet = initSet.Where(e => e.IsTakeExam.HasValue && e.IsTakeExam.Value);
                    break;
                case AnnounceFilterType.NotTakeExam:
                    initSet = initSet.Where(e => e.IsTakeExam.HasValue && !e.IsTakeExam.Value);
                    break;
                case AnnounceFilterType.NotAnnounced:
                    initSet = initSet.Where(e => !e.IsTakeExam.HasValue);
                    break;
            }

            return initSet;

        }

        /// <summary>
        /// 根据关键字、标记、排序指示获取满足条件的报名表。
        /// </summary>
        /// <param name="PlanID"></param>
        /// <param name="MajorCategory"></param>
        /// <param name="AuditFilter"></param>
        /// <param name="AnnounceFilter"></param>
        /// <param name="Keywords"></param>
        /// <param name="OrderColumn"></param>
        /// <param name="DownDirection"></param>
        /// <param name="ItemCount"></param>
        /// <returns></returns>
		public IQueryable<ApplicationForm> GetCommitedEnrollmentData(int PlanID, string MajorCategory, AuditFilterType AuditFilter, AnnounceFilterType AnnounceFilter, string Keywords, string OrderColumn, bool DownDirection, out int ItemCount)
        {
            var resultSet = this.GetCommitedEnrollmentData(PlanID, MajorCategory, AuditFilter, AnnounceFilter, Keywords);

            ItemCount = resultSet.Count();
            if (ItemCount == 0)
                return resultSet;

            //按字段排序
            if (string.IsNullOrEmpty(OrderColumn))
                OrderColumn = "WhenCommited";

            IOrderedQueryable<ApplicationForm> OrderedSet;
            if (DownDirection)
                OrderedSet = resultSet.OrderByDescending(OrderColumn);
            else
                OrderedSet = resultSet.OrderBy(OrderColumn);

            return OrderedSet;
        }

        /// <summary>
        /// 根据关键字、标记、排序指示和分页参数获取满足条件的指定页的报名表。
        /// </summary>
        /// <param name="PlanID"></param>
        /// <param name="MajorCategory"></param>
        /// <param name="AuditFilter"></param>
        /// <param name="AnnounceFilter"></param>
        /// <param name="Keywords"></param>
        /// <param name="OrderColumn"></param>
        /// <param name="DownDirection"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="ItemCount"></param>
        /// <returns></returns>
        public IQueryable<ApplicationForm> GetCommitedEnrollmentData(int PlanID, string MajorCategory, AuditFilterType AuditFilter, AnnounceFilterType AnnounceFilter, string Keywords, string OrderColumn, bool DownDirection, int PageIndex, int PageSize, out int ItemCount)
        {
            var result = this.GetCommitedEnrollmentData(PlanID, MajorCategory, AuditFilter, AnnounceFilter, Keywords, OrderColumn, DownDirection, out ItemCount);
            if (ItemCount == 0)
            {
                return result;
            }

            //检查PageIndex和PageSize是否符合要求
            if (PageSize <= -1)
                PageSize = int.MaxValue;
            if (PageSize >= 0 && PageSize < 5)
                PageSize = 5;

            int PageCount = (int)Math.Ceiling((double)ItemCount / (double)PageSize);

            if (PageIndex < 0)
                PageIndex = 0;
            if (PageIndex >= PageCount)
                PageIndex = PageCount - 1;

            //返回指定分页的条目
            return result.Skip(PageIndex * PageSize).Take(PageSize);
        }

        #endregion

    }
}
