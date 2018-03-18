using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Web
{
    /// <summary>
    /// Notice Extensions.
    /// </summary>
    public static class NoticeExtensions
    {
        /// <summary>
        /// Get published notice collection.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<Notice> Published(this IQueryable<Notice> source)
        {
            return source.Where(n => n.WhenPublished.HasValue);
        }
    }
}
