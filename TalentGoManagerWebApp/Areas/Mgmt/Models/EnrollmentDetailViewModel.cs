using System.ComponentModel.DataAnnotations;
using TalentGo;

namespace TalentGoWebApp.Areas.Mgmt.Models
{
    public class EnrollmentDetailViewModel
	{
		public int ID { get; set; }

		public int UserID { get; set; }

		public Enrollment Enrollment { get; set; }

		[Display(Name = "通过")]
		public bool Approved { get; set; }

		[Display(Name = "未通过")]
		public bool Rejective { get; set; }

		[Display(Name = "附加信息")]
		public string AuditMessage { get; set; }
	}
}