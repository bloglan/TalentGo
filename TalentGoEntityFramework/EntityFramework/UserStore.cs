using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TalentGo.Web;

namespace TalentGo.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class UserStore :
		IUserStore<WebUser, int>,
		IUserPasswordStore<WebUser, int>,
		IUserSecurityStampStore<WebUser, int>,
		IUserEmailStore<WebUser, int>,
		IQueryableUserStore<WebUser, int>,
		IUserPhoneNumberStore<WebUser, int>,
		IUserLockoutStore<WebUser, int>,
		IUserTwoFactorStore<WebUser, int>,
		IUserLoginStore<WebUser, int>
	{
		DbContext database;
        DbSet<WebUser> set;
        DbSet<UserLogin> userLoginSet;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DbContext"></param>
		public UserStore(DbContext DbContext)
		{
			this.database = DbContext;
            this.set = this.database.Set<WebUser>();
            this.userLoginSet = this.database.Set<UserLogin>();
		}

        /// <summary>
        /// 
        /// </summary>
		public void Dispose()
		{
			//this.database.Dispose();
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

        /// <summary>
        /// 
        /// </summary>
		public bool DisposeContext { get; set; }
		private bool _disposed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public async Task CreateAsync(WebUser user)
		{
			this.set.Add(user);
			try
			{
				await this.database.SaveChangesAsync();
			}
			catch(Exception ex)
			{
                Console.WriteLine(ex.ToString());
			}
			
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public async Task DeleteAsync(WebUser user)
		{
			WebUser usr = this.set.Single(m => m.Id == user.Id);
			this.set.Remove(usr);
			await this.database.SaveChangesAsync();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
		public Task<WebUser> FindByIdAsync(int userId)
		{
			return Task.FromResult(this.set.SingleOrDefault(e => e.Id == userId));
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
		public Task<WebUser> FindByNameAsync(string userName)
		{
			WebUser user = this.set.SingleOrDefault(m => m.UserName.ToUpper() == userName.ToUpper());
			return Task.FromResult(user);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public async Task UpdateAsync(WebUser user)
		{
			WebUser current = this.set.Single(m => m.Id == user.Id);

			current.WhenChanged = DateTime.Now;

			var entry = this.database.Entry<WebUser>(current);
			entry.CurrentValues.SetValues(user);

			await this.database.SaveChangesAsync();
		}

		#endregion

		#region IUserPasswordStore<TargetUser>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public Task<string> GetPasswordHashAsync(WebUser user)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			return Task.FromResult<string>(user.HashPassword);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public Task<bool> HasPasswordAsync(WebUser user)
		{
			return Task.FromResult<bool>(user.HashPassword != null);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
		public Task SetPasswordHashAsync(WebUser user, string passwordHash)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="stamp"></param>
        /// <returns></returns>
		public Task SetSecurityStampAsync(WebUser user, string stamp)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.SecurityStamp = stamp;
			return Task.FromResult<int>(0);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public Task<string> GetSecurityStampAsync(WebUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<string>(user.SecurityStamp);
		}



		#endregion

		#region IUserEmailStore<TargetUser, int>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="email"></param>
        /// <returns></returns>
		public Task SetEmailAsync(WebUser user, string email)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			user.Email = email;
			return Task.FromResult<int>(0);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public Task<string> GetEmailAsync(WebUser user)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			return Task.FromResult<string>(user.Email);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public Task<bool> GetEmailConfirmedAsync(WebUser user)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			return Task.FromResult<bool>(user.EmailValid);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
		public Task SetEmailConfirmedAsync(WebUser user, bool confirmed)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.EmailValid = confirmed;
			return Task.FromResult<int>(0);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
		public Task<WebUser> FindByEmailAsync(string email)
		{
				WebUser current = this.set.SingleOrDefault(e => e.Email == email);
				return Task.FromResult(current);
		}


		#endregion

		#region IQueryableUserStore<TargetUser, int>

        /// <summary>
        /// Get Target User collection from database.
        /// </summary>
		public IQueryable<WebUser> Users
		{
			get
			{
                return this.set;
			}
		}

        #endregion

        #region IUserPhoneNumberStore<TargetUser, int>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public Task SetPhoneNumberAsync(WebUser user, string phoneNumber)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.Mobile = phoneNumber;
			return Task.FromResult<int>(0);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public Task<string> GetPhoneNumberAsync(WebUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<string>(user.Mobile);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public Task<bool> GetPhoneNumberConfirmedAsync(WebUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<bool>(user.MobileValid);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
		public Task SetPhoneNumberConfirmedAsync(WebUser user, bool confirmed)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public Task<DateTimeOffset> GetLockoutEndDateAsync(WebUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			DateTimeOffset result = user.LockoutEndDateUTC.HasValue ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUTC.Value, DateTimeKind.Utc)) : default(DateTimeOffset);
			return Task.FromResult(result);

		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="lockoutEnd"></param>
        /// <returns></returns>
		public Task SetLockoutEndDateAsync(WebUser user, DateTimeOffset lockoutEnd)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.LockoutEndDateUTC = ((lockoutEnd == DateTimeOffset.MinValue) ? null : new DateTime?(lockoutEnd.UtcDateTime));
			return Task.FromResult<int>(0);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public Task<int> IncrementAccessFailedCountAsync(WebUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.AccessFailedCount++;
			return Task.FromResult<int>(user.AccessFailedCount);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public Task ResetAccessFailedCountAsync(WebUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.AccessFailedCount = 0;
			return Task.FromResult<int>(0);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public Task<int> GetAccessFailedCountAsync(WebUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<int>(user.AccessFailedCount);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public Task<bool> GetLockoutEnabledAsync(WebUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<bool>(user.LockoutEnabled);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
		public Task SetLockoutEnabledAsync(WebUser user, bool enabled)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
		public Task SetTwoFactorEnabledAsync(WebUser user, bool enabled)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.TwoFactorEnabled = enabled;
			return Task.FromResult(0);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public Task<bool> GetTwoFactorEnabledAsync(WebUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<bool>(user.TwoFactorEnabled);
		}

		#endregion

		#region IUserLoginStore<TargetUser, int>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="login"></param>
        /// <returns></returns>
		public Task AddLoginAsync(WebUser user, UserLoginInfo login)
		{
			UserLogin newEntry = this.userLoginSet.Create();
			newEntry.UserId = user.Id;
			newEntry.LoginProvider = login.LoginProvider;
			newEntry.ProviderKey = login.ProviderKey;
			this.userLoginSet.Add(newEntry);
			this.database.SaveChanges();
			return Task.FromResult(0);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="login"></param>
        /// <returns></returns>
		public Task RemoveLoginAsync(WebUser user, UserLoginInfo login)
		{
			UserLogin current = this.userLoginSet.SingleOrDefault(e => e.UserId == user.Id && e.LoginProvider == login.LoginProvider && e.ProviderKey == login.ProviderKey);
			if (current != null)
			{
				this.userLoginSet.Remove(current);
				this.database.SaveChanges();
			}
			return Task.FromResult(0);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
		public async Task<IList<UserLoginInfo>> GetLoginsAsync(WebUser user)
		{
			WebUser current = this.set.SingleOrDefault(e => e.Id == user.Id);

			if (current == null)
				return new List<UserLoginInfo>();
			//current
			return (from l in current.UserLogins
					where l.UserId == user.Id
					select new UserLoginInfo(l.LoginProvider, l.ProviderKey)).ToList<UserLoginInfo>();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
		public async Task<WebUser> FindAsync(UserLoginInfo login)
		{
			UserLogin entry = this.userLoginSet.SingleOrDefault(e => e.LoginProvider == login.LoginProvider && e.ProviderKey == login.ProviderKey);
			if (entry != null)
			{
				return await this.FindByIdAsync(entry.UserId);
			}
			return default(WebUser);
		}

		#endregion
	}
}
