using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationFormStore : IApplicationFormStore
    {
        DbContext dbContext;
        DbSet<ApplicationForm> set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DbContext"></param>
        public ApplicationFormStore(DbContext DbContext)
        {
            this.dbContext = DbContext;
            this.set = this.dbContext.Set<ApplicationForm>();
        }

        /// <summary>
        /// Gets a queryable collections for enrollment without change tracking.
        /// </summary>
        public IQueryable<ApplicationForm> ApplicationForms
        {
            get
            {
                return this.set;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApplicationForm> FindByIdAsync(int id)
        {
            return await this.set.FindAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Enrollment"></param>
        /// <returns></returns>
        public async Task CreateAsync(ApplicationForm Enrollment)
        {
            this.set.Add(Enrollment);
            await this.dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollment"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ApplicationForm enrollment)
        {
            this.dbContext.Entry(enrollment).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollment"></param>
        /// <returns></returns>
        public async Task DeleteAsync(ApplicationForm enrollment)
        {
            this.set.Remove(enrollment);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
