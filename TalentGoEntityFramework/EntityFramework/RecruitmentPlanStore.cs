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
        /// <param name="dbContext"></param>
        public RecruitmentPlanStore(DbContext dbContext)
        {
            this.db = dbContext;
            this.set = this.db.Set<RecruitmentPlan>();
        }

        /// <summary>
        /// Get Queryable Recruitment Plan collection.
        /// </summary>
        public IQueryable<RecruitmentPlan> Plans => this.set;

        /// <summary>
        /// Create new recruitment plan in databse.
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task CreateAsync(RecruitmentPlan plan)
        {
            if (plan == null)
            {
                throw new System.ArgumentNullException(nameof(plan));
            }

            this.set.Add(plan);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// Update recruitment plan
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task UpdateAsync(RecruitmentPlan plan)
        {
            if (plan == null)
            {
                throw new System.ArgumentNullException(nameof(plan));
            }

            RecruitmentPlan old = await this.FindByIdAsync(plan.Id);
            if (old != null)
            {
                var entry = this.db.Entry<RecruitmentPlan>(old);
                entry.CurrentValues.SetValues(plan);
                entry.Property(p => p.WhenCreated).IsModified = false;

                await this.db.SaveChangesAsync();
            }
            
        }

        /// <summary>
        /// Delete recruitment plan from db.
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task DeleteAsync(RecruitmentPlan plan)
        {
            if (plan == null)
            {
                throw new System.ArgumentNullException(nameof(plan));
            }

            var current = await this.FindByIdAsync(plan.Id);
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
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RecruitmentPlan> FindByIdAsync(int id)
        {
            return await this.set.FindAsync(id);
        }


    }
}
