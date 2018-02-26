using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using TalentGo.Web;

namespace TalentGo.Identity
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationSignInManager : SignInManager<WebUser, int>
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="authenticationManager"></param>
		public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
			: base(userManager, authenticationManager)
		{
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(WebUser user)
		{
			return user.GenerateUserIdentityAsync((ApplicationUserManager)this.UserManager);
		}

		
	}
}
