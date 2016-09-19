using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Utilities
{
    /// <summary>
    /// 表示一个手机号验证管理器。
    /// </summary>
    public class MobileValidationSessionManager
    {
        IMobileValidationSessionStore store;
        public MobileValidationSessionManager(IMobileValidationSessionStore Store)
        {
            this.store = Store;
        }

        public IQueryable<MobilePhoneValidationSession> Sessions
        {
            get { return this.store.Sessions; }
        }

        public async Task<MobilePhoneValidationSession> FindAvailableSession(string Mobile)
        {
            return (from s in this.store.Sessions
                   where s.Mobile == Mobile && s.ExpirationDate > DateTime.Now && !s.IsValid
                   orderby s.WhenCreated descending
                   select s).FirstOrDefault();
        }

        public async Task CreateSession(MobilePhoneValidationSession session)
        {
            await this.store.CreateAsync(session);
        }

        public async Task UpdateSession(MobilePhoneValidationSession session)
        {
            await this.store.UpdateAsync(session);
        }

        public async Task DeleteSession(MobilePhoneValidationSession session)
        {
            await this.store.DeleteAsync(session);
        }


        public async Task<bool> Validate(string Mobile, string ValidationCode)
        {
            var session = await this.FindAvailableSession(Mobile);
            if (session == null)
                return false;

            bool result = session.ValidateCode.ToLower() == ValidationCode.ToLower();

            session.IsValid = true;
            session.LastTryTime = DateTime.Now;
            await this.store.UpdateAsync(session);

            return result;
        }
    }
}
