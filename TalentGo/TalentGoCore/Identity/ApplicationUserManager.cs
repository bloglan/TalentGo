using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentGo.EntityFramework;

namespace TalentGo.Identity
{
	public class ApplicationUserManager : UserManager<TargetUser, int>
	{
		public ApplicationUserManager(IUserStore<TargetUser, int> store):base(store)
		{

		}

		public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
		{
			var manager = new ApplicationUserManager(new TalentGo.EntityFramework.UserStore(context.Get<TalentGoDbContext>()));
			// 配置用户名的验证逻辑
			manager.UserValidator = new UserValidator<TargetUser, int>(manager)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};

			// 配置密码的验证逻辑
			manager.PasswordValidator = new PasswordValidator
			{
				RequiredLength = 8,
				RequireNonLetterOrDigit = false,
				RequireDigit = true,
				RequireLowercase = true,
				RequireUppercase = true,
			};

			// 配置用户锁定默认值
			manager.UserLockoutEnabledByDefault = true;
			manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
			manager.MaxFailedAccessAttemptsBeforeLockout = 5;

			// 注册双重身份验证提供程序。此应用程序使用手机和电子邮件作为接收用于验证用户的代码的一个步骤
			// 你可以编写自己的提供程序并将其插入到此处。
			manager.RegisterTwoFactorProvider("电话代码", new PhoneNumberTokenProvider<TargetUser, int>
			{
				MessageFormat = "你的安全代码是 {0}"
			});
			manager.RegisterTwoFactorProvider("电子邮件代码", new EmailTokenProvider<TargetUser, int>
			{
				Subject = "安全代码",
				BodyFormat = "你的安全代码是 {0}"
			});
			manager.EmailService = new EmailService();
			manager.SmsService = new SmsService();
			var dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null)
			{
				manager.UserTokenProvider =
					new DataProtectorTokenProvider<TargetUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
			}
			return manager;
		}
	}
}
