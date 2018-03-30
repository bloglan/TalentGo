using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task SendAsync(string[] to, string title, string content, params object[] args);
    }
}
