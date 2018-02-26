using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 提供资料类别项的数据存储访问能力。
    /// </summary>
    public interface IArchiveCategoryStore : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<ArchiveCategory> ArchiveCategories { get; }

        /// <summary>
        /// 根据名称查找资料项。
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ArchiveCategory> FindByIdAsync(int Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveCategory"></param>
        /// <returns></returns>
        Task CreateAsync(ArchiveCategory archiveCategory);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveCategory"></param>
        /// <returns></returns>
        Task UpdateAsync(ArchiveCategory archiveCategory);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archiveCategory"></param>
        /// <returns></returns>
        Task DeleteAsync(ArchiveCategory archiveCategory);
    }
}
