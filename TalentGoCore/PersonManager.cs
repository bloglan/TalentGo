using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 用户管理器。
    /// </summary>
    public class PersonManager
    {
        /// <summary>
        /// 使用用户存储初始化用户管理器。
        /// </summary>
        /// <param name="store"></param>
        public PersonManager(IPersonStore store)
        {
            this.Store = store;
        }

        /// <summary>
        /// Protected store
        /// </summary>
        protected IPersonStore Store { get; set; }

        /// <summary>
        /// 修改身份信息。
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="idCardNumber"></param>
        /// <param name="surname"></param>
        /// <param name="givenName"></param>
        /// <param name="sex"></param>
        /// <param name="ethnicity"></param>
        /// <param name="dateofBirth"></param>
        /// <param name="address"></param>
        /// <param name="issuer"></param>
        /// <param name="issueDate"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public async Task UpdateRealNameInfo(Guid personId, string idCardNumber, string surname, string givenName, Sex sex, string ethnicity, DateTime dateofBirth, string address, string issuer, DateTime issueDate, DateTime? expiresAt)
        {
            var person = await this.Store.FindByIdAsync(personId);

            if (person.IDCardValid.HasValue && person.IDCardValid.Value)
                throw new InvalidOperationException("已通过身份证验证后不能修改身份信息。");

            //TODO：填充字段
            //TODO：调用接口尝试验证。
            //如果验证通过，则设定IDCardValid = true
            //否则留空。
            await this.Store.UpdateAsync(person);
        }
    }
}
