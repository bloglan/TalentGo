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
        public static IQueryable<ApplicationForm> Commited(this IQueryable<ApplicationForm> source)
        {
            return source.Where(a => a.WhenCommited.HasValue);
        }

        /// <summary>
        /// Get Pending file review application form collection.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<ApplicationForm> PendingFileReview(this IQueryable<ApplicationForm> source)
        {
            //已提交，没有资料审查时间的。
            return source.Commited().Where(a => !a.WhenFileReview.HasValue);
        }

        /// <summary>
        /// 待审核。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<ApplicationForm> PendingAudit(this IQueryable<ApplicationForm> source)
        {
            return source.Where(a => a.FileReviewAccepted.Value && !a.WhenAudit.HasValue);
        }

        /// <summary>
        /// 审核通过的。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<ApplicationForm> Approved(this IQueryable<ApplicationForm> source)
        {
            return source.Where(a => a.WhenAudit.HasValue && a.Approved.Value);
        }
    }
}
