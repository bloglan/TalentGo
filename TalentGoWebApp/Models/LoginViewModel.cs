using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentGoWebApp.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "用户", Prompt = "身份证号码/手机号码/电子邮件")]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码", Prompt = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }

}