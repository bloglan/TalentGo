using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 提供短信发送服务。
    /// </summary>
    public interface ITalentGoSMSService
    {
        /// <summary>
        /// 发送实名身份验证短信。
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task SendRealIdValidationMessageAsync(Person person);

        /// <summary>
        /// 发送资料审查消息。
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task SendFileReviewMessageAsync(ApplicationForm form);

        /// <summary>
        /// 发送报名表审核短信。
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task SendApplicationFormAuditMessageAsync(ApplicationForm form);
    }
}
