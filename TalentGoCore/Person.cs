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
    [Table("Users")]
	public abstract class Person
	{
        /// <summary>
        /// 
        /// </summary>
		protected Person()
		{
            this.WhenCreated = DateTime.Now;
            this.WhenChanged = DateTime.Now;
		}

        /// <summary>
        /// 用户ID
        /// </summary>
        [Key]
		public int Id { get; protected set; }


        /// <summary>
        /// 身份证号码。
        /// </summary>
        [Required]
		[StringLength(25)]
		public string IDCardNumber { get; set; }

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
	}
}
