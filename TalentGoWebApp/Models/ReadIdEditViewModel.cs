using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TalentGo;

namespace TalentGoWebApp.Models
{
    public class ReadIdEditViewModel
    {
        public ReadIdEditViewModel()
        {
            this.IssueDate = DateTime.Now;
        }

        [Display(Name = "民族", Description = "不用填写\"族\"字")]
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

        [Required]
        [Display(Name = "有效期起")]
        [DataType(DataType.Date)]
        public DateTime? IssueDate { get; set; }

        [Display(Name = "有效期至", Description = "长期有效的，此项留空。")]
        [DataType(DataType.Date)]
        public DateTime? ExpiresAt { get; set; }
    }
}