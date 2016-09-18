using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using TalentGo.Identity;

namespace TalentGo.Recruitment
{
    /// <summary>
    /// 表示一个目标用户
    /// </summary>
    [Table("Users")]
	public class TargetUser : IUser<int>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public TargetUser()
		{
			EnrollmentData = new HashSet<Enrollment>();
			UserLogins = new HashSet<UserLogin>();
		}

        /// <summary>
        /// 用户ID，实现IUser接口
        /// </summary>
        [Key]
		public int Id { get; protected set; }

        /// <summary>
        /// 用户名/登录名，实现IUser接口
        /// </summary>
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

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
        /// 密码哈希。
        /// </summary>
		[Required(AllowEmptyStrings = true)]
		[StringLength(256)]
		public string HashPassword { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
		public DateTime WhenCreated { get; set; }

        /// <summary>
        /// 修改时间。
        /// </summary>
		public DateTime WhenChanged { get; set; }

        /// <summary>
        /// 最后登录时间。
        /// </summary>
		public DateTime? LastLogin { get; set; }

        /// <summary>
        /// 登陆次数。
        /// </summary>
		public int LoginCount { get; set; }

        /// <summary>
        /// 已启用？
        /// </summary>
		public bool Enabled { get; set; }

        /// <summary>
        /// 显示名称，真实姓名。
        /// </summary>
		[Required]
		[StringLength(10)]
		public string DisplayName { get; set; }

        /// <summary>
        /// 注册代理，
        /// </summary>
		[Required]
		[StringLength(50)]
		public string RegisterationDelegate { get; set; }

        /// <summary>
        /// 代理信息，即关联的代报名人。
        /// </summary>
		[StringLength(50)]
		public string DelegateInfo { get; set; }

        /// <summary>
        /// 获取一个值，确定移动电话已验证。
        /// </summary>
		public bool MobileValid { get; set; }

        /// <summary>
        /// 获取一个值，指示邮件已验证。
        /// </summary>
		public bool EmailValid { get; set; }

		
        /// <summary>
        /// 安全戳。
        /// </summary>
		[StringLength(50)]
		public string SecurityStamp { get; set; }

        /// <summary>
        /// 电子邮件地址。
        /// </summary>
		[StringLength(150)]
		public string Email { get; set; }

        /// <summary>
        /// 锁定时间（协调世界时）
        /// </summary>
		public DateTime? LockoutEndDateUTC { get; set; }

        /// <summary>
        /// 是否启用账户锁定策略。
        /// </summary>
		public bool LockoutEnabled { get; set; }

        /// <summary>
        /// 账户访问失败次数。
        /// </summary>
		public int AccessFailedCount { get; set; }

        /// <summary>
        /// 第二因子身份验证（该值目前忽略）
        /// </summary>
		public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// 指示一个值，表示用户是否可以修改其显示名称。
        /// </summary>
		public bool? Renamed { get; set; }

        /// <summary>
        /// 目标用户所持有的报名表。
        /// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Enrollment> EnrollmentData { get; set; }

        /// <summary>
        /// 目标用户所持有的所有关联登陆（目前未使用）
        /// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<UserLogin> UserLogins { get; set; }

        /// <summary>
        /// 提供一个创建ClaimsIdentity的可等待方法，由SignManager调用。
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<TargetUser, int> manager)
		{
			// 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// 在此处添加自定义用户声明
			// 添加了两项声明，RegistrationDelegate和DelegateInfo
			userIdentity.AddClaim(new Claim(ClaimDefinition.RegisterationDelegateType, this.RegisterationDelegate));
			if (!string.IsNullOrEmpty(this.DelegateInfo))
			{
				userIdentity.AddClaim(new Claim(ClaimDefinition.DelegateInfo, this.DelegateInfo));
			}

			// 此处添加一项声明，使外部登陆用户总是具备InternetUser的角色声明。
			userIdentity.AddClaim(new Claim(userIdentity.RoleClaimType, "InternetUser"));
			return userIdentity;
		}

	}
}
