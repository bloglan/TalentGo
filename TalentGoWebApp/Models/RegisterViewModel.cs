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
        [Display(Name = "身份证号码", Description = "请对照身份证如实填写，一经注册不可更改。")]
        public string IDCardNumber { get; set; }

        [Required]
        [Display(Name = "移动电话")]
        [RegularExpression(@"^[1]+[3,4,5,7,8]+\d{9}$", ErrorMessage = "非法手机号。")]
        public string Mobile { get; set; }

        [Display(Name = "验证码", Description = "查看手机短信获取验证码")]
        [Required]
        public string ValidateCode { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "电子邮件", Description = "用于接收招聘有关通知消息。")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "姓氏")]
        [StringLength(10)]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "名字", Description = "不包括姓氏部分。")]
        [StringLength(10)]
        public string GivenName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "新密码", Description = "密码长度至少8位，必须包含大写字母、小写字母和数字。")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

}