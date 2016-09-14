using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using TalentGo.Core;
using TalentGo.Utilities;

namespace TalentGo.Identity
{
    /// <summary>
    /// 一个目标用户管理器。
    /// </summary>
	public class TargetUserManager
	{
        IQueryableUserStore<TargetUser, int> store;
		//ApplicationUserManager userManager;
		public TargetUserManager(IQueryableUserStore<TargetUser, int> Store)
		{
            this.store = Store;
		}

        public IQueryable<TargetUser> TargetUsers
        {
            get { return this.store.Users; }
        }

        public async Task<TargetUser> FindByIdAsync(int UserId)
        {
            return await this.store.FindByIdAsync(UserId);
        }

		public async Task<IEnumerable<TargetUser>> GetAvaiableTargetUsers(RecruitmentContextBase context)
		{
			var identity = context.LoginUser.Identity as WindowsIdentity;
			if (identity == null)
			{
				//不是WindowsIdentity

				var claimsidentity = context.LoginUser.Identity as ClaimsIdentity;
				if (claimsidentity == null)
					throw new InvalidOperationException("操作无效，不支持的标识类型");

				//是ClaimsIdentity
				int userid = int.Parse(claimsidentity.FindFirst(ClaimDefinition.UserIdClaimType).Value);
                TargetUser usr = await this.store.FindByIdAsync(userid);

				List<TargetUser> userList = new List<TargetUser>();
				userList.Add(usr);

				return userList.AsEnumerable();
			}

			//对于WindowsIdentity，返回代理信息所示SID与其关联的用户。
			string SIDStr = identity.User.ToString();
            return this.store.Users.Where(t => t.RegisterationDelegate == RegisterationDelegate.Intranet.ToString() && t.DelegateInfo == SIDStr);
		}


		/// <summary>
		/// 创建一个目标用户。
		/// </summary>
		/// <param name="user"></param>
		/// <returns></returns>
		public async Task CreateTargetUser(TargetUser user, RecruitmentContextBase context)
		{
			var identity = context.LoginUser.Identity as WindowsIdentity;
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

            await this.store.CreateAsync(user);
		}
	}
}
