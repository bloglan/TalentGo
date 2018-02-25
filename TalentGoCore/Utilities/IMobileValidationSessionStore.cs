using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Utilities
{
    /// <summary>
    /// 提供对手机验证会话数据的存取能力。
    /// </summary>
    public interface IMobileValidationSessionStore
    {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<MobilePhoneValidationSession> Sessions { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        Task CreateAsync(MobilePhoneValidationSession session);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        Task UpdateAsync(MobilePhoneValidationSession session);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        Task DeleteAsync(MobilePhoneValidationSession session);
    }
}
