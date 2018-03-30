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
    public interface ITemplatedShortMessageService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="templateId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task SendAsync(string[] to, string templateId, params object[] args);
    }
}
