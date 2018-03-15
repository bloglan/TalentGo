using System;
using System.Collections.Generic;
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

        public bool Next { get; set; }
    }
}