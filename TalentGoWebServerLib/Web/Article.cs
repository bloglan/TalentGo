namespace TalentGo.Web
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
        /// ����ʱ�䡣
        /// </summary>
        public DateTime? WhenPublished { get; set; }
    }
}
