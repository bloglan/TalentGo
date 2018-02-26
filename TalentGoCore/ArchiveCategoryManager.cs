using System.Linq;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 表示一个文档管理器，
    /// </summary>
    public class ArchiveCategoryManager
    {
        IArchiveCategoryStore store;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Store"></param>
        public ArchiveCategoryManager(IArchiveCategoryStore Store)
        {
            this.store = Store;
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<ArchiveCategory> ArchiveCategories { get { return this.store.ArchiveCategories; } }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<ArchiveCategory> AvailableArchiveCategories
        {
            get
            {
                return this.ArchiveCategories.Where(ac => ac.Enabled);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveCategory"></param>
        /// <returns></returns>
        public async Task CreateArchiveCategory(ArchiveCategory archiveCategory)
        {
            await this.store.CreateAsync(archiveCategory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveCategory"></param>
        /// <returns></returns>
        public async Task UpdateArchiveCategory(ArchiveCategory archiveCategory)
        {
            await this.store.UpdateAsync(archiveCategory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveCategory"></param>
        /// <returns></returns>
        public async Task DeleteArchiveCategory(ArchiveCategory archiveCategory)
        {
            await this.store.DeleteAsync(archiveCategory);
        }





    }
}
