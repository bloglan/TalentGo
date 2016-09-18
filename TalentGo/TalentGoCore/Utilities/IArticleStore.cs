using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Utilities
{
    /// <summary>
    /// Defined interface for store Article.
    /// </summary>
    public interface IArticleStore : IDisposable
    {
        /// <summary>
        /// Get a qeuryable collection for all articles.
        /// </summary>
        IQueryable<Article> Articles { get; }

        /// <summary>
        /// insert an article into data store.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        Task CreateAsync(Article article);

        /// <summary>
        /// Update an article.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        Task UpdateAsync(Article article);

        /// <summary>
        /// Remove an article from data store.
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        Task RemoveAsync(Article article);
    }
}
