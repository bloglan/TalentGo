using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Web
{
    /// <summary>
    /// 数据输出器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataExporter<T>
        where T : class
    {
        /// <summary>
        /// 输出CSV格式
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public void ExportCSV(IEnumerable<T> source, Stream dest)
        {
            
        }
    }
}
