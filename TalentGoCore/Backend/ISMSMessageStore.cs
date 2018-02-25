using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Backend
{
    /// <summary>
    /// 短信消息存储。
    /// </summary>
    public interface ISMSMessageStore
    {
        /// <summary>
        /// 短信消息集合。
        /// </summary>
        IQueryable<SMSMessageBag> SMSMessages { get; }

        /// <summary>
        /// 创建短信消息。
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task CreateAsync(SMSMessageBag message);

        /// <summary>
        /// 移除短信消息。
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task RemoveAsync(SMSMessageBag message);
    }
}