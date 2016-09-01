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
		IUserStore<TargetUser, int>,
		IUserPasswordStore<TargetUser, int>,
		IUserSecurityStampStore<TargetUser, int>,
		IUserEmailStore<TargetUser, int>,
		IQueryableUserStore<TargetUser, int>,
		IUserPhoneNumberStore<TargetUser, int>,
		IUserLockoutStore<TargetUser, int>,
		IUserTwoFactorStore<TargetUser, int>,
		IUserLoginStore<TargetUser, int>
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

		#region IUserStore<TargetUser, int>

		public async Task CreateAsync(TargetUser user)
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

		public async Task DeleteAsync(TargetUser user)
		{
			TargetUser usr = this.database.Users.Single(m => m.Id == user.Id);
			this.database.Users.Remove(usr);
			await this.database.SaveChangesAsync();
		}

		public async Task<TargetUser> FindByIdAsync(int userId)
		{
			return this.database.Users.SingleOrDefault(e => e.Id == userId);
		}

		public async Task<TargetUser> FindByNameAsync(string userName)
		{
			TargetUser user = this.database.Users.SingleOrDefault(m => m.UserName.ToUpper() == userName.ToUpper());
			return user;
		}



		public async Task UpdateAsync(TargetUser user)
		{
			TargetUser current = this.database.Users.Single(m => m.Id == user.Id);

			current.WhenChanged = DateTime.Now;

			var entry = this.database.Entry<TargetUser>(current);
			entry.CurrentValues.SetValues(user);

			await this.database.SaveChangesAsync();
		}

		#endregion

		#region IUserPasswordStore<TargetUser>
		public Task<string> GetPasswordHashAsync(TargetUser user)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			return Task.FromResult<string>(user.HashPassword);
		}

		public Task<bool> HasPasswordAsync(TargetUser user)
		{
			return Task.FromResult<bool>(user.HashPassword != null);
		}

		public Task SetPasswordHashAsync(TargetUser user, string passwordHash)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.HashPassword = passwordHash;
			return Task.FromResult<int>(0);
		}


		#endregion

		#region IUserSecurityStampStore<TargetUser, int>

		public Task SetSecurityStampAsync(TargetUser user, string stamp)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.SecurityStamp = stamp;
			return Task.FromResult<int>(0);
		}

		public Task<string> GetSecurityStampAsync(TargetUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<string>(user.SecurityStamp);
		}



		#endregion

		#region IUserEmailStore<TargetUser, int>

		public Task SetEmailAsync(TargetUser user, string email)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			user.Email = email;
			return Task.FromResult<int>(0);
		}

		public Task<string> GetEmailAsync(TargetUser user)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			return Task.FromResult<string>(user.Email);
		}

		public Task<bool> GetEmailConfirmedAsync(TargetUser user)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			return Task.FromResult<bool>(user.EmailValid);
		}

		public Task SetEmailConfirmedAsync(TargetUser user, bool confirmed)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.EmailValid = confirmed;
			return Task.FromResult<int>(0);
		}

		public async Task<TargetUser> FindByEmailAsync(string email)
		{
				TargetUser current = this.database.Users.SingleOrDefault(e => e.Email == email);
				return current;
		}


		#endregion

		#region IQueryableUserStore<TargetUser, int>
		public IQueryable<TargetUser> Users
		{
			get
			{
				return this.database.Users.AsQueryable();
			}
		}
		#endregion

		#region IUserPhoneNumberStore<TargetUser, int>
		public Task SetPhoneNumberAsync(TargetUser user, string phoneNumber)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.Mobile = phoneNumber;
			return Task.FromResult<int>(0);
		}

		public Task<string> GetPhoneNumberAsync(TargetUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<string>(user.Mobile);
		}

		public Task<bool> GetPhoneNumberConfirmedAsync(TargetUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<bool>(user.MobileValid);
		}

		public Task SetPhoneNumberConfirmedAsync(TargetUser user, bool confirmed)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.MobileValid = confirmed;
			return Task.FromResult<int>(0);
		}

		#endregion

		#region IUserLockoutStore<TargetUser, int>
		public Task<DateTimeOffset> GetLockoutEndDateAsync(TargetUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			DateTimeOffset result = user.LockoutEndDateUTC.HasValue ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUTC.Value, DateTimeKind.Utc)) : default(DateTimeOffset);
			return Task.FromResult(result);

		}

		public Task SetLockoutEndDateAsync(TargetUser user, DateTimeOffset lockoutEnd)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.LockoutEndDateUTC = ((lockoutEnd == DateTimeOffset.MinValue) ? null : new DateTime?(lockoutEnd.UtcDateTime));
			return Task.FromResult<int>(0);
		}

		public Task<int> IncrementAccessFailedCountAsync(TargetUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.AccessFailedCount++;
			return Task.FromResult<int>(user.AccessFailedCount);
		}

		public Task ResetAccessFailedCountAsync(TargetUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.AccessFailedCount = 0;
			return Task.FromResult<int>(0);
		}

		public Task<int> GetAccessFailedCountAsync(TargetUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<int>(user.AccessFailedCount);
		}

		public Task<bool> GetLockoutEnabledAsync(TargetUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<bool>(user.LockoutEnabled);
		}

		public Task SetLockoutEnabledAsync(TargetUser user, bool enabled)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.LockoutEnabled = enabled;
			return Task.FromResult<int>(0);
		}

		#endregion

		#region IUserTwoFactorStore<TargetUser, int>

		public Task SetTwoFactorEnabledAsync(TargetUser user, bool enabled)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.TwoFactorEnabled = enabled;
			return Task.FromResult(0);
		}

		public Task<bool> GetTwoFactorEnabledAsync(TargetUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<bool>(user.TwoFactorEnabled);
		}

		#endregion

		#region IUserLoginStore<TargetUser, int>

		public Task AddLoginAsync(TargetUser user, UserLoginInfo login)
		{
			UserLogins newEntry = this.database.UserLogins.Create();
			newEntry.UserId = user.Id;
			newEntry.LoginProvider = login.LoginProvider;
			newEntry.ProviderKey = login.ProviderKey;
			this.database.UserLogins.Add(newEntry);
			this.database.SaveChanges();
			return Task.FromResult(0);
		}

		public Task RemoveLoginAsync(TargetUser user, UserLoginInfo login)
		{
			UserLogins current = this.database.UserLogins.SingleOrDefault(e => e.UserId == user.Id && e.LoginProvider == login.LoginProvider && e.ProviderKey == login.ProviderKey);
			if (current != null)
			{
				this.database.UserLogins.Remove(current);
				this.database.SaveChanges();
			}
			return Task.FromResult(0);
		}

		public async Task<IList<UserLoginInfo>> GetLoginsAsync(TargetUser user)
		{
			TargetUser current = this.database.Users.SingleOrDefault(e => e.Id == user.Id);

			if (current == null)
				return new List<UserLoginInfo>();
			//current
			return (from l in current.UserLogins
					where l.UserId == user.Id
					select new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList<UserLoginInfo>();
		}

		public async Task<TargetUser> FindAsync(UserLoginInfo login)
		{
			UserLogins entry = this.database.UserLogins.SingleOrDefault(e => e.LoginProvider == login.LoginProvider && e.ProviderKey == login.ProviderKey);
			if (entry != null)
			{
				return await this.FindByIdAsync(entry.UserId);
			}
			return default(TargetUser);
		}

		#endregion
	}
}
