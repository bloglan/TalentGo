using System;
using System.Linq;
using System.Threading.Tasks;
using TalentGo.Recruitment;

namespace TalentGo.EntityFramework
{
    public class ArchiveCategoryStore : IArchiveCategoryStore
    {
        TalentGoDbContext dbContext;

        public ArchiveCategoryStore(TalentGoDbContext DbContext)
        {
            this.dbContext = DbContext;
        }

        public IQueryable<ArchiveCategory> ArchiveCategories
        {
            get
            {
                return this.dbContext.ArchiveCategory.AsNoTracking();
            }
        }

        public async Task CreateAsync(ArchiveCategory archiveCategory)
        {
            archiveCategory.WhenCreated = DateTime.Now;
            archiveCategory.WhenChanged = DateTime.Now;

            this.dbContext.ArchiveCategory.Add(archiveCategory);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ArchiveCategory archiveCategory)
        {
            var current = this.dbContext.ArchiveCategory.FirstOrDefault(a => a.id == archiveCategory.id);
            if (current != null)
            {
                this.dbContext.ArchiveCategory.Remove(current);
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task<ArchiveCategory> FindByIdAsync(int Id)
        {
            return this.dbContext.ArchiveCategory.FirstOrDefault(a => a.id == Id);
        }

        public async Task UpdateAsync(ArchiveCategory archiveCategory)
        {
            var current = await this.FindByIdAsync(archiveCategory.id);
            if (current != null)
            {
                var entry = this.dbContext.Entry<ArchiveCategory>(current);
                entry.CurrentValues.SetValues(archiveCategory);
                entry.Property(p => p.WhenCreated).IsModified = false;

                current.WhenChanged = DateTime.Now;

                await this.dbContext.SaveChangesAsync();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

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
