using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TalentGo.Identity
{
    public class ApplicationSignInManager : SignInManager<TargetUser, int>
	{
		public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
			: base(userManager, authenticationManager)
		{
		}

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(TargetUser user)
		{
			return user.GenerateUserIdentityAsync((ApplicationUserManager)this.UserManager);
		}

		
	}
}
