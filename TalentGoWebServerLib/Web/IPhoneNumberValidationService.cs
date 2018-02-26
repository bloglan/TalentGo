using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Web
{
    /// <summary>
    /// 表示一个验证手机号码的服务接口。
    /// </summary>
    public interface IPhoneNumberValidationService
    {
        /// <summary>
        /// Send Validation code to target phone number, if success, return the validation code.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        Task<string> SendValidationCodeAsync(string phoneNumber);

        /// <summary>
        /// Validate that user can received validation code.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<bool> ValidateAsync(string phoneNumber, string code);
    }
}
