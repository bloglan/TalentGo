using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TalentGoManagerWebApp.Models
{
	public class RecruitmentPlanEditViewModel
	{
		public RecruitmentPlanEditViewModel()
		{
			this.ExpirationDate = DateTime.Now.AddMonths(3);
		}

		[Display(Name = "标题")]
		[Required]
		public string Title { get; set; }

		[Display(Name = "招聘简章")]
		[Required]
		[UIHint("CKEditor")]
		[AllowHtml]
		public string Recruitment { get; set; }

		[Display(Name = "报名截止时间")]
        [UIHint("DateTimeMinute")]
		public DateTime ExpirationDate { get; set; }
	}
}