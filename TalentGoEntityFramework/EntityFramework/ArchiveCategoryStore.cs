using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class ArchiveCategoryStore : IArchiveCategoryStore
    {
        DbContext dbContext;
        DbSet<ArchiveCategory> set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public ArchiveCategoryStore(DbContext dbContext)
        {
            this.dbContext = dbContext;
            this.set = this.dbContext.Set<ArchiveCategory>();
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<ArchiveCategory> ArchiveCategories
        {
            get
            {
                return this.set;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveCategory"></param>
        /// <returns></returns>
        public async Task CreateAsync(ArchiveCategory archiveCategory)
        {
            archiveCategory.WhenCreated = DateTime.Now;
            archiveCategory.WhenChanged = DateTime.Now;

            this.set.Add(archiveCategory);
            await this.dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveCategory"></param>
        /// <returns></returns>
        public async Task DeleteAsync(ArchiveCategory archiveCategory)
        {
            var current = this.set.FirstOrDefault(a => a.Id == archiveCategory.Id);
            if (current != null)
            {
                this.set.Remove(current);
                await this.dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ArchiveCategory> FindByIdAsync(int Id)
        {
            return this.set.FirstOrDefault(a => a.Id == Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveCategory"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ArchiveCategory archiveCategory)
        {

            this.dbContext.Entry(archiveCategory).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        /// <summary>
        /// 
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
        // ~ArchiveCategoryStore() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        /// <summary>
        /// 
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
