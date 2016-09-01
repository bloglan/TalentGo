using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TalentGo.EntityFramework;
using TalentGo.Utilities;

namespace TalentGo.Identity
{
	public class TargetUserManager
	{
		HttpContextBase context;
		TalentGoDbContext database;
		//ApplicationUserManager userManager;
		public TargetUserManager(HttpContextBase context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			this.context = context;
			this.database = TalentGoDbContext.FromContext(this.context);
			
		}

		public async Task<IEnumerable<TargetUser>> GetAvaiableTargetUsers()
		{
			var identity = this.context.User.Identity as WindowsIdentity;
			if (identity == null)
			{
				//不是WindowsIdentity

				var claimsidentity = this.context.User.Identity as ClaimsIdentity;
				if (claimsidentity == null)
					throw new InvalidOperationException("操作无效，不支持的标识类型");

				//是ClaimsIdentity
				int userid = int.Parse(claimsidentity.FindFirst(ClaimDefinition.UserIdClaimType).Value);
				TargetUser usr = this.database.Users.Single(e => e.Id == userid);

				List<TargetUser> userList = new List<TargetUser>();
				userList.Add(usr);

				return userList.AsEnumerable();
			}

			//对于WindowsIdentity，返回代理信息所示SID与其关联的用户。
			string SIDStr = identity.User.ToString();
			var userSet = from user in this.database.Users.AsNoTracking()
						  where user.RegisterationDelegate == RegisterationDelegate.Intranet.ToString() && user.DelegateInfo == SIDStr
						  select user;
			return userSet;
		}

		/// <summary>
		/// 绑定一个目标用户。
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		[Obsolete("此方法已过时，应采用SetTargetUserByID方法")]
		public Task AssignTargetUser(TargetUser user)
		{
			//如果是一个ClaimsIdentity，操作无效
			//如果是WindowsIdentity，则获取用户的SID
			//然后检查指定的RegisterationDelegate是否为IntranetWeb，并且DelegateInfo保存的SID与此WindowsIdentity相同。
			if (this.context.User.Identity is ClaimsIdentity)
				throw new InvalidOperationException("采用ClaimsIdentity时操作无效。");

			WindowsIdentity winid = this.context.User.Identity as WindowsIdentity;
			if (winid == null)
				throw new InvalidOperationException("不支持的Identity类型。");

			string winSID = winid.User.ToString();

			var UserSet = from dbuser in this.database.Users
						  where dbuser.RegisterationDelegate == RegisterationDelegate.Intranet.ToString() && dbuser.DelegateInfo == winSID && dbuser.Id == user.Id
						  select dbuser;

			if (!UserSet.Any())
			{
				throw new ArgumentException("给定的ApplicationUser不满足条件。");
			}

			TargetUser target = UserSet.First();
			this.context.Session[TargetUserKey] = user;
			return Task.FromResult(0);
		}

		/// <summary>
		/// 获取目标用户。
		/// </summary>
		public TargetUser TargetUser
		{
			get
			{
				TargetUser user;
				if (this.TryGetTargetUser(out user))
					return user;

				throw new ArgumentException("尚未绑定用户");
			}
		}

		/// <summary>
		/// 获取一个值，指示是否已经有绑定的目标用户
		/// </summary>
		public bool IsAssignedTargetUser
		{
			get
			{
				TargetUser user;
				return this.TryGetTargetUser(out user);
			}
		}

		/// <summary>
		/// 根据指示的UserID设置绑定用户。
		/// </summary>
		/// <param name="UserID"></param>
		public async Task SetTargetUserByID(int UserID)
		{
			var targetUsers = await this.GetAvaiableTargetUsers();
			var current = targetUsers.SingleOrDefault(e => e.Id == UserID);
			if (current == null)
				throw new ArgumentException("找不到用户。");

			this.context.Session[TargetUserKey] = current;
		}

		/// <summary>
		/// 创建一个目标用户。
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public async Task CreateTargetUser(TargetUser user)
		{
			var identity = this.context.User.Identity as WindowsIdentity;
			if (identity == null)
				throw new InvalidOperationException("操作无效，只有WindowsIdentity可以创建目标用户。");

			//执行验证和完善
			ChineseIDCardNumber number = ChineseIDCardNumber.CreateNumber(user.IDCardNumber);

			user.HashPassword = "";
			user.WhenCreated = DateTime.Now;
			user.WhenChanged = DateTime.Now;
			user.Enabled = true;
			user.RegisterationDelegate = RegisterationDelegate.Intranet.ToString();
			user.DelegateInfo = identity.User.ToString();
			user.MobileValid = true;
			user.EmailValid = true;
			user.UserName = number.IDCardNumber;
			user.LockoutEnabled = true;
			user.TwoFactorEnabled = false;

			this.database.Users.Add(user);
			await this.database.SaveChangesAsync();
		}

		/// <summary>
		/// 尝试获得目标用户ID，如果是ClaimsIdentity，则总是返回其自身声明的UserID，同时IsAssigned返回true表示匹配成功。
		/// 如果是WindowsIdentity，当只有一个ApplicationUser与之绑定时，则返回对应的UserID，同时IsAssigned返回true表示匹配成功。
		///						若有多个ApplicationUser与之对应时，若没有缓存绑定，则返回无意义，IsAssigned返回false表示未绑定。
		/// </summary>
		/// <param name="IsAssigned"></param>
		/// <returns></returns>
		bool TryGetTargetUser(out TargetUser User)
		{
			//尝试从Session缓存返回User
			if (this.context.Session[TargetUserKey] != null)
			{
				User = (TargetUser)this.context.Session[TargetUserKey];
				return true;
			}


			var avaiableUserSet = this.GetAvaiableTargetUsers().GetAwaiter().GetResult();
			if (avaiableUserSet.Count() == 1)
			{
				TargetUser user = avaiableUserSet.First();
				this.context.Session[TargetUserKey] = user;
				User = user;
				return true;
			}


			User = null;
			return false;
		}

		static readonly string TargetUserKey = "TargetUser";
	}
}
