using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentGoManagerWebApp.Models
{
    public class ReviewFileModel
    {
        public ReviewFileModel()
        {
            this.Next = true;
        }

        public int FormId { get; set; }

        public bool Accepted { get; set; }

        [Display(Name = "附加消息")]
        public string FileReviewMessage { get; set; }

        public bool Next { get; set; }
    }
}