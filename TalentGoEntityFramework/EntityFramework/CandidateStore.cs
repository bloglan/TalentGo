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
    public class CandidateStore : ICandidateStore
    {
        DbContext db;
        DbSet<Candidate> set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public CandidateStore(DbContext dbContext)
        {
            this.db = dbContext;
            this.set = this.db.Set<Candidate>();
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<Candidate> Candidates => this.set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task CreateAsync(Candidate item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            this.set.Add(item);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Candidate item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.set.Remove(item);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="examId"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        public async Task<Candidate> FindByIdAsync(int examId, Guid personId) => await this.set.FindAsync(examId, personId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Candidate item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            this.db.Entry(item).State = EntityState.Modified;
            await this.db.SaveChangesAsync();
        }
    }
}
