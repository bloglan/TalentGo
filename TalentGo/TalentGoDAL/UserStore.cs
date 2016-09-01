using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentGo.Identity;

namespace TalentGo.EntityFramework
{
	public class UserStore :
		IUserStore<ApplicationUser, int>,
		IUserPasswordStore<ApplicationUser, int>,
		IUserSecurityStampStore<ApplicationUser, int>,
		IUserEmailStore<ApplicationUser, int>,
		IQueryableUserStore<ApplicationUser, int>,
		IUserPhoneNumberStore<ApplicationUser, int>,
		IUserLockoutStore<ApplicationUser, int>,
		IUserTwoFactorStore<ApplicationUser, int>,
		IUserLoginStore<ApplicationUser, int>
	{
		TalentGoDbContext database;



		public UserStore(TalentGoDbContext DbContext)
		{
			this.database = DbContext;
		}

		public void Dispose()
		{
			//this.database.Dispose();
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public bool DisposeContext { get; set; }
		private bool _disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (this.DisposeContext && disposing && this.database != null)
			{
				this.database.Dispose();
			}
			this._disposed = true;
			this.database = null;
		}

		void ThrowIfDisposed()
		{
			if (this._disposed)
				throw new ObjectDisposedException(this.ToString());
		}

		#region IUserStore<ApplicationUser, int>

		public async Task CreateAsync(ApplicationUser user)
		{
			user.WhenCreated = DateTime.Now;
			user.WhenChanged = DateTime.Now;
			user.LoginCount = 0;
			user.Enabled = true;
			RegisterationDelegate result;

			//如果RegisterationDelegate不匹配，默认为Internet，只对创建的新用户有影响。
			if (!Enum.TryParse<RegisterationDelegate>(user.RegisterationDelegate, out result))
				user.RegisterationDelegate = RegisterationDelegate.Internet.ToString();

			//user.RegisterationDelegate = RegisterationDelegate.Internet.ToString();
			this.database.Users.Add(user);
			try
			{
				await this.database.SaveChangesAsync();
			}
			catch(Exception ex)
			{
                Console.WriteLine(ex.ToString());
			}
			
		}

		public async Task DeleteAsync(ApplicationUser user)
		{
			ApplicationUser usr = this.database.Users.Single(m => m.Id == user.Id);
			this.database.Users.Remove(usr);
			await this.database.SaveChangesAsync();
		}

		public async Task<ApplicationUser> FindByIdAsync(int userId)
		{
			return this.database.Users.SingleOrDefault(e => e.Id == userId);
		}

		public async Task<ApplicationUser> FindByNameAsync(string userName)
		{
			ApplicationUser user = this.database.Users.SingleOrDefault(m => m.UserName.ToUpper() == userName.ToUpper());
			return user;
		}



		public async Task UpdateAsync(ApplicationUser user)
		{
			ApplicationUser current = this.database.Users.Single(m => m.Id == user.Id);

			current.WhenChanged = DateTime.Now;

			var entry = this.database.Entry<ApplicationUser>(current);
			entry.CurrentValues.SetValues(user);

			await this.database.SaveChangesAsync();
		}

		#endregion

		#region IUserPasswordStore<ApplicationUser>
		public Task<string> GetPasswordHashAsync(ApplicationUser user)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			return Task.FromResult<string>(user.HashPassword);
		}

		public Task<bool> HasPasswordAsync(ApplicationUser user)
		{
			return Task.FromResult<bool>(user.HashPassword != null);
		}

