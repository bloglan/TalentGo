﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TalentGo.Recruitment;

namespace TalentGoWebApp.Areas.Mgmt.Models
{
    public class EnrollmentListViewModel
	{
		public EnrollmentListViewModel()
		{
			this.OrderColumn = "WhenCommited";
			this.DownDirection = true;
			this.AuditFilter = AuditFilterType.All;
			this.PageSize = 30;
		}
		public string RecruitmentPlanTitle { get; set; }

		public int RecruitmentPlanID { get; set; }

		public bool IsAudit { get; set; }

        [Display(Name = "学校名称")]
        public string CollegeName { get; set; }

		public string MajorCategory { get; set; }

		public AuditFilterType AuditFilter { get; set; }

		public AnnounceFilterType AnnounceFilter { get; set; }

		public bool? AnnouncedForExam { get; set; }

		[Display(Name = "搜索")]
		public string Keywords { get; set; }

		public string OrderColumn { get; set; }

		public bool DownDirection { get; set; }

		public int PageIndex { get; set; }

		public int PageSize { get; set; }

		public int AllCount { get; set; }

		public IEnumerable<Enrollment> EnrollmentList { get; set; }
	}
}