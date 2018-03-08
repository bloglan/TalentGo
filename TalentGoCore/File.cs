using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 表示一个文件。
    /// </summary>
    public class File
    {
        /// <summary>
        /// Protected Initlalize File.
        /// </summary>
        protected File()
        {
            this.WhenCreated = DateTime.Now;
        }

        /// <summary>
        /// 使用一个Id和MimeType初始化File.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mimeType"></param>
        public File(string id, string mimeType)
            : this()
        {
            this.Id = id;
            this.MimeType = mimeType;
        }

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// MIME Type
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public virtual byte[] Data { get; set; }

        /// <summary>
        /// WhenCreated
        /// </summary>
        public DateTime WhenCreated { get; protected set; }

        /// <summary>
        /// Write data to stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task WriteAsync(Stream stream)
        {
            await stream.WriteAsync(Data, 0, Data.Length);
        }

        /// <summary>
        /// Read data from stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task ReadAsync(Stream stream)
        {
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);

            memoryStream.Position = 0;
            byte[] dataBytes = new byte[memoryStream.Length];
            await memoryStream.ReadAsync(dataBytes, 0, dataBytes.Length);

            this.Data = dataBytes;
        }
    }
}
