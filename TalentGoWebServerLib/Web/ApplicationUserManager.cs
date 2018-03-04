using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationUserManager : UserManager<WebUser, Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
		public ApplicationUserManager(IUserStore<WebUser, Guid> store)
            : base(store)
        {

        }
    }
}
