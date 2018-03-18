using System;
using System.ComponentModel.DataAnnotations;
using TalentGo;

namespace TalentGoManagerWebApp.Models
{
    public class RecruitmentPlanPublishViewModel
	{
		public int PlanId { get; set; }

		[Display(Name = "报名截止时间")]
		[Required]
		[UIHint("DateTimeMinute")]
		public DateTime EnrollExpirationDate { get; set; }
	}
}