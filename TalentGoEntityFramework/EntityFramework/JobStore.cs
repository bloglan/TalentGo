using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.EntityFramework
{
    public class JobStore : IJobStore
    {
        DbContext db;
        DbSet<Job> set;

        public JobStore(DbContext dbContext)
        {
            this.db = dbContext;
            this.set = this.db.Set<Job>();
        }

        /// <summary>
        /// Get queryable collection of jobs.
        /// </summary>
        public IQueryable<Job> Jobs => this.set;
    }
}
