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
        /// <param name="fileStore"></param>
        public PersonManager(IPersonStore store, IFileStore fileStore)
        {
            this.Store = store;
            this.FileStore = fileStore;
        }

        /// <summary>
        /// Protected store
        /// </summary>
        protected IPersonStore Store { get; set; }

        /// <summary>
        /// FileStore.
        /// </summary>
        protected IFileStore FileStore { get; set; }

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

            if (person.WhenRealIdCommited.HasValue)
                throw new InvalidOperationException("已提交身份验证后不能修改身份信息。");

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
        /// 传送身份证个人信息面。
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

            if (person.WhenRealIdCommited.HasValue)
                throw new InvalidOperationException("已提交身份验证不能修改身份信息。");

            var newFile = new File(Guid.NewGuid().ToString(), mimeType);
            await newFile.ReadAsync(stream);
            await this.FileStore.CreateAsync(newFile);


            if (!string.IsNullOrEmpty(person.IDCardFrontFile))
            {
                var oldFile = await this.FileStore.FindByIdAsync(person.IDCardFrontFile);
                if (oldFile != null)
                    await this.FileStore.DeleteAsync(oldFile);
            }

            person.IDCardFrontFile = newFile.Id;
            person.RealIdValid = null;
            await this.Store.UpdateAsync(person);

        }

        /// <summary>
        /// 传送身份证国徽面。
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

            if (person.WhenRealIdCommited.HasValue)
                throw new InvalidOperationException("已提交身份验证不能修改身份信息。");

            var newFile = new File(Guid.NewGuid().ToString(), mimeType);
            await newFile.ReadAsync(stream);
            await this.FileStore.CreateAsync(newFile);


            if (!string.IsNullOrEmpty(person.IDCardBackFile))
            {
                var oldFile = await this.FileStore.FindByIdAsync(person.IDCardBackFile);
                if (oldFile != null)
                    await this.FileStore.DeleteAsync(oldFile);
            }

            person.IDCardBackFile = newFile.Id;
            person.RealIdValid = null;
            await this.Store.UpdateAsync(person);

        }

        /// <summary>
        /// 提交实名身份验证信息。
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task CommitForRealIdValidation(Person person)
        {
            if (person == null)
                throw new ArgumentNullException();

            if (person.WhenRealIdCommited.HasValue)
                return;

            //检查所需字段是否填写
            if (string.IsNullOrEmpty(person.Ethnicity)
                || string.IsNullOrEmpty(person.Address)
                || string.IsNullOrEmpty(person.Issuer)
                || !person.IssueDate.HasValue)
                throw new InvalidOperationException("实名制信息不完整。");

            if (string.IsNullOrEmpty(person.IDCardFrontFile) || string.IsNullOrEmpty(person.IDCardBackFile))
                throw new InvalidOperationException("未上传身份证图像。");

            person.WhenRealIdCommited = DateTime.Now;
            //使用自动验证器尝试验证。
            //如果验证通过，则设置RealIdValid=true
            //否则不设置，转人工服务。
            await this.Store.UpdateAsync(person);
        }

        /// <summary>
        /// 手动验证实名信息。
        /// </summary>
        /// <param name="person"></param>
        /// <param name="accepted"></param>
        /// <returns></returns>
        public async Task ValidateRealId(Person person, bool accepted)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            if (!person.WhenRealIdCommited.HasValue)
                throw new InvalidOperationException("实名验证尚未提交。");

            person.RealIdValid = accepted;
            person.WhenRealIdValid = DateTime.Now;
            person.RealIdValidBy = "Manual";

            await this.Store.UpdateAsync(person);
        }

        /// <summary>
        /// 重置实名验证信息。
        /// 改操作不修改用户资料，但会重置实名验证提交时间、验证标记、验证时间等相关控制字段。
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task ResetRealIdValidation(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            person.WhenRealIdCommited = null;
            person.RealIdValid = null;
            person.WhenRealIdValid = null;
            person.RealIdValidBy = null;

            await this.Store.UpdateAsync(person);
        }
    }
}
