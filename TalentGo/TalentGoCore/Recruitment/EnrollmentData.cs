using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalentGo.Identity;

namespace TalentGo.Recruitment
{
    /// <summary>
    /// 表示一个报名表。
    /// </summary>
	[Table("EnrollmentData")]
	public class EnrollmentData
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public EnrollmentData()
		{
			EnrollmentArchives = new HashSet<EnrollmentArchives>();
		}

		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int RecruitPlanID { get; set; }

		[Key]
		[Column(Order = 1)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int UserID { get; set; }

		[Display(Name = "姓名")]
		[Required]
		[StringLength(5, MinimumLength = 2)]
		public string Name { get; set; }

		[Display(Name = "性别")]
		[Required]
		[StringLength(2)]
		public string Sex { get; set; }

		[Display(Name = "出生日期")]
		[DataType(DataType.Date)]
		[Column(TypeName = "date")]
		public DateTime DateOfBirth { get; set; }

		[Display(Name = "民族")]
		[Required]
		[StringLength(5)]
		public string Nationality { get; set; }

		[Display(Name = "籍贯")]
		[Required]
		[StringLength(50)]
		public string PlaceOfBirth { get; set; }

		[Display(Name = "家庭现居地")]
		[Required]
		[StringLength(100)]
		public string Source { get; set; }

		[Display(Name = "政治面貌")]
		[Required]
		[StringLength(15)]
		public string PoliticalStatus { get; set; }

		[Display(Name = "健康状况")]
		[Required]
		[StringLength(10)]
		public string Health { get; set; }

		[Display(Name = "婚姻状况")]
		[Required]
		[StringLength(5)]
		public string Marriage { get; set; }

		[Display(Name = "身份证号码")]
		[Required]
		[StringLength(18)]
		public string IDCardNumber { get; set; }

		[Display(Name = "手机号码")]
		[Required]
		[StringLength(15)]
		public string Mobile { get; set; }

		[Display(Name = "毕业院校")]
		[Required]
		[StringLength(50)]
		public string School { get; set; }

		[Display(Name = "毕业专业")]
		[Required]
		[StringLength(50)]
		public string Major { get; set; }

		[Display(Name = "毕业年度")]
		public int YearOfGraduated { get; set; }

		[Display(Name = "报考专业")]
		[Required]
		[StringLength(15)]
		public string SelectedMajor { get; set; }

		[Display(Name = "学历")]
		[Required]
		[StringLength(15)]
		public string EducationBackground { get; set; }

		[Display(Name = "学位")]
		[Required]
		[StringLength(15)]
		public string Degree { get; set; }

		[Display(Name = "简历")]
		[Required]
		[StringLength(200)]
		[DataType(DataType.MultilineText)]
		public string Resume { get; set; }

		[Display(Name = "特长及自我介绍")]
		[StringLength(1000)]
		[DataType(DataType.MultilineText)]
		public string Accomplishments { get; set; }

		[Display(Name = "创建时间")]
		public DateTime WhenCreated { get; set; }

		[Display(Name = "修改时间")]
		public DateTime? WhenChanged { get; set; }

		[Display(Name = "提交时间")]
		public DateTime? WhenCommited { get; set; }

		[Display(Name = "审核时间")]
		public DateTime? WhenAudit { get; set; }

		[Display(Name = "已批准")]
		public bool? Approved { get; set; }

		[Display(Name = "审核消息")]
		[StringLength(50)]
		public string AuditMessage { get; set; }

		[Display(Name = "声明时间")]
		public DateTime? WhenAnnounced { get; set; }

		[Display(Name = "参加考试")]
		public bool? IsTakeExam { get; set; }

		public virtual Degree Degree1 { get; set; }

		public virtual EducationBackground EducationBackground1 { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<EnrollmentArchives> EnrollmentArchives { get; set; }

		public virtual MajorCategory MajorCategory { get; set; }

		public virtual RecruitmentPlan RecruitmentPlan { get; set; }

		public virtual TargetUser Users { get; set; }
	}
}
