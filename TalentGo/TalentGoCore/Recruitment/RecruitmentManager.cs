using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TalentGo.EntityFramework;
using TalentGo.Identity;

namespace TalentGo.Recruitment
{
	/// <summary>
	/// 招聘管理器
	/// </summary>
	public class RecruitmentManager
	{
		TalentGoDbContext database;
		TargetUserManager targetUserManager;
		HttpContextBase context;
		IRecruitmentStore<RecruitmentPlan> store;

		public RecruitmentManager(HttpContextBase context)
		{
			this.context = context;
			this.targetUserManager = new TargetUserManager(context);
			this.database = TalentGoDbContext.FromContext(this.context);
		}

		/// <summary>
		/// 获取可用的、与用户相关联的招聘计划。
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<RecruitmentPlan>> GetAvariableRecruitPlan()
		{
			if (!this.targetUserManager.IsAssignedTargetUser)
			{
				throw new InvalidOperationException("操作无效，没有绑定用户");
			}
			///如果已存在报名资料，则返回报名资料对应的Plan
			///
			TargetUser currentuser = this.targetUserManager.TargetUser;

			//查找该用户的报名表
			var enrollmentOfUserSet = from enrollment in this.database.EnrollmentData
									  where enrollment.UserID == currentuser.Id
									  select enrollment;

			if (enrollmentOfUserSet.Any())
			{
				List<RecruitmentPlan> plans = new List<RecruitmentPlan>();
				plans.Add(enrollmentOfUserSet.First().RecruitmentPlan);
				return plans.AsEnumerable();
			}

			///招聘计划状态为Normal
			///招聘年度与DateTime.Now.Year
			///根据用户的RegisterationDelegate决定过滤是否公开的招聘计划
			///		如果IIdentity是ClaimsIdentity，说明为外部用户，选择公开招聘计划。
			///		如果IIdentity是WindowsIdentity，说明是内部用户，根据其SID从用户表中选取模拟用户（其子女）///
			///招聘计划处于ExpirationDate指定的有效期内。
			///

			RegisterationDelegate regDelegate;
			if (!Enum.TryParse<RegisterationDelegate>(currentuser.RegisterationDelegate, true, out regDelegate))
				return new List<RecruitmentPlan>().AsEnumerable();

			//确定是否内外网的代码段，还需斟酌。
			//采用绑定目标用户的RegisterationDelegate将不依赖于IIdentity的类型，也就意味着其与网站具体的验证方式无关。
			//但是否会造成由于TargetUser设置不当的风险？需要在TargetUserManager中绑定目标用户时进行验证和保护。
			bool IsPublicFlag = true;
			if (regDelegate == RegisterationDelegate.Internet)
				IsPublicFlag = true;
			else
				IsPublicFlag = false;


			int year = DateTime.Now.Year;
			var recruitmentPlanSet = from recruitment in this.database.RecruitmentPlan
									 where recruitment.WhenPublished.HasValue &&
										recruitment.Year == DateTime.Now.Year &&
                                        recruitment.EnrollExpirationDate > DateTime.Now&&
										recruitment.IsPublic == IsPublicFlag
									 orderby recruitment.WhenCreated descending
									 select recruitment;

			return recruitmentPlanSet;
		}

		/// <summary>
		/// 获取或设置已选中的招聘计划。如果可用的招聘计划只有一项，则返回该项代表默认选中。
		/// 如果可用的招聘计划有多项，且尚未明确设置前，该属性返回空引用。
		/// 如果没有可用的招聘计划，该属性返回空引用。
		/// </summary>
		/// <returns></returns>
		public RecruitmentPlan SelectedRecruitPlan
		{
			get
			{
				///尝试从缓存获取
				if (this.context.Session[SelectedRecruitPlanKey] != null)
					return (RecruitmentPlan)this.context.Session[SelectedRecruitPlanKey];

				///如果获取失败，尝试获得可用的招聘计划列表
				///如果已存在报名资料，则返回已存在报名资料的报名计划。
				///如果列表只有一项，则自动设为选中并返回。
				///否则返回null.
				///
				var enrollment = this.database.EnrollmentData.SingleOrDefault(e => e.UserID == this.targetUserManager.TargetUser.Id);
				if (enrollment != null)
				{
					RecruitmentPlan plan = enrollment.RecruitmentPlan;
					this.context.Session[SelectedRecruitPlanKey] = plan; //cache in session
					return plan;
				}


				var avaiablePlans = this.GetAvariableRecruitPlan().GetAwaiter().GetResult();
				if (avaiablePlans.Count() == 1)
				{
					RecruitmentPlan thisplan = avaiablePlans.First();
					this.context.Session[SelectedRecruitPlanKey] = thisplan; //cache in session
					return thisplan;
				}

				return null;
			}
		}

		public async Task SelectRecruitmentPlanByID(int PlanID)
		{
			var plans = await this.GetAvariableRecruitPlan();
			var current = plans.SingleOrDefault(e => e.id == PlanID);
			if (current == null)
				throw new ArgumentException("找不到指定的招聘计划");

			this.context.Session[SelectedRecruitPlanKey] = current;
		}

