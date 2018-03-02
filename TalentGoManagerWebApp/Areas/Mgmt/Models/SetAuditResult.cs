using System;

namespace TalentGoWebApp.Areas.Mgmt.Models
{
	public class SetAuditResult
	{
		public SetAuditResult(int formId)
		{
			this.FormId = formId;
		}
		public int FormId { get; set; }

		public int Code { get; set; }

		public string Message { get; set; }

		public EnrollmentStatisticsViewModel Statistics { get; set; }
	}
}