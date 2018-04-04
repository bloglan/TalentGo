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
    public class NoneOperationEmailService : IEmailService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task SendAsync(string[] to, string title, string content, params object[] args)
        {
            return Task.FromResult(0);
        }
    }
}
