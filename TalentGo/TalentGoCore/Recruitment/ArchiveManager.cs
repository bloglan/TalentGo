using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TalentGo.EntityFramework;

namespace TalentGo.Recruitment
{
	/// <summary>
	/// 表示一个文档管理器，
	/// </summary>
	public class ArchiveManager
	{
		HttpContextBase context;
		RecruitmentManager recruitmentNamager;
		EnrollmentManager enrollmentManager;
		TalentGoDbContext database;
		public ArchiveManager(HttpContextBase context)
		{
			this.context = context;
			this.recruitmentNamager = new RecruitmentManager(context);
			this.enrollmentManager = new EnrollmentManager(context);
			this.database = TalentGoDbContext.FromContext(this.context);
		}

		/// <summary>
		/// 获取所有文档编目。
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<ArchiveCategory>> GetAllArchiveCategory()
		{
			var cateSet = from ac in this.database.ArchiveCategory
						  select ac;

			return cateSet.AsEnumerable();
		}

		/// <summary>
		/// 创建或更新指定的文档编目。
		/// </summary>
		/// <param name="archiveCategory"></param>
		/// <returns></returns>
		public async Task CreateOrUpdateArchiveCategory(ArchiveCategory archiveCategory)
		{
			//创建或更新，
			//
			if (archiveCategory.id != 0)
			{
				ArchiveCategory edit = this.database.ArchiveCategory.SingleOrDefault(e => e.id == archiveCategory.id);
				if (edit == null)
					throw new ArgumentException("找不到要更新的文档编目。");

				//检查和设定
				archiveCategory.WhenChanged = DateTime.Now;

				var editEntry = this.database.Entry<ArchiveCategory>(edit);
				editEntry.CurrentValues.SetValues(archiveCategory);
				editEntry.Property(e => e.CreatedBy).IsModified = false;
				editEntry.Property(e => e.Enabled).IsModified = false;
				editEntry.Property(e => e.WhenCreated).IsModified = false;
			}
			else
			{
				archiveCategory.WhenCreated = DateTime.Now;
				archiveCategory.WhenChanged = DateTime.Now;
				archiveCategory.Enabled = true;

				this.database.ArchiveCategory.Add(archiveCategory);
			}

			await this.database.SaveChangesAsync();
		}

		/// <summary>
		/// 禁用或启用一个ArchiveCategory。启用或禁用文档编目会影响用户能够看到和操作的文档编目，以及系统对用户所要求指定文档的可见性。
		/// </summary>
		/// <param name="Enabled"></param>
		/// <returns></returns>
		public async Task SetArchiveCategoryEnabledState(ArchiveCategory item, bool Enabled)
		{
			var edit = this.database.ArchiveCategory.SingleOrDefault(e => e.id == item.id);
			if (edit == null)
				throw new ArgumentException("找不到要设定的ArchiveCategory");

			edit.Enabled = Enabled;

			await this.database.SaveChangesAsync();
		}

		/// <summary>
		/// 获取选中的招聘计划指示的文档材料需求。
		/// </summary>
		/// <param name="RecruitmentPlanID"></param>
		/// <returns></returns>
		public async Task<IEnumerable<ArchiveRequirements>> GetArchiveRequirements()
		{
			///如果没有选定的招聘计划，则返回空列表
			///
			RecruitmentPlan selectedPlan = this.recruitmentNamager.SelectedRecruitPlan;
			if (selectedPlan == null)
				return new List<ArchiveRequirements>().AsEnumerable();


			var resultSet = from ar in this.database.ArchiveRequirements
							where ar.RecruitmentPlanID == selectedPlan.id && ar.ArchiveCategory.Enabled == true
							orderby ar.ArchiveCategoryID
							select ar;

			return resultSet.AsEnumerable();
		}

		/// <summary>
		/// 获取报名对应的上传的文档集合。
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<EnrollmentArchives>> GetEnrollmentArchives()
		{
			///获取报名表
			///获取报名表对应的资料
			///
			if (!this.enrollmentManager.HasEnrollmentData)
			{
				return new List<EnrollmentArchives>().AsEnumerable();
			}

			EnrollmentData enroll = await this.enrollmentManager.GetAvaiableOrDefault();
			var resultSet = from ea in this.database.EnrollmentArchives
							where ea.RecruitPlanID == enroll.RecruitPlanID && ea.UserID == enroll.UserID
							select ea;

			return resultSet.AsEnumerable();

		}

		/// <summary>
		/// 添加或更新上传资料。如果指示的EnrollmentArchives.id等于0，则创建新的资料条目。
		/// </summary>
		/// <param name="archive"></param>
		/// <returns></returns>
		public async Task AddOrUpdateArchive(EnrollmentArchives archive)
		{
			///检查Plan的合法性，获得对应的Enrollment.
			///
			EnrollmentData enroll = await this.GetAndCheckEnrollment();

			if (archive.RecruitPlanID == enroll.RecruitPlanID && archive.UserID == enroll.UserID)
			{
				if (archive.id == 0)
				{
					//Create
					archive.WhenCreated = DateTime.Now;
					archive.WhenChanged = DateTime.Now;

					this.database.EnrollmentArchives.Add(archive);
				}
				else
				{
					//Try update item
					EnrollmentArchives orginial = this.database.EnrollmentArchives.SingleOrDefault(e => e.id == archive.id);
					if (orginial == null)
						throw new ArgumentException("找不到要更新的文档对象");

					archive.WhenChanged = DateTime.Now;

					DbEntityEntry<EnrollmentArchives> orgEntry = this.database.Entry<EnrollmentArchives>(orginial);
					orgEntry.CurrentValues.SetValues(archive);
				}

				try
				{
					await this.database.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			else
				throw new ArgumentException("给定的参数与当前报名不匹配");
		}

		/// <summary>
		/// 移除资料条目。
		/// </summary>
		/// <param name="archive"></param>
		/// <returns></returns>
		public async Task RemoveEnrollmentArchive(EnrollmentArchives archive)
		{
			//检查指定的Archive以及PlanID和UserID合法性。
			EnrollmentData enroll = await this.GetAndCheckEnrollment();
			///从数据库获取对象，如果有，删除之，若不存在，则不做任何操作返回
			///
			var resultSet = from arch in this.database.EnrollmentArchives
							where arch.RecruitPlanID == enroll.RecruitPlanID && arch.UserID == enroll.UserID && arch.id == archive.id
							select arch;
			if (!resultSet.Any())
				return;

			EnrollmentArchives selectedArchive = resultSet.First();
			this.database.EnrollmentArchives.Remove(selectedArchive);
			await this.database.SaveChangesAsync();
		}

		#region 帮助方法

		async Task<EnrollmentData> GetAndCheckEnrollment()
		{
			if (!this.enrollmentManager.HasEnrollmentData)
				throw new InvalidOperationException("操作无效，尚未报名");

			EnrollmentData enroll = await this.enrollmentManager.GetAvaiableOrDefault();
			///如果Enrollment已经提交，或已超过报名截止日期，则失败。
			if (enroll.RecruitmentPlan.EnrollExpirationDate <= DateTime.Now)
				throw new InvalidOperationException("操作无效，已超过报名截止日期。");

			if (enroll.WhenCommited.HasValue)
				throw new InvalidOperationException("报名表已提交，无法更新。");

			return enroll;
		}

		#endregion
	}
}
