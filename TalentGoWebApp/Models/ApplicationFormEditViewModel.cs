using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TalentGo;

namespace TalentGoWebApp.Models
{
    public class ApplicationFormEditViewModel
    {
        public Job Job { get; set; }

        /// <summary>
        /// Place of birth.
        /// </summary>
		[Display(Name = "籍贯", Prompt = "xx省xx市(县、区）")]
        [Required]
        [StringLength(50)]
        public string NativePlace { get; set; }

        /// <summary>
        /// Current home address.
        /// </summary>
		[Display(Name = "现居地", Description = "您目前实际居住的地址", Prompt = "xx省xx市xx县区xx街道")]
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
		[Display(Name = "主修专业", Description = "请根据学历证明文件所标注的专业如实准确填写。")]
        [Required]
        [StringLength(50)]
        public string Major { get; set; }

        /// <summary>
        /// Major of education.
        /// </summary>
		[Display(Name = "报名专业", Description = "如果列表中没有所学专业，请确认是否符合报名要求。要继续报名，请选择一个你认为最符合的报名专业。")]
        [Required]
        [StringLength(50)]
        public string SelectedMajor { get; set; }

        /// <summary>
        /// Year of graduated.
        /// </summary>
		[Display(Name = "毕业年度", Description = "预计取得毕业证书所在的年度")]
        public int YearOfGraduated { get; set; }

        /// <summary>
        /// Educational background.
        /// </summary>
		[Display(Name = "学历")]
        [Required]
        [StringLength(15)]
        public string EducationalBackground { get; set; }

        /// <summary>
        /// Educational background.
        /// </summary>
		[Display(Name = "学历证书编号", Description = "请填写毕业证、就业推荐表或其他证明文件上的编号。")]
        [StringLength(50)]
        public string AcademicCertNumber { get; set; }

        /// <summary>
        /// Educational Degree.
        /// </summary>
		[Display(Name = "学位")]
        [Required]
        [StringLength(15)]
        public string Degree { get; set; }

        /// <summary>
        /// Educational background.
        /// </summary>
		[Display(Name = "学位证书编号", Description = "尚未取得学位证书的，可不填此项。")]
        [StringLength(50)]
        public string DegreeCertNumber { get; set; }

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