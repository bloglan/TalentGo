using System;
using System.ComponentModel.DataAnnotations;
using TalentGo;

namespace TalentGoManagerWebApp.Models
{
    public class RecruitmentPlanPublishViewModel
	{
		public RecruitmentPlanPublishViewModel()
		{
			this.EnrollExpirationDate = DateTime.Now.AddDays(15);
		}
		public RecruitmentPlan Plan { get; set; }

		[Display(Name = "报名截止日期")]
		[Required]
		[UIHint("DateTimeMinute")]
		public DateTime EnrollExpirationDate { get; set; }
	}
}