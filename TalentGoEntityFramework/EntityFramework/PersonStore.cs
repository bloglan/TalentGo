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
    public class PersonStore : IPersonStore
    {
        DbContext db;
        DbSet<Person> set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public PersonStore(DbContext dbContext)
        {
            this.db = dbContext;
            this.set = this.db.Set<Person>();
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<Person> People => this.set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task CreateAsync(Person person)
        {
            this.set.Add(person);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Person person)
        {
            this.set.Remove(person);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Person> FindByIdAsync(Guid Id)
        {
            return await this.set.FindAsync(Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Person person)
        {
            this.db.Entry(person).State = EntityState.Modified;
            await this.db.SaveChangesAsync();
        }
    }
}
