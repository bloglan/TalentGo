using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TalentGo;

namespace TalentGoWebApp.Models
{
    public class EditPersonInfoModel
    {
        [Display(Name = "民族")]
        [Required]
        [StringLength(50)]
        public string Ethnicity { get; set; }

        [Display(Name = "住址")]
        [Required]
        [StringLength(150)]
        public string Address { get; set; }

        [Display(Name = "签发机关")]
        [Required]
        [StringLength(50)]
        public string Issuer { get; set; }

        [Display(Name = "有效期起")]
        public DateTime IssueDate { get; set; }

        [Display(Name = "有效期至", Description = "长期有效的，此项留空。")]
        public DateTime? ExpiresAt { get; set; }
    }
}