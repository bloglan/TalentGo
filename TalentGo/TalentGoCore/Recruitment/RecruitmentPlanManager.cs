using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    /// <summary>
    /// 招聘计划管理器
    /// </summary>
    public class RecruitmentPlanManager
    {
        IRecruitmentPlanStore store;

        public RecruitmentPlanManager(IRecruitmentPlanStore Store)
        {
            this.store = Store;
        }

        public IQueryable<RecruitmentPlan> AllRecruitmentPlans
        {
            get { return this.store.RecruitmentPlans; }
        }

        public IQueryable<RecruitmentPlan> AvailableRecruitmentPlans
        {
            get
            {
                return this.AllRecruitmentPlans.Where(plan => plan.WhenPublished.HasValue && plan.ExpirationDate > DateTime.Now);
            }
        }

        public async Task<IQueryable<RecruitmentPlan>> GetPlansForUser(TargetUser User)
        {
            switch(User.RegisterationDelegate)
            {
                case "Internet":
                    return this.AvailableRecruitmentPlans.Where(plan => plan.IsPublic);
                case "Intranet":
                    return this.AvailableRecruitmentPlans.Where(plan => !plan.IsPublic);
            }
            throw new InvalidOperationException("无法识别TargetUser的RegisterationDelegate。");
        }

        /// <summary>
        /// 获取可用的、与用户相关联的招聘计划。
        /// </summary>
        /// <returns></returns>
        public async Task<IQueryable<RecruitmentPlan>> GetAvariableRecruitPlan(TargetUser user)
        {
            
            /////如果已存在报名资料，则返回报名资料对应的Plan
            /////
            //TargetUser currentuser = context.TargetUser;

            ////查找该用户的报名表
            //var enrollmentOfUserSet = from enrollment in this.database.EnrollmentData
            //                          where enrollment.UserID == currentuser.Id
            //                          select enrollment;

            //if (enrollmentOfUserSet.Any())
            //{
            //    List<RecruitmentPlan> plans = new List<RecruitmentPlan>();
            //    plans.Add(enrollmentOfUserSet.First().RecruitmentPlan);
            //    return plans.AsEnumerable();
            //}

            ///招聘计划状态为Normal
            ///招聘年度与DateTime.Now.Year
            ///根据用户的RegisterationDelegate决定过滤是否公开的招聘计划
            ///		如果IIdentity是ClaimsIdentity，说明为外部用户，选择公开招聘计划。
            ///		如果IIdentity是WindowsIdentity，说明是内部用户，根据其SID从用户表中选取模拟用户（其子女）///
            ///招聘计划处于ExpirationDate指定的有效期内。
            ///

            RegisterationDelegate regDelegate;
            if (!Enum.TryParse<RegisterationDelegate>(user.RegisterationDelegate, true, out regDelegate))
                return new List<RecruitmentPlan>().AsQueryable();

            //确定是否内外网的代码段，还需斟酌。
            //采用绑定目标用户的RegisterationDelegate将不依赖于IIdentity的类型，也就意味着其与网站具体的验证方式无关。
            //但是否会造成由于TargetUser设置不当的风险？需要在TargetUserManager中绑定目标用户时进行验证和保护。
            bool IsPublicFlag = true;
            if (regDelegate == RegisterationDelegate.Internet)
                IsPublicFlag = true;
            else
                IsPublicFlag = false;


            int year = DateTime.Now.Year;
            var recruitmentPlanSet = from recruitment in this.store.RecruitmentPlans
                                     where recruitment.WhenPublished.HasValue &&
                                        recruitment.Year == DateTime.Now.Year &&
                                        recruitment.EnrollExpirationDate > DateTime.Now &&
                                        recruitment.IsPublic == IsPublicFlag
                                     orderby recruitment.WhenCreated descending
                                     select recruitment;

            return recruitmentPlanSet;
        }

        /// <summary>
        /// 创建招聘计划。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task CreateRecruitmentPlan(RecruitmentPlan plan)
        {
            ///必须有明确的Publisher
            ///设置WhenCreated为当前时间
            ///设置State为Created
            ///有效期晚于当前日期
            ///报名截止日期晚于当前日期
            ///其他必填字段验证
            ///
            if (plan.ExpirationDate < DateTime.Now)
                throw new ArgumentException("招聘计划的有效期早于当前时间。");

            //if (plan.EnrollExpirationDate < DateTime.Now)
            //	throw new ArgumentException("报名截止日期早于当前时间。");

            //if (plan.EnrollExpirationDate > plan.ExpirationDate)
            //	throw new ArgumentException("报名的截止日期晚于招聘计划设定的有效期。");

            plan.Year = DateTime.Now.Year;
            plan.WhenCreated = DateTime.Now;
            //plan.State = RecruitmentPlanState.Created.ToString();

            await this.store.CreateAsync(plan);
        }

        /// <summary>
        /// 更新招聘计划。更新招聘不能改变招聘计划的状态。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task UpdateRecruitmentPlan(RecruitmentPlan plan)
        {
            ///只有State处于Created的计划可以被修改。
            ///可修改的字段包括Title, Recruitment, IsPublic, ExpirationDate, EnrollExpirationDate
            ///
            RecruitmentPlan current = this.store.RecruitmentPlans.SingleOrDefault(e => e.id == plan.id && !e.WhenPublished.HasValue);
            if (current == null)
                throw new ArgumentException("找不到计划或指定的计划不能修改");

            //检查有效期
            if (plan.ExpirationDate < DateTime.Now)
                throw new ArgumentException("招聘计划的有效期早于当前时间。");

            if (plan.EnrollExpirationDate < DateTime.Now)
                throw new ArgumentException("报名截止日期早于当前时间。");

            if (plan.EnrollExpirationDate > plan.ExpirationDate)
                throw new ArgumentException("报名的截止日期晚于招聘计划设定的有效期。");


            current.Title = plan.Title;
            current.Recruitment = plan.Recruitment;
            current.IsPublic = plan.IsPublic;
            current.ExpirationDate = plan.ExpirationDate;
            current.EnrollExpirationDate = plan.EnrollExpirationDate;

            await this.store.UpdateAsync(current);
        }

        /// <summary>
        /// 删除招聘计划。
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task DeleteRecruitmentPlan(RecruitmentPlan plan)
        {
            var toRemove = await this.FindByIDAsync(plan.id);
            if (plan.WhenPublished.HasValue)
                throw new InvalidOperationException("已发布的招聘计划无法删除。");

            await this.store.DeleteAsync(plan);
        }

        /// <summary>
        /// 发布招聘计划。招聘计划发布后，将可被所有符合条件的用户看到。
        /// </summary>
        /// <returns></returns>
        public async Task PublishRecruitmentPlan(int PlanID, DateTime EnrollExpirationDate)
        {
            ///当State处于Created时，将其设置为Normal.
            ///当State处于Normal时，不做任何操作。
            ///
            RecruitmentPlan plan = await this.FindByIDAsync(PlanID);
            if (plan == null)
                return;

            if (!plan.WhenPublished.HasValue)
            {
                //plan.State = RecruitmentPlanState.Normal.ToString();
                plan.WhenPublished = DateTime.Now;

                //所指定的截止日期当日全天都算作有效。
                plan.EnrollExpirationDate = EnrollExpirationDate;
            }

            await this.store.UpdateAsync(plan);
        }

        /// <summary>
        /// 对指定报名的所有已提交的报名提交审核状态。执行该操作意味着审核已结束，并且将审核状态发布到应聘者。
        /// 执行该操作后，不能执行反提交操作。
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="AnnounceExamExpirationDate"></param>
        /// <returns></returns>
        public async Task CommitAudit(RecruitmentPlan plan, DateTime AnnounceExamExpirationDate, DateTime ExamStartTime, DateTime ExamEndTime, string ExamLocation)
        {
            if (plan == null)
                throw new ArgumentNullException("plan");

            if (AnnounceExamExpirationDate < DateTime.Now)
                throw new ArgumentException("声明参考的截止日期不能晚于当前日期。");

            RecruitmentPlan currentplan = await this.FindByIDAsync(plan.id);
            if (currentplan == null)
                throw new ArgumentException("找不到指定的招聘计划。");

            if (DateTime.Now < currentplan.EnrollExpirationDate)
                throw new ArgumentException("审核的提交早于报名截止日期。");

            if (AnnounceExamExpirationDate < currentplan.EnrollExpirationDate)
                throw new ArgumentException("声明参考的截止日期不能早于报名截止日期。");

            if (currentplan.WhenAuditCommited.HasValue)
                return;

            ///检查与此招聘计划关联的报名表。
            ///如果报名表未提交，则直接设置不通过，附加说明为未在报名截止日期内提交。
            ///如果审核状态Approved值未设置，则回退，报告人力资源管理员必须为已提交的报名表设置审核状态。
            ///如果以上检查都符合条件，为Plan设置提交日期，为每份报名表设置提交日期，并将审核结果提交短信发送队列进行发送。
            ///
            //检查提交日期在规定截止日期内的，尚未设置Approved的报名表。若存在，则审核提交操作无效。
            var checkSet = from enroll in currentplan.EnrollmentData
                           where enroll.WhenCommited < currentplan.EnrollExpirationDate && !enroll.Approved.HasValue
                           select enroll;
            if (checkSet.Any())
                throw new InvalidOperationException("还有未设定审核结果的报名表。请全部设定后，再进行提交。");

            //设置声明考试的截止日期。
            currentplan.AnnounceExpirationDate = AnnounceExamExpirationDate;

            SMSSvc.SMSServiceClient smsClient = new SMSSvc.SMSServiceClient();
            SmtpClient smtpClient = new SmtpClient("mail.qjyc.cn");
            smtpClient.UseDefaultCredentials = true;

            //开始每项提交并发送短信。
            foreach (EnrollmentData data in currentplan.EnrollmentData)
            {
                //若没有提交，或提交日期晚于报名截止日期的，直接设定为不通过。
                if (!data.WhenCommited.HasValue || data.WhenCommited > currentplan.EnrollExpirationDate)
                {
                    data.Approved = false;
                    data.AuditMessage = "未在指定的报名截止时间内提交";
                }

                data.WhenAudit = DateTime.Now;

                //提交发送短信
                string smsMsg;
                if (data.Approved.Value)
                    smsMsg = string.Format(smsApprovedMsg, data.Name, currentplan.Title, currentplan.AnnounceExpirationDate.Value.ToString("yyyy-MM-dd HH:mm"));
                else
                    smsMsg = string.Format(smsRejectiveMsg, data.Name, currentplan.Title, currentplan.AnnounceExpirationDate.Value.ToString("yyyy-MM-dd HH:mm"));

                await smsClient.SendMessageAsync(new string[] { data.Mobile }, smsMsg, new SMSSvc.SendMessageOption());

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

            }

            smsClient.Close();
            smtpClient.Dispose();

            //每项提交完成后，修改currentPlan的标记，表示已提交审核。
            currentplan.WhenAuditCommited = DateTime.Now;

            //设定考试时间和地点
            currentplan.ExamStartTime = ExamStartTime;
            currentplan.ExamEndTime = ExamEndTime;
            currentplan.ExamLocation = ExamLocation;

            await this.store.UpdateAsync(currentplan);
        }
        string smsRejectiveMsg = "{0}，您好，您所填报的{1}报名资料，经初审未通过，感谢您的参与！";
        string smsApprovedMsg = "{0}，您好，您所填报的{1}报名资料，经初审通过，请于{2}前登陆网站声明是否参加考试，逾期未声明是否参加考试的不予参加考试。谢谢您的合作。";



        /// <summary>
        /// 根据指定的ID查找招聘计划。若未找到指定的招聘计划，则返回默认值null。
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        public async Task<RecruitmentPlan> FindByIDAsync(int PlanID)
        {
            return await this.store.FindByIdAsync(PlanID);
        }

        public async Task<IQueryable<ArchiveRequirements>> GetArchiveRequirements(RecruitmentPlan plan)
        {
            var arStore = this.store as IArchiveRequirementStore;
            if (arStore == null)
                throw new NotSupportedException("不支持。请为传入的存储实现IArchiveRequirementStore接口。");

            return await arStore.GetArchiveRequirementsAsync(plan);
        }

        public async Task AddArchiveRequirement(RecruitmentPlan plan, ArchiveRequirements requirement)
        {
            var arStore = this.store as IArchiveRequirementStore;
            if (arStore == null)
                throw new NotSupportedException("不支持。请为传入的存储实现IArchiveRequirementStore接口。");

            await arStore.AddArchiveRequirementAsync(plan, requirement);
        }

        public async Task UpdateArchiveRequirement(RecruitmentPlan plan, ArchiveRequirements requirement)
        {
            var arStore = this.store as IArchiveRequirementStore;
            if (arStore == null)
                throw new NotSupportedException("不支持。请为传入的存储实现IArchiveRequirementStore接口。");

            await arStore.UpdateArchiveRequirementAsync(plan, requirement);
        }

        public async Task RemoveArchiveRequirement(RecruitmentPlan plan, ArchiveRequirements requirement)
        {
            var arStore = this.store as IArchiveRequirementStore;
            if (arStore == null)
                throw new NotSupportedException("不支持。请为传入的存储实现IArchiveRequirementStore接口。");

            await arStore.RemoveArchiveRequirementAsync(plan, requirement);
        }
    }
}
