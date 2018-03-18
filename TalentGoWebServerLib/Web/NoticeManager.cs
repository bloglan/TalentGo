using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class NoticeManager
	{
        INoticeStore store;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Store"></param>
		public NoticeManager(INoticeStore Store)
		{
            this.store = Store;
		}

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<Notice> Notices { get { return this.store.Articles; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public async Task<Notice> FindByIdAsync(int id)
		{
            return await this.store.FindByIdAsync(id);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public async Task CreateAsync(Notice item)
		{
			//article.WhenCreated = DateTime.Now;
			item.WhenChanged = DateTime.Now;

			await this.store.CreateAsync(item);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public async Task UpdateAsync(Notice item)
		{
            await this.store.UpdateAsync(item);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public async Task DeleteAsync(Notice item)
		{
            await this.store.RemoveAsync(item);
		}

        /// <summary>
        /// Publish
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task PublishAsync(Notice item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (item.WhenPublished.HasValue)
                return;

            item.WhenPublished = DateTime.Now;
            await this.store.UpdateAsync(item);
        }

        /// <summary>
        /// Cancel publish.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task CancelPublishAsync(Notice item)
        {
            if (item == null)
                throw new ArgumentNullException();
            if (!item.WhenPublished.HasValue)
                return;

            item.WhenPublished = null;
            await this.store.UpdateAsync(item);
        }
	}
}
