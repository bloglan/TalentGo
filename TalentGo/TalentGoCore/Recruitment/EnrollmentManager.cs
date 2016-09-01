using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TalentGo.Identity;
using TalentGo.Utilities;
using TalentGo.ViewModels;
using TalentGo.Linq;
using TalentGo.EntityFramework;

namespace TalentGo.Recruitment
{
	/// <summary>
	/// 表示招聘报名管理器
	/// </summary>
	public class EnrollmentManager
	{
		HttpContextBase context;
		TalentGoDbContext database;
		RecruitmentManager recruitManager;
		TargetUserManager targetUserManager;

		public EnrollmentManager(HttpContextBase context)
		{
			this.context = context;
			this.database = TalentGoDbContext.FromContext(this.context);
			this.recruitManager = new RecruitmentManager(context);
			this.targetUserManager = new TargetUserManager(context);
		}

		/// <summary>
		/// 获取可用的报名表，若没有，则创建一个默认。
		/// </summary>
		/// <returns></returns>
		public async Task<EnrollmentData> GetAvaiableOrDefault()
		{
			EnrollmentData data;
			this.TryGetEnrollmentData(out data);

			this.database.EnrollmentData.TakeWhile(e => e.UserID == 123);
			return data;
		}

		/// <summary>
		/// 获取一个值，指示用户是否具有与选定报名计划关联的报名表。
		/// </summary>
		public bool HasEnrollmentData
		{
			get
			{
				EnrollmentData data;
				return this.TryGetEnrollmentData(out data);
			}
		}

		/// <summary>
		/// 创建或更新报名表资料。
		/// </summary>
		/// <param name="enrollData"></param>
		/// <returns></returns>
		public async Task CreateOrUpdate(EnrollmentData enrollData)
		{
			///创建或更新报名表
			///根据指示的招聘计划和用户，查找是否已存在报名表，如果不存在，则创建。
			///如果存在，则如果WhenCommitted没有值，则允许修改，否则抛出异常——已提交的不能修改。
			///
			///如果是创建：
			///		对应有效的招聘计划
			///		没有超过报名计划指示的报名截止日期
			///		设置和验证其他需要的字段
			///如果是更新：
			///		只能更新报名表字段，不能用来更新控制字段。
			///		更新WhenChanged.

			//查找执行操作的报名计划

			EnrollmentData storedEnrollmentData;
			bool UpdateFlag = this.TryGetEnrollmentData(out storedEnrollmentData);

			if (UpdateFlag)
			{
				//update current
				var edit = storedEnrollmentData;

				if (edit.WhenCommited.HasValue)
					throw new InvalidOperationException("已提交的报名资料不允许修改。");

				//检查和约束
				enrollData.WhenChanged = DateTime.Now;

				var entry = this.database.Entry<EnrollmentData>(edit);
				entry.CurrentValues.SetValues(enrollData);
				entry.Property(e => e.WhenCreated).IsModified = false;
				entry.Property(e => e.WhenCommited).IsModified = false;
				entry.Property(e => e.WhenAudit).IsModified = false;
				entry.Property(e => e.Approved).IsModified = false;
				entry.Property(e => e.AuditMessage).IsModified = false;
				entry.Property(e => e.WhenAnnounced).IsModified = false;
				entry.Property(e => e.IsTakeExam).IsModified = false;
			}
			else
			{
				//Create a new enrollment data
				//检查和约束
				enrollData.RecruitPlanID = this.recruitManager.SelectedRecruitPlan.id;
				enrollData.UserID = this.targetUserManager.TargetUser.Id;
				enrollData.WhenCreated = DateTime.Now;
				enrollData.WhenChanged = DateTime.Now;
				enrollData.WhenCommited = null;
				enrollData.WhenCommited = null;
				enrollData.WhenAudit = null;
				enrollData.Approved = null;
				enrollData.AuditMessage = null;
				enrollData.WhenAnnounced = null;

				this.database.EnrollmentData.Add(enrollData);

			}

			await this.database.SaveChangesAsync();


		}

		/// <summary>
		/// 提交报名资料。
		/// </summary>
		/// <returns></returns>
		public async Task CommitEnrollment()
		{
			///提交报名资料时，对报名资料以及关联的图片文件资料进行检查。
			///提交后不能反向提交。
			///
			EnrollmentData data;
			if (!this.TryGetEnrollmentData(out data))
				throw new InvalidOperationException("无效的操作，不存在已登记的报名表，不能提交。");

			if (data.WhenCommited.HasValue)
				throw new InvalidOperationException("报名资料已处于提交状态，不能重复提交。");

			//检查提交文档的符合性。
			ArchiveManager archiveMgr = new ArchiveManager(this.context);
			var archiveReqs = await archiveMgr.GetArchiveRequirements();
			var archives = await archiveMgr.GetEnrollmentArchives();
			List<string> failMsg = new List<string>();
			foreach (ArchiveRequirements req in archiveReqs)
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
			///检查报名表及关联报名资料是否合格，若不合格，则提示错误。
			///检查报名截止时间，如果该提交在报名截止时间之后，则直接提示未通过。

			if (data.RecruitmentPlan.EnrollExpirationDate < DateTime.Now)
				throw new InvalidOperationException("报名截止时间已过。");

			data.WhenCommited = DateTime.Now;
			await this.database.SaveChangesAsync();
		}

