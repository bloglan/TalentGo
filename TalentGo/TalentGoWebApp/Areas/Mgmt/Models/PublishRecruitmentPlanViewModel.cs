using System;
using System.ComponentModel.DataAnnotations;
using TalentGo.Recruitment;

namespace TalentGoWebApp.Areas.Mgmt.Models
{
    public class PublishRecruitmentPlanViewModel
	{
		public PublishRecruitmentPlanViewModel()
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