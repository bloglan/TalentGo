using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 表示一个用户
    /// </summary>
	public abstract class Person
	{
        /// <summary>
        /// 
        /// </summary>
		protected Person()
		{
            this.WhenCreated = DateTime.Now;
            this.WhenChanged = DateTime.Now;
            this.ApplicationForms = new HashSet<ApplicationForm>();
		}

        /// <summary>
        /// 用户ID
        /// </summary>
        [Key]
		public Guid Id { get; protected set; }


        /// <summary>
        /// 身份证号码。
        /// </summary>
        [Required]
		[StringLength(25)]
		public string IDCardNumber { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 出生日期。
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// 民族
        /// </summary>
        public string Ethnicity { get; set; }

        /// <summary>
        /// 住址。
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 签发机关。
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 签发日期。
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// 失效日期。
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// 出生地。
        /// </summary>
        public string NativePlace { get; set; }

        /// <summary>
        /// 移动电话号码。
        /// </summary>
		[Required]
		[StringLength(15)]
		public string Mobile { get; set; }


        /// <summary>
        /// 创建时间。
        /// </summary>
		public DateTime WhenCreated { get; protected set; }

        /// <summary>
        /// 修改时间。
        /// </summary>
		public DateTime WhenChanged { get; set; }


        /// <summary>
        /// 显示名称，真实姓名。
        /// </summary>
		[Required]
		[StringLength(10)]
		public string DisplayName { get; set; }


        /// <summary>
        /// 电子邮件地址。
        /// </summary>
		[StringLength(150)]
		public string Email { get; set; }

        /// <summary>
        /// IDCard Front File Id.
        /// </summary>
        public Guid? IDCardFrontFileId { get; internal set; }

        /// <summary>
        /// IDCard Front File.
        /// </summary>
        public virtual File IDCardFrontFile { get; internal set; }

        /// <summary>
        /// IDCard Back File Id.
        /// </summary>
        public Guid? IDCardBackFileId { get; internal set; }

        /// <summary>
        /// IDCard Back File
        /// </summary>
        public virtual File IDCardBackFile { get; internal set; }

        /// <summary>
        /// 证件照片Id。
        /// </summary>
        public Guid? HeadImageId { get; set; }

        /// <summary>
        /// HeadImage
        /// </summary>
        public virtual File HeadImage { get; set; }

        /// <summary>
        /// 获取与用户关联的报名表。
        /// </summary>
        public virtual ICollection<ApplicationForm> ApplicationForms { get; protected set; }
	}
}
