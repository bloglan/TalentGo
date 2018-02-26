using System;
using System.ComponentModel.DataAnnotations;
using TalentGo;

namespace TalentGoWebApp.Areas.Mgmt.Models
{
    public class CommitAuditViewModel
	{
		public RecruitmentPlan Plan { get; set; }

		[Display(Name = "声明考试截止日期")]
		[UIHint("DateTimeMinute")]
		public DateTime AnnounceExpirationDate { get; set; }

		[Display(Name = "考试开始时间")]
		[UIHint("DateTimeMinute")]
		public DateTime ExamStartTime { get; set; }

		[Display(Name = "考试结束时间")]
		[UIHint("DateTimeMinute")]
		public DateTime ExamEndTime { get; set; }

		[Display(Name = "考试地点")]
		[Required]
		public string ExamLocation { get; set; }
	}
}