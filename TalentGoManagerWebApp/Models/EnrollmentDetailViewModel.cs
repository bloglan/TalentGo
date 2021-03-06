﻿using System;
using System.ComponentModel.DataAnnotations;
using TalentGo;

namespace TalentGoManagerWebApp.Models
{
    public class EnrollmentDetailViewModel
	{
		public int ID { get; set; }

		public Guid UserID { get; set; }

		public ApplicationForm Enrollment { get; set; }

		[Display(Name = "通过")]
		public bool Approved { get; set; }

		[Display(Name = "未通过")]
		public bool Rejective { get; set; }

		[Display(Name = "附加信息")]
		public string AuditMessage { get; set; }
	}
}