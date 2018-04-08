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
        }

        [Required]
        [StringLength(50)]
        [Display(Name = "标题", Description = "本次考试的主题，如“xxxx年应届毕业生招聘笔试”")]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "考场地址", Description = "考场所在详细地址，如xx省xx市xx街道xx号等，该信息将标准在准考证里")]
        public string Address { get; set; }

        [Display(Name = "声明参加考试的截止日期", Description = "如果不要求考试候选人确认是否参加考试，请将此项留空。")]
        [UIHint("DateTimeMinute")]
        public DateTime? AttendanceConfirmationExpiresAt { get; set; }
    }
}