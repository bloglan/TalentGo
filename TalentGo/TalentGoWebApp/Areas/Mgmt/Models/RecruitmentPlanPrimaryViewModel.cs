using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TalentGoWebApp.Areas.Mgmt.Models
{
	public class RecruitmentPlanPrimaryViewModel
	{
		public RecruitmentPlanPrimaryViewModel()
		{
			this.ExpirationDate = DateTime.Now.AddMonths(3);
		}

		[Display(Name = "计划标题")]
		[Required]
		public string Title { get; set; }

		[Display(Name = "招聘简章")]
		[Required]
		[UIHint("CKEditor")]
		[AllowHtml]
		public string Recruitment { get; set; }

		[Display(Name = "是否外部招聘计划")]
		public bool IsPublic { get; set; }

		[Display(Name = "有效期至")]
		[DataType(DataType.Date)]
		[Required]
		public DateTime ExpirationDate { get; set; }

		[Display(Name = "发布者")]
		[Required]
		public string Publisher { get; set; }
	}
}