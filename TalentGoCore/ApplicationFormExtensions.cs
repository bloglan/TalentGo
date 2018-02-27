using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// Extension method for ApplicationForm Collection.
    /// </summary>
    public static class ApplicationFormExtensions
    {
        /// <summary>
        /// Get Commited ApplicationForm collection.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<ApplicationForm> CommitedForms(this IQueryable<ApplicationForm> source)
        {
            return source.Where(a => a.WhenCommited.HasValue);
        }
    }
}
