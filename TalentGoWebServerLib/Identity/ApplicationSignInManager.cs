using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using TalentGo.Recruitment;
using TalentGo.Web;

namespace TalentGo.Identity
{
    public class ApplicationSignInManager : SignInManager<WebUser, int>
	{
		public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
			: base(userManager, authenticationManager)
		{
		}

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(WebUser user)
		{
			return user.GenerateUserIdentityAsync((ApplicationUserManager)this.UserManager);
		}

		
	}
}
