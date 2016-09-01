namespace TalentGo.Utilities
{
	using Recruitment;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;
	using System.Web.Mvc;
	[Table("Article")]
    public partial class Article
    {
        public int id { get; set; }

		[Display(Name = "标题")]
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

		[Display(Name = "概要")]
		[StringLength(150)]
        public string Summary { get; set; }

		[Display(Name = "正文")]
		[Required]
		[AllowHtml]
		[UIHint("CKEditor")]
        public string MainContent { get; set; }

		[Display(Name = "创建者")]
		[Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

		[Display(Name = "创建时间")]
		[HiddenInput]
		public DateTime WhenCreated { get; set; }

		[Display(Name = "修改时间")]
		[HiddenInput]
		public DateTime WhenChanged { get; set; }

		[Display(Name = "显示")]
		public bool Visible { get; set; }

		[Display(Name = "关联计划ID")]
		public int? RelatedPlan { get; set; }

		[Display(Name = "公开")]
		[UIHint("PublicFlag")]
		public bool? IsPublic { get; set; }

        public virtual RecruitmentPlan RecruitmentPlan { get; set; }
    }
}
