using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    public class ApplicationUserManager : UserManager<TargetUser, int>
	{
		public ApplicationUserManager(IUserStore<TargetUser, int> store):base(store)
		{

		}

        public async Task<TargetUser> FindByMobileAsync(string Mobile)
        {
            var store = this.Store as IQueryableUserStore<TargetUser, int>;
            if (store == null)
                throw new NotSupportedException();

            return store.Users.FirstOrDefault(u => u.Mobile == Mobile);
        }

	}
}
