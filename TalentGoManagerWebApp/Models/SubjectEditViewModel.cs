using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentGoManagerWebApp.Models
{
    public class SubjectEditViewModel
    {
        public SubjectEditViewModel()
        {
            var initTime = DateTime.Now.AddDays(10);
            this.StartTime = new DateTime(initTime.Year, initTime.Month, initTime.Day, 9, 0, 0);
            this.EndTime = new DateTime(initTime.Year, initTime.Month, initTime.Day, 12, 0, 0);
        }

        [Display(Name = "科目")]
        [Required]
        [StringLength(50)]
        public string Subject { get; set; }

        [Display(Name = "考试开始时间")]
        [UIHint("DateTimeMinute")]
        public DateTime StartTime { get; set; }

        [Display(Name = "考试结束时间")]
        [UIHint("DateTimeMinute")]
        public DateTime EndTime { get; set; }

    }
}