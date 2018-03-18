using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentGoManagerWebApp.Models
{
    public class ValidateRealIdViewModel
    {
        public ValidateRealIdViewModel()
        {
            this.Next = true;
            this.ReturnBackIfRefused = true;
        }

        public Guid PersonId { get; set; }

        [Display(Name = "审核附加消息")]
        public string ValidationMessage { get; set; }

        public bool Accepted { get; set; }

        [Display(Name = "审核未通过时退回给用户")]
        public bool ReturnBackIfRefused { get; set; }

        public bool Next { get; set; }
    }
}