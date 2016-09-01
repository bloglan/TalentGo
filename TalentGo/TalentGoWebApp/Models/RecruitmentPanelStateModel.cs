using TalentGo.Recruitment;

namespace TalentGoWebApp.Models
{
	public class RecruitmentPanelStateModel
	{
		public bool HasEnrollment { get; set; }

		public RecruitmentPlan Plan { get; set; }

		public EnrollmentData Enrollment { get; set; }
	}
}