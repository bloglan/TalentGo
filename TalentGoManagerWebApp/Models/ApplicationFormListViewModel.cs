using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TalentGo;
using TalentGoManagerWebApp.Utilities;

namespace TalentGoManagerWebApp.Models
{
    public class ApplicationFormListViewModel : IPaging
	{
		public ApplicationFormListViewModel()
		{
			this.OrderColumn = "WhenCommited";
			this.DownDirection = true;
			this.AuditFilter = AuditFilterType.All;
			this.PageSize = 30;
		}


		public AuditFilterType AuditFilter { get; set; }

		[Display(Name = "搜索")]
		public string Keywords { get; set; }

		public string OrderColumn { get; set; }

		public bool DownDirection { get; set; }

		public int PageIndex { get; set; }

		public int PageSize { get; set; }

		public int AllCount { get; set; }

		public IEnumerable<ApplicationForm> EnrollmentList { get; set; }
	}
}