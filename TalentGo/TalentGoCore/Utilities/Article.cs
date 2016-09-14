namespace TalentGo.Utilities
{
    using Recruitment;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("Article")]
    public partial class Article
    {
        public int id { get; set; }

		[Display(Name = "����")]
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

		[Display(Name = "��Ҫ")]
		[StringLength(150)]
        public string Summary { get; set; }

		[Display(Name = "����")]
		[Required]
		[UIHint("CKEditor")]
        public string MainContent { get; set; }

		[Display(Name = "������")]
		[Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

		[Display(Name = "����ʱ��")]
		public DateTime WhenCreated { get; set; }

		[Display(Name = "�޸�ʱ��")]
		public DateTime WhenChanged { get; set; }

		[Display(Name = "��ʾ")]
		public bool Visible { get; set; }

		[Display(Name = "�����ƻ�ID")]
		public int? RelatedPlan { get; set; }

		[Display(Name = "����")]
		[UIHint("PublicFlag")]
		public bool? IsPublic { get; set; }

        public virtual RecruitmentPlan RecruitmentPlan { get; set; }
    }
}
