using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.EntityFramework
{
    /// <summary>
    /// An implement for stored recruitment data via Entity Framework.
    /// </summary>
    public class RecruitmentPlanStore : IRecruitmentPlanStore
    {
        DbContext db;
        DbSet<RecruitmentPlan> set;

        /// <summary>
        /// Default ctor.
        /// </summary>
        /// <param name="DbContext"></param>
        public RecruitmentPlanStore(DbContext DbContext)
        {
            this.db = DbContext;
            this.set = this.db.Set<RecruitmentPlan>();
        }

        /// <summary>
        /// Get Queryable Recruitment Plan collection.
        /// </summary>
        public IQueryable<RecruitmentPlan> Plans => this.set;

        /// <summary>
        /// Create new recruitment plan in databse.
        /// </summary>
        /// <param name="Plan"></param>
        /// <returns></returns>
        public async Task CreateAsync(RecruitmentPlan Plan)
        {
            if (Plan == null)
            {
                throw new System.ArgumentNullException(nameof(Plan));
            }

            this.set.Add(Plan);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// Update recruitment plan
        /// </summary>
        /// <param name="Plan"></param>
        /// <returns></returns>
        public async Task UpdateAsync(RecruitmentPlan Plan)
        {
            if (Plan == null)
            {
                throw new System.ArgumentNullException(nameof(Plan));
            }

            RecruitmentPlan old = await this.FindByIdAsync(Plan.Id);
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
            if (Plan == null)
            {
                throw new System.ArgumentNullException(nameof(Plan));
            }

            var current = await this.FindByIdAsync(Plan.Id);
            if (current != null)
            {
                this.set.Remove(current);
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
            return await this.set.FindAsync(Id);
        }


    }
}
