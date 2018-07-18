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
    public class NoticeStore : INoticeStore
    {
        DbContext dbContext;
        DbSet<Notice> set;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DbContext"></param>
        public NoticeStore(DbContext DbContext)
        {
            this.dbContext = DbContext;
            this.set = this.dbContext.Set<Notice>();
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<Notice> Articles
        {
            get
            {
                return this.set;
            }
        }

        /// <summary>
        /// Find
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Notice> FindByIdAsync(int id)
        {
            return await this.set.FindAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public async Task CreateAsync(Notice article)
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
        public async Task RemoveAsync(Notice article)
        {
            this.set.Remove(article);
            await this.dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Notice article)
        {
            this.dbContext.Entry(article).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync();
        }
    }
}
