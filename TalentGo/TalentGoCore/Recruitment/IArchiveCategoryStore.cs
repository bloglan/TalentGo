using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    /// <summary>
    /// 提供资料类别项的数据存储访问能力。
    /// </summary>
    public interface IArchiveCategoryStore : IDisposable
    {
        IQueryable<ArchiveCategory> ArchiveCategories { get; }

        /// <summary>
        /// 根据名称查找资料项。
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ArchiveCategory> FindByIdAsync(int Id);

        Task CreateAsync(ArchiveCategory archiveCategory);

        Task UpdateAsync(ArchiveCategory archiveCategory);

        Task DeleteAsync(ArchiveCategory archiveCategory);
    }
}
