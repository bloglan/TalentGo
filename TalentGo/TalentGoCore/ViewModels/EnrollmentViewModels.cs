using System;
using System.ComponentModel.DataAnnotations;

namespace TalentGo.ViewModels
{
    public class EnrollmentViewModel
	{


		[Required]
		[Display(Name = "姓名")]
		[StringLength(5, MinimumLength = 2)]
		public string Name { get; set; }

		[Required]
		[Display(Name = "性别")]
		public string Sex { get; set; }

		[Required]
		[Display(Name = "出生年月")]
		[DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }

		[Required]
		[Display(Name = "民族")]
		public string Nationality { get; set; }

		[Required]
		[Display(Name = "籍贯")]
		[StringLength(15)]
		public string PlaceOfBirth { get; set; }

		[Required]
		[Display(Name = "生源地")]
		[StringLength(15)]
		public string Source { get; set; }

		[Required]
		[Display(Name = "政治面貌")]
		public string PoliticalStatus { get; set; }

		[Required]
		[Display(Name = "健康状况")]
		[StringLength(10)]
		public string Health { get; set; }

		[Required]
		[Display(Name = "婚姻状况")]
		public string Marriage { get; set; }

		[Required]
		[Display(Name = "身份证号码")]
		[StringLength(18)]
		public string IDCardNumber { get; set; }

		[Required]
		[Display(Name = "移动电话号码")]
		[StringLength(11)]
		public string Mobile { get; set; }

		[Required]
		[Display(Name = "毕业院校")]
		[StringLength(50)]
		public string School { get; set; }

		[Required]
		[Display(Name = "毕业专业")]
		[StringLength(50)]
		public string Major { get; set; }

		[Required]
		[Display(Name = "毕业日期")]
		[DataType(DataType.Date)]
		public DateTime WhenGraduated { get; set; }

		[Required]
		[Display(Name = "报考专业")]
		public string SelectedMajor { get; set; }

		[Required]
		[Display(Name = "学历")]
		public string EducationBackground { get; set; }

		[Required]
		[Display(Name = "学位")]
		public string Degree { get; set; }

		[Required]
		[Display(Name = "简历")]
		[DataType(DataType.MultilineText)]
		public string Resume { get; set; }

		[Required]
		[Display(Name = "自我介绍及特长")]
		[DataType(DataType.MultilineText)]
		public string Accomplishments { get; set; }
	}
}