		/// <summary>
		/// 创建招聘计划。
		/// </summary>
		/// <param name="plan"></param>
		/// <returns></returns>
		public Task CreateRecruitmentPlan(RecruitmentPlan plan)
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

			this.database.RecruitmentPlan.Add(plan);

			try
			{
				this.database.SaveChanges();
			}
			catch (Exception ex)
			{
				string a = "";
			}
				

			return Task.FromResult(0);
		}

		/// <summary>
		/// 获取招聘计划，如果提供了年度，则获取指定年度的招聘计划。
		/// </summary>
		/// <param name="Year"></param>
		/// <returns></returns>
		public async Task<IEnumerable<RecruitmentPlan>> GetAllRecruitmentPlans(int? Year)
		{
			///如未提供年度，则查询所有计划，若提供年度，则只查询指定年度。
			///查询结果按照年度（或创建时间）倒序排列。
			if (Year.HasValue)
			{
				var resultSet = from recruitment in this.database.RecruitmentPlan
								where recruitment.Year == Year
								orderby recruitment.WhenCreated descending
								select recruitment;

				return resultSet.AsEnumerable();
			}
			else
			{
				var resultSet = from recruitment in this.database.RecruitmentPlan
								orderby recruitment.WhenCreated descending
								select recruitment;

				return resultSet.Take(100).AsEnumerable();
			}
		}

		/// <summary>
		/// 更新招聘计划。更新招聘不能改变招聘计划的状态。
		/// </summary>
		/// <param name="plan"></param>
		/// <returns></returns>
		public Task UpdateRecruitmentPlan(RecruitmentPlan plan)
		{
			///只有State处于Created的计划可以被修改。
			///可修改的字段包括Title, Recruitment, IsPublic, ExpirationDate, EnrollExpirationDate
			///
			RecruitmentPlan current = this.database.RecruitmentPlan.SingleOrDefault(e => e.id == plan.id && !e.WhenPublished.HasValue);
			if (current == null)
				throw new ArgumentException("找不到计划或指定的计划不能修改");

			//检查有效期
			if (plan.ExpirationDate < DateTime.Now)
				throw new ArgumentException("招聘计划的有效期早于当前时间。");

			if (plan.EnrollExpirationDate < DateTime.Now)
				throw new ArgumentException("报名截止日期早于当前时间。");

			if (plan.EnrollExpirationDate > plan.ExpirationDate)
				throw new ArgumentException("报名的截止日期晚于招聘计划设定的有效期。");

			var currentEntry = this.database.Entry<RecruitmentPlan>(current);
			currentEntry.Property(e => e.Title).CurrentValue = plan.Title;
			currentEntry.Property(e => e.Title).IsModified = true;
			currentEntry.Property(e => e.Recruitment).CurrentValue = plan.Recruitment;
			currentEntry.Property(e => e.Recruitment).IsModified = true;
			currentEntry.Property(e => e.IsPublic).CurrentValue = plan.IsPublic;
			currentEntry.Property(e => e.IsPublic).IsModified = true;
			currentEntry.Property(e => e.ExpirationDate).CurrentValue = plan.ExpirationDate;
			currentEntry.Property(e => e.ExpirationDate).IsModified = true;
			currentEntry.Property(e => e.EnrollExpirationDate).CurrentValue = plan.EnrollExpirationDate;
			currentEntry.Property(e => e.EnrollExpirationDate).IsModified = true;

			this.database.SaveChanges();
			return Task.FromResult(0);

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
			RecruitmentPlan plan = this.database.RecruitmentPlan.SingleOrDefault(e => e.id == PlanID);
			if (plan == null)
				return;

			if (!plan.WhenPublished.HasValue)
			{
				//处于创建状态的将其设置为Normal
				//plan.State = RecruitmentPlanState.Normal.ToString();
				plan.WhenPublished = DateTime.Now;

				//所指定的截止日期当日全天都算作有效。
				plan.EnrollExpirationDate = EnrollExpirationDate;
			}

			await this.database.SaveChangesAsync();
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

			RecruitmentPlan currentplan = this.database.RecruitmentPlan.SingleOrDefault(e => e.id == plan.id);
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

			await this.database.SaveChangesAsync();
		}
		string smsRejectiveMsg = "{0}，您好，您所填报的{1}报名资料，经初审未通过，感谢您的参与！";
		string smsApprovedMsg = "{0}，您好，您所填报的{1}报名资料，经初审通过，请于{2}前登陆网站声明是否参加考试，逾期未声明是否参加考试的不予参加考试。谢谢您的合作。";



		/// <summary>
		/// 根据指定的ID查找招聘计划。若未找到指定的招聘计划，则返回默认值null。
		/// </summary>
		/// <param name="PlanID"></param>
		/// <returns></returns>
		public Task<RecruitmentPlan> FindByID(int PlanID)
		{
			///返回指定ID的招聘计划或默认值。
			RecruitmentPlan plan = this.database.RecruitmentPlan.SingleOrDefault(e => e.id == PlanID);
			return Task.FromResult<RecruitmentPlan>(plan);
		}

		static readonly string SelectedRecruitPlanKey = "SelectedRecruitPlan";
	}
}
