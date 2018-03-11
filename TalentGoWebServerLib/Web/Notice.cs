namespace TalentGo.Web
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 表示一个文章。
    /// </summary>
    public partial class Notice
    {
        /// <summary>
        /// Default ctor.
        /// </summary>
        protected Notice()
        {
            this.WhenCreated = DateTime.Now;
        }

        /// <summary>
        /// Initialize a new notice with title, main content and created by.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="mainContent"></param>
        /// <param name="createdBy"></param>
        public Notice(string title, string mainContent, string createdBy)
            : this()
        {
            this.Title = title;
            this.MainContent = mainContent;
            this.CreatedBy = createdBy;
        }

        /// <summary>
        /// 文章id
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// Article title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Primary content of Article.
        /// </summary>
        public string MainContent { get; set; }

        /// <summary>
        /// Creator of this article.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// When created.
        /// </summary>
		public DateTime WhenCreated { get; protected set; }

        /// <summary>
        /// When changed.
        /// </summary>
		public DateTime WhenChanged { get; set; }

        /// <summary>
        /// Is visible.
        /// </summary>
		public bool Visible { get; set; }

        /// <summary>
        /// 发布时间。
        /// </summary>
        public DateTime? WhenPublished { get; set; }
    }
}
