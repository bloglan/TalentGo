using System;
using System.Linq;
using System.Threading.Tasks;
using TalentGo.Utilities;

namespace TalentGo.EntityFramework
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

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ArticleStore() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
