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
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(a => a.WhenCommited.HasValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<ApplicationForm> Commited(this IEnumerable<ApplicationForm> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(a => a.WhenCommited.HasValue);
        }

        /// <summary>
        /// Get Pending file review application form collection.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<ApplicationForm> PendingFileReview(this IQueryable<ApplicationForm> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            //已提交，没有资料审查时间的。
            return source.Commited().Where(a => !a.WhenFileReviewed.HasValue);
        }

        /// <summary>
        /// 待审核。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<ApplicationForm> Auditable(this IQueryable<ApplicationForm> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Commited().Where(a => !a.WhenAuditComplete.HasValue && a.FileReviewAccepted.Value);
        }

        /// <summary>
        /// 审核通过的。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IQueryable<ApplicationForm> Approved(this IQueryable<ApplicationForm> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Commited().Where(a => a.WhenAuditComplete.HasValue && a.AuditFlag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<ApplicationForm> Approved(this IEnumerable<ApplicationForm> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Commited().Where(a => a.WhenAuditComplete.HasValue && a.AuditFlag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="planId"></param>
        /// <returns></returns>
        public static IQueryable<ApplicationForm> OfPlan(this IQueryable<ApplicationForm> source, int planId)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Where(f => f.PlanId == planId);
        }
    }
}
