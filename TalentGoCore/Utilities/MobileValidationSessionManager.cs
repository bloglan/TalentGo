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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Store"></param>
        public MobileValidationSessionManager(IMobileValidationSessionStore Store)
        {
            this.store = Store;
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<MobilePhoneValidationSession> Sessions
        {
            get { return this.store.Sessions; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mobile"></param>
        /// <returns></returns>
        public async Task<MobilePhoneValidationSession> FindAvailableSession(string Mobile)
        {
            return (from s in this.store.Sessions
                   where s.Mobile == Mobile && s.ExpirationDate > DateTime.Now && !s.IsValid
                   orderby s.WhenCreated descending
                   select s).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task CreateSession(MobilePhoneValidationSession session)
        {
            await this.store.CreateAsync(session);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task UpdateSession(MobilePhoneValidationSession session)
        {
            await this.store.UpdateAsync(session);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task DeleteSession(MobilePhoneValidationSession session)
        {
            await this.store.DeleteAsync(session);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mobile"></param>
        /// <param name="ValidationCode"></param>
        /// <returns></returns>
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
