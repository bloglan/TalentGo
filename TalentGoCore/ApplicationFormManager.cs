using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalentGo.Linq;
using System.Transactions;

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
        /// Find application form by id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ApplicationForm> FindByIdAsync(int Id)
        {
            return await this.store.FindByIdAsync(Id);
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
            return null;

        }

        /// <summary>
        /// 根据指定的ArchiveId获取报名表对应的文档。
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public EnrollmentArchive FindEnrollmentArchiveByIdAsync(int Id)
        {
            return null;
        }



        #region Operations for enrollment

        /// <summary>
        /// 为指定的用户和招聘计划创建报名表。
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task CreateAsync(ApplicationForm form)
        {
            //根据要求，一个用户只能参与一个报名。
            var currentPlanId = form.Job.PlanId;
            var currentPersonId = form.PersonId;

            if (this.store.ApplicationForms.Any(a => a.Job.PlanId == currentPlanId && a.PersonId == currentPersonId))
            {
                throw new InvalidOperationException("在一个招聘计划内只能创建一份报名表。");
            }
            
            await this.store.CreateAsync(form);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task ModifyAsync(ApplicationForm form)
        {
            if (form == null)
                throw new ArgumentNullException();

            if (form.WhenCommited.HasValue)
                throw new InvalidOperationException("已提交的报名表不能修改。");

            form.WhenChanged = DateTime.Now;

            await this.store.UpdateAsync(form);
        }

        /// <summary>
        /// 删除报名表。
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task DeleteAsync(ApplicationForm form)
        {
            //如果该报名表已经提交，则不能删除。
            if (form.WhenCommited.HasValue)
                throw new InvalidOperationException("已提交的报名表不能删除。");

            await this.store.DeleteAsync(form);
        }

        /// <summary>
        /// 提交报名资料。
        /// </summary>
        /// <returns></returns>
        public async Task CommitAsync(ApplicationForm form)
        {
            if (form == null)
                throw new ArgumentNullException();

            if (form.Id == 0)
                await this.CreateAsync(form);

            //TODO:检查传送资料是否齐全并满足规则。

            if (!form.WhenCommited.HasValue)
                form.WhenCommited = DateTime.Now;

            await this.ModifyAsync(form);
        }

        //string smsRejectiveMsg = "{0}，您好，您所填报的{1}报名资料，经初审未通过，感谢您的参与！";
        //string smsApprovedMsg = "{0}，您好，您所填报的{1}报名资料，经初审通过，请于{2}前登陆网站声明是否参加考试，逾期未声明是否参加考试的不予参加考试。谢谢您的合作。";


        /// <summary>
        /// 声明是否参加考试。
        /// </summary>
        /// <param name="form"></param>
        /// <param name="IsTakeExam"></param>
        /// <returns></returns>
		public async Task AnnounceForExamAsync(ApplicationForm form, bool IsTakeExam)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            //必须是已提交的，通过审核的，尚未声明的，当前在声明有效期内的。
            if (!form.WhenAudit.HasValue)
                throw new InvalidOperationException("无效的操作，尚未审核。");

            if (!form.Approved.Value)
                throw new InvalidOperationException("无效的操作，审核未通过。");

            if (form.WhenAnnounced.HasValue)
                throw new InvalidOperationException("无效的操作，已进行了声明。不能重复声明。");

            if (form.Job.Plan.AnnounceExpirationDate.Value < DateTime.Now)
                throw new InvalidOperationException("无效的操作，已过声明截止时间。");

            form.IsTakeExam = IsTakeExam;
            form.WhenAnnounced = DateTime.Now;

            await this.store.UpdateAsync(form);
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
            
        }

        /// <summary>
        /// 为报名表移除文档。
        /// </summary>
        /// <param name="enrollment"></param>
        /// <param name="archive"></param>
        /// <returns></returns>
        public async Task RemoveEnrollmentArchive(ApplicationForm enrollment, EnrollmentArchive archive)
        {
            
        }

        #endregion







        #region 管理板块的方法

        /// <summary>
        /// 根据关键字、标记、获取满足条件的报名表。
        /// </summary>
        /// <param name="PlanID">指定的招聘计划。</param>
        /// <param name="AuditFilter">审核标记，null表示未审核，通过标记为true，false标记为未通过</param>
        /// <param name="AnounceFilter"></param>
        /// <param name="Keywords">关键字，若提供，可对姓名、身份证号码、移动电话号码、籍贯、生源地、学校、填写专业字段进行匹配搜索。否则查询全部。</param>
        /// <returns></returns>
        public IQueryable<ApplicationForm> GetCommitedEnrollmentData(int PlanID, AuditFilterType AuditFilter, AnnounceFilterType AnounceFilter, string Keywords)
        {
            //带分页
            //
            //先获得符合初始条件的集合
            var initSet = this.CommitedForms.Where(e => e.JobId == PlanID);


            if (!string.IsNullOrEmpty(Keywords))
                initSet = initSet.Where(e =>
                    e.Name.StartsWith(Keywords) ||
                    e.Person.IDCardNumber.StartsWith(Keywords) ||
                    e.Person.Mobile.StartsWith(Keywords) ||
                    e.NativePlace.StartsWith(Keywords) ||
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
        /// <param name=" AuditFilter"></param>
        /// <param name="AnnounceFilter"></param>
        /// <param name="Keywords"></param>
        /// <param name="OrderColumn"></param>
        /// <param name="DownDirection"></param>
        /// <param name="ItemCount"></param>
        /// <returns></returns>
		public IQueryable<ApplicationForm> GetCommitedEnrollmentData(int PlanID, AuditFilterType AuditFilter, AnnounceFilterType AnnounceFilter, string Keywords, string OrderColumn, bool DownDirection, out int ItemCount)
        {
            var resultSet = this.GetCommitedEnrollmentData(PlanID, AuditFilter, AnnounceFilter, Keywords);

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
        /// <param name="AuditFilter"></param>
        /// <param name="AnnounceFilter"></param>
        /// <param name="Keywords"></param>
        /// <param name="OrderColumn"></param>
        /// <param name="DownDirection"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="ItemCount"></param>
        /// <returns></returns>
        public IQueryable<ApplicationForm> GetCommitedEnrollmentData(int PlanID, AuditFilterType AuditFilter, AnnounceFilterType AnnounceFilter, string Keywords, string OrderColumn, bool DownDirection, int PageIndex, int PageSize, out int ItemCount)
        {
            var result = this.GetCommitedEnrollmentData(PlanID, AuditFilter, AnnounceFilter, Keywords, OrderColumn, DownDirection, out ItemCount);
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
