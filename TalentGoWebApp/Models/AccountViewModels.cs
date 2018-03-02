using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TalentGoWebApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "代码")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "记住此浏览器?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "身份证号码")]
        public string IDCardNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
		[Required]
		[Display(Name = "身份证号码")]
		public string IDCardNumber { get; set; }

		[Required]
		[Display(Name = "移动电话")]
        [RegularExpression(@"^[1]+[3,4,5,7,8]+\d{9}$", ErrorMessage = "非法手机号。")]
        public string Mobile { get; set; }

		[Display(Name = "验证码")]
		[Required]
		public string ValidateCode { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "姓氏")]
        [StringLength(10)]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "名字")]
        [StringLength(10)]
        public string GivenName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        //[RegularExpression(@"(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])[a-zA-Z0-9]{8,15}", ErrorMessage = "密码格式错误。")]
		//不需要
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViaEmailViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [Required]
        //[RegularExpression(@"(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])[a-zA-Z0-9]{8,15}", ErrorMessage = "密码格式错误。")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

	public class ResetPasswordViaMobileViewModel
	{
		[Required]
		[Display(Name = "手机号码")]
		[RegularExpression(@"^[1]+[3,4,5,7,8]+\d{9}$", ErrorMessage = "非法手机号。")]
		public string Mobile { get; set; }

		[Required]
		[Display(Name = "验证码")]
		public string ValidateCode { get; set; }

		[Required]
		//[RegularExpression(@"(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])[a-zA-Z0-9]{8,15}", ErrorMessage = "密码格式错误。")]
		[DataType(DataType.Password)]
		[Display(Name = "密码")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "确认密码")]
		[Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
		public string ConfirmPassword { get; set; }

		public string Code { get; set; }

	}

	public class FindPasswordViaEmailViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

	public class FindPasswordViaMobileViewModel
	{
		[Required]
		[Display(Name = "手机号码")]
		[RegularExpression(@"^[1]+[3,4,5,7,8]+\d{9}$", ErrorMessage = "非法手机号。")]
		public string Mobile { get; set; }
	}
}
