using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// TalentGo电子邮件服务。
    /// </summary>
    public interface ITalentGoEmailService
    {
        /// <summary>
        /// 发送实名身份验证短信。
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task SendRealIdValidationMailAsync(Person person);

        /// <summary>
        /// 发送资料审查消息。
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task SendFileReviewMailAsync(ApplicationForm form);

        /// <summary>
        /// 发送报名表审核短信。
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task SendApplicationFormAuditMailAsync(ApplicationForm form);
    }
}
