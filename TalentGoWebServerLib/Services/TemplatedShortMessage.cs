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
    public class TemplatedShortMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string[] To { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object[] Args { get; set; }
    }
}
