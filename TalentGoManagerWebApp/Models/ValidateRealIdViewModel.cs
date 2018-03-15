using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalentGoManagerWebApp.Models
{
    public class ValidateRealIdViewModel
    {
        public ValidateRealIdViewModel()
        {
            this.Next = true;
        }

        public Guid PersonId { get; set; }

        public bool Accepted { get; set; }

        public bool Next { get; set; }
    }
}