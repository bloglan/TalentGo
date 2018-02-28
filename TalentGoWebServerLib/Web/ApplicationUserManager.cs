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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public Task<WebUser> FindByMobileAsync(string Mobile)
        {
            var store = this.Store as IQueryableUserStore<WebUser, Guid>;
            if (store == null)
                throw new NotSupportedException();

            return Task.FromResult(store.Users.FirstOrDefault(u => u.Mobile == Mobile));
        }


    }
}
