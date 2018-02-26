using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Web
{
    /// <summary>
    /// 手机验证码服务。
    /// </summary>
    public class PhoneNumberValidationService : IPhoneNumberValidationService
    {
        /// <summary>
        /// 向指定的手机发送短信验证码。
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public async Task<string> SendValidationCodeAsync(string phoneNumber)
        {
            using (var client = new ValidationCodeSvc.VerificationCodeClient())
            {
                var result = await client.SendAsync(phoneNumber);
                if (result.StatusCode != 0)
                    throw new InvalidOperationException("发送验证码时出现无效操作，代码" + result.StatusCode + "，消息：" + result.Message);

                return result.VerificationCode;
            }
        }

        /// <summary>
        /// 验证短信验证码是否正确。
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> ValidateAsync(string phoneNumber, string code)
        {
            using (var client = new ValidationCodeSvc.VerificationCodeClient())
            {
                return await client.VerifyAsync(phoneNumber, code);
            }
        }
    }
}
