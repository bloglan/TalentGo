using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TalentGo.Web;

namespace TalentGo.EntityFramework
{
    /// <summary>
    /// Article data store via EF
    /// </summary>
    public class ArticleStore : IArticleStore
    {
        DbContext dbContext;
        DbSet<Article> set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DbContext"></param>
        public ArticleStore(DbContext DbContext)
        {
            this.dbContext = DbContext;
            this.set = this.dbContext.Set<Article>();
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<Article> Articles
        {
            get
            {
                return this.set;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public async Task CreateAsync(Article article)
        {
            //article.WhenCreated = DateTime.Now;
            //article.WhenChanged = DateTime.Now;

            this.set.Add(article);
            await this.dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public async Task RemoveAsync(Article article)
        {
            this.set.Remove(article);
            await this.dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Article article)
        {
            this.dbContext.Entry(article).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
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
        /// <summary>
        /// 
        /// </summary>
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
