using System.ComponentModel.DataAnnotations;

namespace TalentGoWebApp.Models
{
	public class CreateTargetUserViewModel
	{
		[Display(Name = "身份证号码")]
		[Required]
		[StringLength(18)]
		public string IDCardNumber { get; set; }

		[Display(Name = "手机号码")]
		[Required]
		[StringLength(11)]
        [RegularExpression(@"^[1]+[3,4,5,7,8]+\d{9}$", ErrorMessage = "非法手机号。")]
		public string Mobile { get; set; }

		[Display(Name = "电子邮件地址")]
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Display(Name = "姓名")]
		[Required]
		[StringLength(5, MinimumLength = 2)]
		public string DisplayName { get; set; }
	}
}