		public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.HashPassword = passwordHash;
			return Task.FromResult<int>(0);
		}


		#endregion

		#region IUserSecurityStampStore<ApplicationUser, int>

		public Task SetSecurityStampAsync(ApplicationUser user, string stamp)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.SecurityStamp = stamp;
			return Task.FromResult<int>(0);
		}

		public Task<string> GetSecurityStampAsync(ApplicationUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<string>(user.SecurityStamp);
		}



		#endregion

		#region IUserEmailStore<ApplicationUser, int>

		public Task SetEmailAsync(ApplicationUser user, string email)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			user.Email = email;
			return Task.FromResult<int>(0);
		}

		public Task<string> GetEmailAsync(ApplicationUser user)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			return Task.FromResult<string>(user.Email);
		}

		public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			return Task.FromResult<bool>(user.EmailValid);
		}

		public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.EmailValid = confirmed;
			return Task.FromResult<int>(0);
		}

		public async Task<ApplicationUser> FindByEmailAsync(string email)
		{
				ApplicationUser current = this.database.Users.SingleOrDefault(e => e.Email == email);
				return current;
		}


		#endregion

		#region IQueryableUserStore<ApplicationUser, int>
		public IQueryable<ApplicationUser> Users
		{
			get
			{
				return this.database.Users.AsQueryable();
			}
		}
		#endregion

		#region IUserPhoneNumberStore<ApplicationUser, int>
		public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.Mobile = phoneNumber;
			return Task.FromResult<int>(0);
		}

		public Task<string> GetPhoneNumberAsync(ApplicationUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<string>(user.Mobile);
		}

		public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<bool>(user.MobileValid);
		}

		public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.MobileValid = confirmed;
			return Task.FromResult<int>(0);
		}

		#endregion

		#region IUserLockoutStore<ApplicationUser, int>
		public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			DateTimeOffset result = user.LockoutEndDateUTC.HasValue ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUTC.Value, DateTimeKind.Utc)) : default(DateTimeOffset);
			return Task.FromResult(result);

		}

		public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.LockoutEndDateUTC = ((lockoutEnd == DateTimeOffset.MinValue) ? null : new DateTime?(lockoutEnd.UtcDateTime));
			return Task.FromResult<int>(0);
		}

		public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.AccessFailedCount++;
			return Task.FromResult<int>(user.AccessFailedCount);
		}

		public Task ResetAccessFailedCountAsync(ApplicationUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.AccessFailedCount = 0;
			return Task.FromResult<int>(0);
		}

		public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<int>(user.AccessFailedCount);
		}

		public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<bool>(user.LockoutEnabled);
		}

		public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.LockoutEnabled = enabled;
			return Task.FromResult<int>(0);
		}

		#endregion

		#region IUserTwoFactorStore<ApplicationUser, int>

		public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.TwoFactorEnabled = enabled;
			return Task.FromResult(0);
		}

		public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<bool>(user.TwoFactorEnabled);
		}

		#endregion

		#region IUserLoginStore<ApplicationUser, int>

		public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
		{
			UserLogins newEntry = this.database.UserLogins.Create();
			newEntry.UserId = user.Id;
			newEntry.LoginProvider = login.LoginProvider;
			newEntry.ProviderKey = login.ProviderKey;
			this.database.UserLogins.Add(newEntry);
			this.database.SaveChanges();
			return Task.FromResult(0);
		}

		public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
		{
			UserLogins current = this.database.UserLogins.SingleOrDefault(e => e.UserId == user.Id && e.LoginProvider == login.LoginProvider && e.ProviderKey == login.ProviderKey);
			if (current != null)
			{
				this.database.UserLogins.Remove(current);
				this.database.SaveChanges();
			}
			return Task.FromResult(0);
		}

		public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
		{
			ApplicationUser current = this.database.Users.SingleOrDefault(e => e.Id == user.Id);

			if (current == null)
				return new List<UserLoginInfo>();
			//current
			return (from l in current.UserLogins
					where l.UserId == user.Id
					select new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList<UserLoginInfo>();
		}

		public async Task<ApplicationUser> FindAsync(UserLoginInfo login)
		{
			UserLogins entry = this.database.UserLogins.SingleOrDefault(e => e.LoginProvider == login.LoginProvider && e.ProviderKey == login.ProviderKey);
			if (entry != null)
			{
				return await this.FindByIdAsync(entry.UserId);
			}
			return default(ApplicationUser);
		}

		#endregion
	}
}
