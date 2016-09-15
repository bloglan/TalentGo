using System;
using System.Linq;
using System.Threading.Tasks;
using TalentGo.Recruitment;

namespace TalentGo.Utilities
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
    }
}
