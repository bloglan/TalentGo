using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        /// Find user by Phone Number.
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public Task<Person> FindByMobileAsync(string mobile)
        {
            var user = this.Store.People.FirstOrDefault(p => p.Mobile == mobile);
            return Task.FromResult(user);
        }

        /// <summary>
        /// Gets all people.
        /// </summary>
        public IQueryable<Person> People => this.Store.People;

        /// <summary>
        /// 修改身份信息。
        /// </summary>
        /// <param name="person"></param>
        /// <param name="idCardNumber"></param>
        /// <param name="surname"></param>
        /// <param name="givenName"></param>
        /// <param name="ethnicity"></param>
        /// <param name="address"></param>
        /// <param name="issuer"></param>
        /// <param name="issueDate"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public async Task UpdateRealNameInfo(Person person, string idCardNumber, string surname, string givenName, string ethnicity, string address, string issuer, DateTime issueDate, DateTime? expiresAt)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            if (person.RealIdValid.HasValue && person.RealIdValid.Value)
                throw new InvalidOperationException("已通过身份证验证后不能修改身份信息。");

            var cardNumber = ChineseIDCardNumber.Parse(idCardNumber);

            //TODO：填充字段
            person.IDCardNumber = cardNumber.ToString();
            person.Surname = surname;
            person.GivenName = givenName;
            person.Sex = cardNumber.IsMale?Sex.Male:Sex.Female;
            person.Ethnicity = ethnicity;
            person.DateOfBirth = cardNumber.DateOfBirth;
            person.Address = address;
            person.Issuer = issuer;
            person.IssueDate = issueDate;
            person.ExpiresAt = expiresAt;
            person.WhenRealIdCommited = DateTime.Now;
            person.RealIdValid = null;
            //TODO：调用接口尝试验证。
            //如果验证通过，则设定IDCardValid = true
            //否则留空。
            await this.Store.UpdateAsync(person);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <param name="mimeType"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task UploadIDCardFrontFileAsync(Person person, string mimeType, Stream stream)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (person.RealIdValid.HasValue && person.RealIdValid.Value)
                throw new InvalidOperationException("已通过身份验证不能修改身份信息");

            if (person.IDCardFrontFile == null)
                person.IDCardFrontFile = new File();
            person.IDCardFrontFile.MimeType = mimeType;
            await person.IDCardFrontFile.ReadAsync(stream);
            person.RealIdValid = null;
            //TODO：调用接口尝试验证。
            //如果验证通过，则设定IDCardValid = true
            //否则留空。
            await this.Store.UpdateAsync(person);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <param name="mimeType"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task UploadIDCardBackFileAsync(Person person, string mimeType, Stream stream)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (person.RealIdValid.HasValue && person.RealIdValid.Value)
                throw new InvalidOperationException("已通过身份验证不能修改身份信息");

            if (person.IDCardBackFile == null)
                person.IDCardBackFile = new File();
            person.IDCardBackFile.MimeType = mimeType;
            await person.IDCardBackFile.ReadAsync(stream);
            person.RealIdValid = null;
            //TODO：调用接口尝试验证。
            //如果验证通过，则设定IDCardValid = true
            //否则留空。
            await this.Store.UpdateAsync(person);

        }

    }
}
