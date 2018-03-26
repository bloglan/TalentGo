using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class ExaminationPlanStore : IExaminationPlanStore
    {
        DbContext db;
        DbSet<ExaminationPlan> set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public ExaminationPlanStore(DbContext dbContext)
        {
            this.db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.set = this.db.Set<ExaminationPlan>();
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<ExaminationPlan> ExaminationPlans => this.set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task CreateAsync(ExaminationPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            this.set.Add(plan);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task DeleteAsync(ExaminationPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            this.set.Remove(plan);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ExaminationPlan> FindByIdAsync(int Id)
        {
            return await this.set.FindAsync(Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ExaminationPlan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            this.db.Entry(plan).State = EntityState.Modified;
            await this.db.SaveChangesAsync();
        }
    }
}
