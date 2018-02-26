using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// Job Store.
    /// </summary>
    public interface IJobStore
    {
        /// <summary>
        /// Jobs
        /// </summary>
        IQueryable<Job> Jobs { get; }
    }
}
