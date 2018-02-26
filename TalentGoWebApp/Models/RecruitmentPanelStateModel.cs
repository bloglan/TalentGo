using TalentGo;

namespace TalentGoWebApp.Models
{
    public class RecruitmentPanelStateModel
	{
		public bool HasEnrollment { get; set; }

		public RecruitmentPlan Plan { get; set; }

		public ApplicationForm Enrollment { get; set; }
	}
}