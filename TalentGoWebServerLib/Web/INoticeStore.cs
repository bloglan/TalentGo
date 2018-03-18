using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Web
{
    /// <summary>
    /// Defined interface for store Article.
    /// </summary>
    public interface INoticeStore : IDisposable
    {
        /// <summary>
        /// Get a qeuryable collection for all articles.
        /// </summary>
        IQueryable<Notice> Articles { get; }

        /// <summary>
        /// Find Notice by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Notice> FindByIdAsync(int id);

        /// <summary>
        /// insert an article into data store.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        Task CreateAsync(Notice article);

        /// <summary>
        /// Update an article.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        Task UpdateAsync(Notice article);

        /// <summary>
        /// Remove an article from data store.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        Task RemoveAsync(Notice article);
    }
}
