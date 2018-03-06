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
        /// <param name="key"></param>
        /// <param name="mimeType"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task SaveAsync(string key, string mimeType, Stream stream);

        /// <summary>
        /// 将指定key的文件写入指定流。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task LoadAsync(string key, Stream stream);

        /// <summary>
        /// 获取一个值，指示指定的文件是否存在。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// 删除指定的文件。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task DeleteAsync(string key);

        /// <summary>
        /// 获取指定文件的MIME类型。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetMimeTypeAsync(string key);

        /// <summary>
        /// 设置指定文件的MIME类型。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        Task SetMimeTypeAsync(string key, string mimeType);
    }
}
