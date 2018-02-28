using System;

namespace TalentGoWebApp.Areas.Mgmt.Models
{
	public class SetAuditResult
	{
		public SetAuditResult(int PlanID, Guid UserID)
		{
			this.PlanID = PlanID;
			this.UserID = UserID;
		}
		public int PlanID { get; set; }

		public Guid UserID { get; set; }

		public int Code { get; set; }

		public string Message { get; set; }

		public EnrollmentStatisticsViewModel Statistics { get; set; }
	}
}