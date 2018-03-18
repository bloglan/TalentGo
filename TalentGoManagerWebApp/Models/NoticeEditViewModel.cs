using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TalentGoManagerWebApp.Models
{
    public class NoticeEditViewModel
    {
        [Display(Name = "标题")]
        [Required]
        [StringLength(30)]
        public string Title { get; set; }

        [Display(Name = "正文")]
        [Required]
        [AllowHtml]
        [UIHint("CKEditor")]
        public string MainContent { get; set; }

        [Display(Name = "创建人")]
        [Required]
        public string CreatedBy { get; set; }
    }
}