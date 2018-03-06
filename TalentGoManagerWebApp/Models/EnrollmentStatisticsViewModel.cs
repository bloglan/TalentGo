namespace TalentGoManagerWebApp.Models
{
	public class EnrollmentStatisticsViewModel
	{

		public int CommitedEnrollmentCount { get; set; }

		public int ApprovedEnrollmentCount { get; set; }

		public int RejectiveEnrollmentCount { get; set; }

		public int NotAuditEnrollmentCount { get; set; }

		public int AnnouncedTakeExamCount { get; set; }

		public int AnnouncedNotTakeExamCount { get; set; }

		public int NotAnnouncedCount { get; set; }
	}
}