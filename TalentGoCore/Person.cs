﻿using System;
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
            this.IssueDate = new DateTime(1900, 1, 1);
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
		public string IDCardNumber { get; set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// 显示名称，真实姓名。
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string DisplayName { get; protected set; }

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
        public string Ethnicity { get; internal set; }

        /// <summary>
        /// 住址。
        /// </summary>
        public string Address { get; internal set; }

        /// <summary>
        /// 签发机关。
        /// </summary>
        public string Issuer { get; internal set; }

        /// <summary>
        /// 签发日期。
        /// </summary>
        public DateTime? IssueDate { get; internal set; }

        /// <summary>
        /// 失效日期。
        /// </summary>
        public DateTime? ExpiresAt { get; internal set; }

        /// <summary>
        /// 指示什么时候提交了实名信息。
        /// </summary>
        public DateTime? WhenRealIdCommited { get; internal set; }

        /// <summary>
        /// 验证时间。
        /// </summary>
        public DateTime? WhenRealIdValid { get; internal set; }

        /// <summary>
        /// 指示身份证是否成功验证。
        /// </summary>
        public bool? RealIdValid { get; internal set; }

        /// <summary>
        /// 验证人。
        /// </summary>
        public string RealIdValidBy { get; internal set; }

        /// <summary>
        /// 移动电话号码。
        /// </summary>
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
        /// 电子邮件地址。
        /// </summary>
		public string Email { get; set; }

        /// <summary>
        /// IDCard Front File Id.
        /// </summary>
        public string IDCardFrontFile { get; internal set; }

        /// <summary>
        /// IDCard Back File Id.
        /// </summary>
        public string IDCardBackFile { get; internal set; }

        /// <summary>
        /// 获取与用户关联的报名表。
        /// </summary>
        public virtual ICollection<ApplicationForm> ApplicationForms { get; protected set; }
    }
}
