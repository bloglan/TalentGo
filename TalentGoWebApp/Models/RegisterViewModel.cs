using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentGoWebApp.Models
{
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
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

}