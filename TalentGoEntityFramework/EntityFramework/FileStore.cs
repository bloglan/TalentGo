using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.EntityFramework
{
    /// <summary>
    /// File Store.
    /// </summary>
    public class FileStore : IFileStore
    {
        DbContext db;
        DbSet<File> set;

        /// <summary>
        /// Initialize file store using DbContext.
        /// </summary>
        /// <param name="dbContext"></param>
        public FileStore(DbContext dbContext)
        {
            this.db = dbContext;
            this.set = this.db.Set<File>();
        }

        /// <summary>
        /// Get all files.
        /// </summary>
        public IQueryable<File> Files => this.set;

        /// <summary>
        /// Create file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task CreateAsync(File file)
        {
            this.set.Add(file);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// Delete file from database.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task DeleteAsync(File file)
        {
            this.set.Remove(file);
            await this.db.SaveChangesAsync();
        }

        /// <summary>
        /// Check wheather file is exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(string id)
        {
            return Task.FromResult(this.set.Any(f => f.Id == id));
        }

        /// <summary>
        /// Find file by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<File> FindByIdAsync(string id)
        {
            return await this.set.FindAsync(id);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task UpdateAsync(File file)
        {
            this.db.Entry(file).State = EntityState.Modified;
            await this.db.SaveChangesAsync();
        }
    }
}
