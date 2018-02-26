using TalentGo.Recruitment;

namespace TalentGoWebApp.Areas.Mgmt.Models
{
    public class ArchiveRequirementsViewModel
	{
		public ArchiveCategory ArchiveCategory { get; set; }

		public bool Enabled { get; set; }

		public string RequirementType { get; set; }
	}
}