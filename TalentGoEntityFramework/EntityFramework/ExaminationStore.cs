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
    public class ExaminationStore : IExaminationStore
    {
        DbContext db;
        DbSet<Examination> set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public ExaminationStore(DbContext dbContext)
        {
            this.db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.set = this.db.Set<Examination>();
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<Examination> Exams => this.set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        public async Task CreateAsync(Examination exam)
        {
            if (exam == null)
            {
                throw new ArgumentNullException(nameof(exam));
            }

            this.set.Add(exam);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Examination exam)
        {
            if (exam == null)
            {
                throw new ArgumentNullException(nameof(exam));
            }

            this.set.Remove(exam);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Examination> FindByIdAsync(int Id)
        {
            return await this.set.FindAsync(Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Examination exam)
        {
            if (exam == null)
            {
                throw new ArgumentNullException(nameof(exam));
            }

            this.db.Entry(exam).State = EntityState.Modified;
            await this.db.SaveChangesAsync();
        }
    }
}
