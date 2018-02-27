namespace TalentGo.Utilities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// ��ʾһ�����¡�
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
        /// ����id
        /// </summary>
        public int id { get; protected set; }

        /// <summary>
        /// Article title.
        /// </summary>
		[Display(Name = "����")]
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// Article summary.
        /// </summary>
		[Display(Name = "��Ҫ")]
		[StringLength(150)]
        public string Summary { get; set; }

        /// <summary>
        /// Primary content of Article.
        /// </summary>
		[Display(Name = "����")]
		[Required]
		[UIHint("CKEditor")]
        public string MainContent { get; set; }

        /// <summary>
        /// Creator of this article.
        /// </summary>
		[Display(Name = "������")]
		[Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// When created.
        /// </summary>
		[Display(Name = "����ʱ��")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime WhenCreated { get; protected set; }

        /// <summary>
        /// When changed.
        /// </summary>
		[Display(Name = "�޸�ʱ��")]
		public DateTime WhenChanged { get; set; }

        /// <summary>
        /// Is visible.
        /// </summary>
		[Display(Name = "��ʾ")]
		public bool Visible { get; set; }

        /// <summary>
        /// Present wheather realted to a recruitment plan via plan's id.
        /// </summary>
		[Display(Name = "�����ƻ�ID")]
		public int? RelatedPlan { get; set; }

        /// <summary>
        /// Present a value indicate wheather this article displayed in public area or private.
        /// </summary>
		[Display(Name = "����")]
		[UIHint("PublicFlag")]
		public bool? IsPublic { get; set; }

    }
}