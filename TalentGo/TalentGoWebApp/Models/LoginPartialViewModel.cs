using System.Security.Principal;
using TalentGo.Identity;

namespace TalentGoWebApp.Models
{
	public class LoginPartialViewModel
    {
        public IIdentity identity { get; set; }
        public string DisplayName { get; set; }

        public TargetUser TargetUser { get; set; }
    }
}