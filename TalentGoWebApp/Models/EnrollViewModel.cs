using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TalentGo;

namespace TalentGoWebApp.Models
{
    public class EnrollViewModel
    {
        public Job Job { get; set; }

        /// <summary>
        /// Place of birth.
        /// </summary>
		[Display(Name = "籍贯")]
        [Required]
        [StringLength(50)]
        public string NativePlace { get; set; }

        /// <summary>
        /// Current home address.
        /// </summary>
		[Display(Name = "家庭现居地")]
        [Required]
        [StringLength(100)]
        public string Source { get; set; }

        /// <summary>
        /// Political of user.
        /// </summary>
		[Display(Name = "政治面貌")]
        [Required]
        [StringLength(15)]
        public string PoliticalStatus { get; set; }

        /// <summary>
        /// Health status of user.
        /// </summary>
		[Display(Name = "健康状况")]
        [Required]
        [StringLength(10)]
        public string Health { get; set; }

        /// <summary>
        /// Marital status of user.
        /// </summary>
		[Display(Name = "婚姻状况")]
        [Required]
        [StringLength(5)]
        public string Marriage { get; set; }

        /// <summary>
        /// Last graduated school.
        /// </summary>
		[Display(Name = "毕业院校")]
        [Required]
        [StringLength(50)]
        public string School { get; set; }

        /// <summary>
        /// Major of education.
        /// </summary>
		[Display(Name = "毕业专业")]
        [Required]
        [StringLength(50)]
        public string Major { get; set; }

        /// <summary>
        /// Year of graduated.
        /// </summary>
		[Display(Name = "毕业年度")]
        public int YearOfGraduated { get; set; }

        /// <summary>
        /// Educational background.
        /// </summary>
		[Display(Name = "学历")]
        [Required]
        [StringLength(15)]
        public string EducationBackground { get; set; }

        /// <summary>
        /// Educational Degree.
        /// </summary>
		[Display(Name = "学位")]
        [Required]
        [StringLength(15)]
        public string Degree { get; set; }

        /// <summary>
        /// Resume.
        /// </summary>
		[Display(Name = "简历")]
        [Required]
        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string Resume { get; set; }

        /// <summary>
        /// Accomplishments.
        /// </summary>
		[Display(Name = "特长及自我介绍")]
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Accomplishments { get; set; }
    }
}