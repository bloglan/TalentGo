using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Utilities
{
    /// <summary>
    /// 提供对手机验证会话数据的存取能力。
    /// </summary>
    public interface IMobileValidationSessionStore
    {
        IQueryable<MobilePhoneValidationSession> Sessions { get; }

        Task CreateAsync(MobilePhoneValidationSession session);

        Task UpdateAsync(MobilePhoneValidationSession session);

        Task DeleteAsync(MobilePhoneValidationSession session);
    }
}
