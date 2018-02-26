using TalentGo;

namespace TalentGoWebApp.Models
{
    public class RecruitmentPanelStateModel
	{
		public bool HasEnrollment { get; set; }

		public RecruitmentPlan Plan { get; set; }

		public Enrollment Enrollment { get; set; }
	}
}