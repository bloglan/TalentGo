using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 表示一个文件存储。
    /// </summary>
    public interface IFileStore
    {
        /// <summary>
        /// 将文件流写入到指定的key。
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task CreateAsync(File file);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task UpdateAsync(File file);

        /// <summary>
        /// 删除指定的文件。
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task DeleteAsync(File file);

        /// <summary>
        /// 获取一个值，指示指定的文件是否存在。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string id);

        /// <summary>
        /// Find file by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<File> FindByIdAsync(string id);

        /// <summary>
        /// Get all files.
        /// </summary>
        IQueryable<File> Files { get; }
    }
}
