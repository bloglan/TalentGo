using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentGoManagerWebApp.Models
{
    public class ImportFromRecruitmentPlanEditModel
    {
        [Display(Name = "招聘计划")]
        public int SelectedRecruitmentPlanId { get; set; }
    }
}