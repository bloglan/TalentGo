using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApplicationFormNotificationService
    {
        /// <summary>
        /// 发送资料审查消息。
        /// </summary>
        /// <param name="form"></param>
        /// 
        /// <returns></returns>
        Task NotifyFileReviewStateAsync(ApplicationForm form);

        /// <summary>
        /// 发送报名表审核短信。
        /// </summary>
        /// <param name="form"></param>
        /// 
        /// <returns></returns>
        Task NotifyAuditStateAsync(ApplicationForm form);
    }
}
