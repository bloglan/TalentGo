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

        [Display(Name = "描述", Description = "对该职位的描述，可不填写。")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "计划招聘人数", Description = "该职位/类别计划招聘人数。可不填写。")]
        public int? ExpectRecruitCount { get; set; }

        [Display(Name = "工作地点", Description = "该职位预期的工作地点，若在聘用时分配，请填聘用时分配。")]
        [Required]
        public string WorkLocation { get; set; }

        [Display(Name = "学历要求", Description = "每个选项作一行，供求职者选择。")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string EducationBakcgroundRequirement { get; set; }

        [Display(Name = "学位要求", Description = "每个选项作一行，供求职者选择。")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string DegreeRequirement { get; set; }

        [Display(Name = "专业要求", Description = "每个选项作一行，供求职者选择。")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string MajorRequirement { get; set; }
    }
}