		public async Task AnnounceForExam(bool IsTakeExam)
		{
			EnrollmentData data;
			if (!this.TryGetEnrollmentData(out data))
			{
				throw new InvalidOperationException("无效的操作，未发现报名信息。");
			}

			//必须是已提交的，通过审核的，尚未声明的，当前在声明有效期内的。
			if (!data.WhenCommited.HasValue)
				throw new InvalidOperationException("无效的操作，报名表尚未提交。");

			if (!data.WhenAudit.HasValue)
				throw new InvalidOperationException("无效的操作，尚未审核。");

			if (!data.Approved.Value)
				throw new InvalidOperationException("无效的操作，审核未通过。");

			if (data.WhenAnnounced.HasValue)
				throw new InvalidOperationException("无效的操作，已进行了声明。不能重复声明。");

			if (data.RecruitmentPlan.AnnounceExpirationDate.Value < DateTime.Now)
				throw new InvalidOperationException("无效的操作，已过声明截止时间。");

			data.WhenAnnounced = DateTime.Now;
			data.IsTakeExam = IsTakeExam;

			await this.database.SaveChangesAsync();
		}

		#region 管理板块的方法

		/// <summary>
		/// 根据关键字、标记、排序指示和分页参数获取满足条件的指定页的报名表。
		/// </summary>
		/// <param name="plan">指定的招聘计划。</param>
		/// <param name="MajorCategory">指定的专业大类，若未指定，则获取全部。</param>
		/// <param name="AuditFilter">审核标记，null表示未审核，通过标记为true，false标记为未通过</param>
		/// <param name="Keywords">关键字，若提供，可对姓名、身份证号码、移动电话号码、籍贯、生源地、学校、填写专业字段进行匹配搜索。否则查询全部。</param>
		/// <param name="OrderColumn">指定用来排序的字段名称。若未指定，则按WhenCommited倒序排序。</param>
		/// <param name="DownDirection">指示一个值，表示是按正序还是倒序排列。</param>
		/// <param name="PageIndex">指示要显示的页面索引。</param>
		/// <param name="PageSize">指示每一页要显示的项数。若给定值小于0，则使用MaxValue值，若介于0-5之间，则PageSize设为5项。</param>
		/// <param name="ItemCount">返回一个值，指示满足条件的所有项计数。</param>
		/// <returns></returns>
		public IQueryable<EnrollmentData> GetCommitedEnrollmentData(int PlanID, string MajorCategory, AuditFilterType AuditFilter, AnnounceFilterType AnounceFilter, string Keywords)
		{
			///带分页
			///
			//先获得符合初始条件的集合
			var initSet = from enrollment in this.database.EnrollmentData
						  where enrollment.RecruitPlanID == PlanID &&
						enrollment.WhenCommited.HasValue
						  select enrollment;

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

			switch(AnounceFilter)
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


			//ItemCount = 0;
			//return new List<EnrollmentData>().AsEnumerable();

		}

		public IQueryable<EnrollmentData> GetCommitedEnrollmentData(int PlanID, string MajorCategory, AuditFilterType AuditFilter,AnnounceFilterType AnnounceFilter, string Keywords, string OrderColumn, bool DownDirection, out int ItemCount)
		{
			var resultSet = this.GetCommitedEnrollmentData(PlanID, MajorCategory, AuditFilter, AnnounceFilter, Keywords);

			ItemCount = resultSet.Count();
			if (ItemCount == 0)
				return resultSet;

			//按字段排序
			if (string.IsNullOrEmpty(OrderColumn))
				OrderColumn = "WhenCommited";

			IOrderedQueryable<EnrollmentData> OrderedSet;
			if (DownDirection)
				OrderedSet = resultSet.OrderByDescending(OrderColumn);
			else
				OrderedSet = resultSet.OrderBy(OrderColumn);

			return OrderedSet;



		}

		

		public IQueryable<EnrollmentData> GetCommitedEnrollmentData(int PlanID, string MajorCategory, AuditFilterType AuditFilter, AnnounceFilterType AnnounceFilter, string Keywords, string OrderColumn, bool DownDirection, int PageIndex, int PageSize, out int ItemCount)
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

		#region 帮助方法

		/// <summary>
		/// 尝试获得报名表数据。
		/// </summary>
		/// <param name="IsDefault"></param>
		/// <returns></returns>
		bool TryGetEnrollmentData(out EnrollmentData enrollmentData)
		{
			///如果没有选中招聘计划，则返回false;
			///
			if (this.recruitManager.SelectedRecruitPlan == null)
			{
				enrollmentData = null;
				return false;
			}

			var user = this.targetUserManager.TargetUser;

			EnrollmentData data = this.database.EnrollmentData.SingleOrDefault(e => e.RecruitPlanID == this.recruitManager.SelectedRecruitPlan.id && e.UserID == this.targetUserManager.TargetUser.Id);

			//data = user.EnrollmentData.SingleOrDefault(e => e.RecruitPlanID == this.recruitManager.SelectedRecruitPlan.id);

			if (data == null)
			{
				//Create a new
				data = this.database.EnrollmentData.Create();
				data.RecruitPlanID = this.recruitManager.SelectedRecruitPlan.id;
				data.UserID = user.Id;
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
				enrollmentData = data;
				return false;
			}

			enrollmentData = data;
			return true;
		}

		#endregion
	}
}
