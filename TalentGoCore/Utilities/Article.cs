namespace TalentGo.Utilities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 表示一个文章。
    /// </summary>
    [Table("Article")]
    public partial class Article
    {
        /// <summary>
        /// Default ctor.
        /// </summary>
        public Article()
        {
        }

        /// <summary>
        /// 文章id
        /// </summary>
        public int id { get; protected set; }

        /// <summary>
        /// Article title.
        /// </summary>
		[Display(Name = "标题")]
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// Article summary.
        /// </summary>
		[Display(Name = "概要")]
		[StringLength(150)]
        public string Summary { get; set; }

        /// <summary>
        /// Primary content of Article.
        /// </summary>
		[Display(Name = "正文")]
		[Required]
		[UIHint("CKEditor")]
        public string MainContent { get; set; }

        /// <summary>
        /// Creator of this article.
        /// </summary>
		[Display(Name = "创建者")]
		[Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// When created.
        /// </summary>
		[Display(Name = "创建时间")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime WhenCreated { get; protected set; }

        /// <summary>
        /// When changed.
        /// </summary>
		[Display(Name = "修改时间")]
		public DateTime WhenChanged { get; set; }

        /// <summary>
        /// Is visible.
        /// </summary>
		[Display(Name = "显示")]
		public bool Visible { get; set; }

        /// <summary>
        /// Present wheather realted to a recruitment plan via plan's id.
        /// </summary>
		[Display(Name = "关联计划ID")]
		public int? RelatedPlan { get; set; }

        /// <summary>
        /// Present a value indicate wheather this article displayed in public area or private.
        /// </summary>
		[Display(Name = "公开")]
		[UIHint("PublicFlag")]
		public bool? IsPublic { get; set; }

    }
}
