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
		public virtual string IDCardNumber { get; protected internal set; }

        /// <summary>
        /// 姓氏
        /// </summary>
        public virtual string Surname { get; protected internal set; }

        /// <summary>
        /// 名字
        /// </summary>
        public virtual string GivenName { get; protected internal set; }

        /// <summary>
        /// 显示名称，真实姓名。
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual string DisplayName { get; protected set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual Sex Sex { get; protected internal set; }

        /// <summary>
        /// 出生日期。
        /// </summary>
        public virtual DateTime DateOfBirth { get; protected internal set; }

        /// <summary>
        /// 民族
        /// </summary>
        public virtual string Ethnicity { get; internal set; }

        /// <summary>
        /// 住址。
        /// </summary>
        public virtual string Address { get; internal set; }

        /// <summary>
        /// 签发机关。
        /// </summary>
        public virtual string Issuer { get; internal set; }

        /// <summary>
        /// 签发日期。
        /// </summary>
        public virtual DateTime? IssueDate { get; internal set; }

        /// <summary>
        /// 失效日期。
        /// </summary>
        public virtual DateTime? ExpiresAt { get; internal set; }

        /// <summary>
        /// 指示什么时候提交了实名信息。
        /// </summary>
        public virtual DateTime? WhenRealIdCommited { get; internal set; }

        /// <summary>
        /// 实名认证提交次数。
        /// </summary>
        public virtual int RealIdCommitCount { get; internal set; }

        /// <summary>
        /// 验证时间。
        /// </summary>
        public virtual DateTime? WhenRealIdValid { get; internal set; }

        /// <summary>
        /// 指示身份证是否成功验证。
        /// </summary>
        public virtual bool? RealIdValid { get; internal set; }

        /// <summary>
        /// 实名认证附加消息。
        /// </summary>
        public virtual string RealIdValidationMessage { get; internal set; }
        /// <summary>
        /// 验证人。
        /// </summary>
        public virtual string RealIdValidBy { get; internal set; }

        /// <summary>
        /// 移动电话号码。
        /// </summary>
		public virtual string Mobile { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
		public virtual DateTime WhenCreated { get; protected set; }

        /// <summary>
        /// 修改时间。
        /// </summary>
		public virtual DateTime WhenChanged { get; set; }

        /// <summary>
        /// 电子邮件地址。
        /// </summary>
		public virtual string Email { get; set; }

        /// <summary>
        /// IDCard Front File Id.
        /// </summary>
        public virtual string IDCardFrontFile { get; internal set; }

        /// <summary>
        /// IDCard Back File Id.
        /// </summary>
        public virtual string IDCardBackFile { get; internal set; }

        /// <summary>
        /// 获取与用户关联的报名表。
        /// </summary>
        public virtual ICollection<ApplicationForm> ApplicationForms { get; protected set; }
    }
}
