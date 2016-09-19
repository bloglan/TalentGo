using System.Linq;
using System.Threading.Tasks;
using TalentGo.Recruitment;

namespace TalentGo.EntityFramework
{
    /// <summary>
    /// An implement for stored recruitment data via Entity Framework.
    /// </summary>
    public class RecruitmentStore : IRecruitmentPlanStore, IArchiveRequirementStore
    {
        TalentGoDbContext db;

        /// <summary>
        /// Default ctor.
        /// </summary>
        /// <param name="DbContext"></param>
        public RecruitmentStore(TalentGoDbContext DbContext)
        {
            this.db = DbContext;
        }

        /// <summary>
        /// Get Queryable Recruitment Plan collection.
        /// </summary>
        public IQueryable<RecruitmentPlan> RecruitmentPlans
        {
            get
            {
                return this.db.RecruitmentPlan;
            }
        }

        /// <summary>
        /// Create new recruitment plan in databse.
        /// </summary>
        /// <param name="Plan"></param>
        /// <returns></returns>
        public async Task CreateAsync(RecruitmentPlan Plan)
        {
            this.db.RecruitmentPlan.Add(Plan);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// Update recruitment plan
        /// </summary>
        /// <param name="Plan"></param>
        /// <returns></returns>
        public async Task UpdateAsync(RecruitmentPlan Plan)
        {
            RecruitmentPlan old = await this.FindByIdAsync(Plan.id);
            if (old != null)
            {
                var entry = this.db.Entry<RecruitmentPlan>(old);
                entry.CurrentValues.SetValues(Plan);
                entry.Property(p => p.WhenCreated).IsModified = false;

                await this.db.SaveChangesAsync();
            }
            
        }

        /// <summary>
        /// Delete recruitment plan from db.
        /// </summary>
        /// <param name="Plan"></param>
        /// <returns></returns>
        public async Task DeleteAsync(RecruitmentPlan Plan)
        {
            var current = await this.FindByIdAsync(Plan.id);
            if (current != null)
            {
                this.db.RecruitmentPlan.Remove(current);
                await this.db.SaveChangesAsync();
            }
            
        }

        /// <summary>
        /// Find one recruitment plan by its id.
        /// If not exist, return null.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<RecruitmentPlan> FindByIdAsync(int Id)
        {
            return this.db.RecruitmentPlan.FirstOrDefault(plan => plan.id == Id);
        }

        /// <summary>
        /// Get queryable archive requirement collection of plan.
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task<IQueryable<ArchiveRequirement>> GetArchiveRequirementsAsync(RecruitmentPlan plan)
        {
            return this.db.ArchiveRequirements.Where(ar => ar.RecruitmentPlanID == plan.id);
        }

        /// <summary>
        /// Add an archive requirement for present plan.
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        public async Task AddArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirement requirement)
        {
            this.db.ArchiveRequirements.Add(requirement);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// Update archive requirement info.
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        public async Task UpdateArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirement requirement)
        {
            var current = (await this.GetArchiveRequirementsAsync(plan)).FirstOrDefault(r => r.ArchiveCategoryID == requirement.ArchiveCategoryID && r.RecruitmentPlanID == requirement.RecruitmentPlanID);
            if (current != null)
            {
                var entry = this.db.Entry<ArchiveRequirement>(current);
                entry.CurrentValues.SetValues(requirement);

                await this.db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Remove present archive requirement of plan.
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        public async Task RemoveArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirement requirement)
        {
            var current = (await this.GetArchiveRequirementsAsync(plan)).FirstOrDefault(r => r.ArchiveCategoryID == requirement.ArchiveCategoryID && r.RecruitmentPlanID == requirement.RecruitmentPlanID);
            if (current != null)
            {
                this.db.ArchiveRequirements.Remove(current);
                await this.db.SaveChangesAsync();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        /// <summary>
        /// Dispose with disposing flag.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~RecruitmentStore() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        /// <summary>
        /// Dispose this instance.
        /// </summary>
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
