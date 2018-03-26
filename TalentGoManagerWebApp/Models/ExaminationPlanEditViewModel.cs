using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentGoManagerWebApp.Models
{
    public class ExaminationPlanEditViewModel
    {
        public ExaminationPlanEditViewModel()
        {
            this.AttendanceConfirmationExpiresAt = DateTime.Now.AddDays(7);
        }

        [Required]
        [StringLength(50)]
        [Display(Name = "标题")]
        public string Title { get; set; }

        [Display(Name = "声明参加考试的截止日期")]
        [UIHint("DateTimeMinute")]
        public DateTime AttendanceConfirmationExpiresAt { get; set; }
    }
}