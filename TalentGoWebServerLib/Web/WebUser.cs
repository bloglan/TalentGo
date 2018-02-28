using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Web
{
    /// <summary>
    /// 表示一个网站用户。
    /// </summary>
    public class WebUser : Person, IUser<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        public WebUser()
            : base()
        {
            UserLogins = new HashSet<UserLogin>();
        }

        /// <summary>
        /// 用户名/登录名，实现IUser接口
        /// </summary>
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码哈希。
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [StringLength(256)]
        public string HashPassword { get; set; }

        /// <summary>
        /// 登陆次数。
        /// </summary>
        public int LoginCount { get; set; }

        /// <summary>
        /// 已启用？
        /// </summary>
		public bool Enabled { get; set; }

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
        /// 提供一个创建ClaimsIdentity的可等待方法，由SignManager调用。
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<WebUser, Guid> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }

        /// <summary>
        /// 目标用户所持有的所有关联登陆（目前未使用）
        /// </summary>
        public virtual ICollection<UserLogin> UserLogins { get; set; }

    }
}
