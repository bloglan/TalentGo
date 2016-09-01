using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TalentGo.Recruitment;

namespace TalentGo.Identity
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
			EnrollmentData = new HashSet<EnrollmentData>();
			UserLogins = new HashSet<UserLogins>();
		}

		public int Id { get; set; }

		[Required]
		[StringLength(25)]
		public string IDCardNumber { get; set; }

		[Required]
		[StringLength(15)]
		public string Mobile { get; set; }

		[Required(AllowEmptyStrings = true)]
		[StringLength(256)]
		public string HashPassword { get; set; }

		public DateTime WhenCreated { get; set; }

		public DateTime WhenChanged { get; set; }

		public DateTime? LastLogin { get; set; }

		public int LoginCount { get; set; }

		public bool Enabled { get; set; }

		[Required]
		[StringLength(10)]
		public string DisplayName { get; set; }

		[Required]
		[StringLength(50)]
		public string RegisterationDelegate { get; set; }

		[StringLength(50)]
		public string DelegateInfo { get; set; }

		public bool MobileValid { get; set; }

		public bool EmailValid { get; set; }

		[Required]
		[StringLength(50)]
		public string UserName { get; set; }

		[StringLength(50)]
		public string SecurityStamp { get; set; }

		[StringLength(150)]
		public string Email { get; set; }

		public DateTime? LockoutEndDateUTC { get; set; }

		public bool LockoutEnabled { get; set; }

		public int AccessFailedCount { get; set; }

		public bool TwoFactorEnabled { get; set; }

		public bool? Renamed { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<EnrollmentData> EnrollmentData { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<UserLogins> UserLogins { get; set; }


		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<TargetUser, int> manager)
		{
			// 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// 在此处添加自定义用户声明
			// 添加了两项声明，RegistrationDelegate和DelegateInfo
			userIdentity.AddClaim(new Claim(ClaimDefinition.RegisterationDelegateType, this.RegisterationDelegate, ClaimDefinition.TypeofString));
			if (!string.IsNullOrEmpty(this.DelegateInfo))
			{
				userIdentity.AddClaim(new Claim(ClaimDefinition.DelegateInfo, this.DelegateInfo, ClaimDefinition.TypeofString));
			}

			// 此处添加一项声明，使外部登陆用户总是具备InternetUser角色。
			userIdentity.AddClaim(new Claim(ClaimDefinition.RoleClaimType, "InternetUser", ClaimDefinition.TypeofString));
			return userIdentity;
		}

	}
}
