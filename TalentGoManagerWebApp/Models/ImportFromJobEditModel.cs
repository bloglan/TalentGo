using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentGoManagerWebApp.Models
{
    public class ImportFromJobEditModel
    {
        [Display(Name = "职位/类别")]
        public int SelectedJobId { get; set; }
    }
}