using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 表示通知服务。
    /// </summary>
    public interface IPersonNotificationService
    {
        /// <summary>
        /// 发送实名身份验证短信。
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task NotifyRealIdValidationStateAsync(Person person);
    }
}
