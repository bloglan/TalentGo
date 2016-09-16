using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Utilities
{
    /// <summary>
    /// Article data store via EF
    /// </summary>
    public class ArticleStore : IArticleStore
    {
        TalentGoDbContext dbContext;
        public ArticleStore(TalentGoDbContext DbContext)
        {
            this.dbContext = DbContext;
        }

        public IQueryable<Article> Articles
        {
            get
            {
                return this.dbContext.Article.AsNoTracking();
            }
        }

        public async Task CreateAsync(Article article)
        {
            //article.WhenCreated = DateTime.Now;
            //article.WhenChanged = DateTime.Now;

            this.dbContext.Article.Add(article);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Article article)
        {
            if (article == null)
                throw new ArgumentNullException(nameof(article));

            var current = await this.dbContext.Article.FindAsync(article.id);
            if (current == null)
                throw new ArgumentException("Can not find article in any data store.");

            this.dbContext.Article.Remove(current);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Article article)
        {
            var current = this.dbContext.Article.FirstOrDefault(a => a.id == article.id);

            var currentEntry = this.dbContext.Entry<Article>(current);
            currentEntry.CurrentValues.SetValues(article);
            //currentEntry.Property(p => p.WhenCreated).IsModified = false;

            current.WhenChanged = DateTime.Now;

            await this.dbContext.SaveChangesAsync();
        }
    }
}
