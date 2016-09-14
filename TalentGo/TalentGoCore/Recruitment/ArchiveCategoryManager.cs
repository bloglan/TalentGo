using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    /// <summary>
    /// 表示一个文档管理器，
    /// </summary>
    public class ArchiveCategoryManager
    {
        IArchiveCategoryStore store;

        public ArchiveCategoryManager(IArchiveCategoryStore Store)
        {
            this.store = Store;
        }

        public IQueryable<ArchiveCategory> ArchiveCategories { get { return this.store.ArchiveCategories; } }

        public IQueryable<ArchiveCategory> AvailableArchiveCategories
        {
            get
            {
                return this.ArchiveCategories.Where(ac => ac.Enabled);
            }
        }

        public async Task CreateArchiveCategory(ArchiveCategory archiveCategory)
        {
            await this.store.CreateAsync(archiveCategory);
        }

        public async Task UpdateArchiveCategory(ArchiveCategory archiveCategory)
        {
            await this.store.UpdateAsync(archiveCategory);
        }

        public async Task DeleteArchiveCategory(ArchiveCategory archiveCategory)
        {
            await this.store.DeleteAsync(archiveCategory);
        }





    }
}
