using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalentGoManagerWebApp.Models
{
    public class NoticeEditViewModel
    {
        public string Title { get; set; }

        public string MainContent { get; set; }

        public string CreatedBy { get; set; }

        public bool Visible { get; set; }
    }
}