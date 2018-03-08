using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentGoManagerWebApp.Models
{
    public class JobEditViewModel
    {
        [Display(Name = "名称", Description = "职位/职位方向/类别的名称")]
        public string Name { get; set; }

        [Display(Name = "Description", Description = "Detail of this job.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Work Location")]
        [Required]
        public string WorkLocation { get; set; }

        [Display(Name = "Education background requirement", Description = "每个选项作一行，供求职者选择。")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string EducationBakcgroundRequirement { get; set; }

        [Display(Name = "Education background requirement", Description = "每个选项作一行，供求职者选择。")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string DegreeRequirement { get; set; }

        [Display(Name = "Education background requirement", Description = "每个选项作一行，供求职者选择。")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string MajorRequirement { get; set; }
    }
}