using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// 获取或设置身份证图像OCR识别服务。
        /// </summary>
        public virtual IIDCardOCRService IDCardOCRService { get; set; }

        /// <summary>
        /// 获取或设置短信服务。
        /// </summary>
        public virtual ITalentGoSMSService SMSService { get; set; }

        /// <summary>
        /// 获取或设置邮件服务。
        /// </summary>
        public virtual ITalentGoEmailService EmailService { get; set; }

        /// <summary>
        /// Find person by it's id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Person> FindByIdAsync(Guid id)
        {
            return await this.Store.FindByIdAsync(id);
        }

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
        /// 
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task CreateAsync(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            await this.Store.CreateAsync(person);
        }

        /// <summary>
        /// 修改身份信息。
        /// </summary>
        /// <param name="person"></param>
        /// <param name="surname"></param>
        /// <param name="givenName"></param>
        /// <param name="ethnicity"></param>
        /// <param name="address"></param>
        /// <param name="issuer"></param>
        /// <param name="issueDate"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public async Task UpdateRealIdAsync(Person person, string surname, string givenName, string ethnicity, string address, string issuer, DateTime issueDate, DateTime? expiresAt)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            if (person.WhenRealIdCommited.HasValue)
                throw new InvalidOperationException("已提交身份验证后不能修改身份信息。");

            //TODO：填充字段
            person.Surname = surname;
            person.GivenName = givenName;
            person.Ethnicity = ethnicity;
            person.Address = address;
            person.Issuer = issuer;
            person.IssueDate = issueDate;
            person.ExpiresAt = expiresAt;
            await this.Store.UpdateAsync(person);
        }

        /// <summary>
        /// 传送身份证个人信息面。
        /// </summary>
        /// <param name="person"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UploadIDCardFrontFileAsync(Person person, Stream data)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (person.WhenRealIdCommited.HasValue)
                throw new InvalidOperationException("已提交身份验证不能修改身份信息。");

            if (!string.IsNullOrEmpty(person.IDCardFrontFile))
                throw new InvalidOperationException("已存在身份证图像");

            File newFile;
            using (var ms = new MemoryStream())
            {
                data.CopyTo(ms);
                if (data.Length > 1024 * 1024)
                    throw new ArgumentException("图片太大，不能超过1MB。");

                ms.Position = 0;
                this.EnsureImage(ms, out string mimeType);

                ms.Position = 0;
                newFile = new File(Guid.NewGuid().ToString(), mimeType);
                await newFile.ReadAsync(ms);
            }
            await this.FileStore.CreateAsync(newFile);

            person.IDCardFrontFile = newFile.Id;
            person.RealIdValid = null;
            await this.Store.UpdateAsync(person);
        }

        /// <summary>
        /// 传送身份证国徽面。
        /// </summary>
        /// <param name="person"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UploadIDCardBackFileAsync(Person person, Stream data)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (person.WhenRealIdCommited.HasValue)
                throw new InvalidOperationException("已提交身份验证不能修改身份信息。");

            if (!string.IsNullOrEmpty(person.IDCardBackFile))
                throw new InvalidOperationException("已存在身份证图像");

            File newFile;
            using (var ms = new MemoryStream())
            {
                data.CopyTo(ms);
                if (data.Length > 1024 * 1024)
                    throw new ArgumentException("图片太大，不能超过1MB。");

                ms.Position = 0;
                this.EnsureImage(ms, out string mimeType);

                ms.Position = 0;
                newFile = new File(Guid.NewGuid().ToString(), mimeType);
                await newFile.ReadAsync(ms);
            }
            await this.FileStore.CreateAsync(newFile);

            person.IDCardBackFile = newFile.Id;
            person.RealIdValid = null;
            await this.Store.UpdateAsync(person);
        }

        /// <summary>
        /// Remove id card front file.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task RemoveIDCardFrontFile(Person person)
        {
            if (person == null)
                throw new ArgumentNullException();

            if (person.WhenRealIdCommited.HasValue)
                throw new InvalidOperationException("已提交身份验证不能修改身份信息。");

            if (string.IsNullOrEmpty(person.IDCardFrontFile))
                return;

            var file = await this.FileStore.FindByIdAsync(person.IDCardFrontFile);
            if (file != null)
                await this.FileStore.DeleteAsync(file);

            person.IDCardFrontFile = null;
            await this.Store.UpdateAsync(person);
        }

        /// <summary>
        /// Remove ID card back file.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task RemoveIDCardBackFile(Person person)
        {
            if (person == null)
                throw new ArgumentNullException();

            if (person.WhenRealIdCommited.HasValue)
                throw new InvalidOperationException("已提交身份验证不能修改身份信息。");

            if (string.IsNullOrEmpty(person.IDCardBackFile))
                return;

            var file = await this.FileStore.FindByIdAsync(person.IDCardBackFile);
            if (file != null)
                await this.FileStore.DeleteAsync(file);

            person.IDCardBackFile = null;
            await this.Store.UpdateAsync(person);
        }

        void EnsureImage(Stream data, out string mimeType)
        {
            using (var img = Image.FromStream(data))
            {
                if (img.Size.Width < 506 || img.Size.Height < 319)
                    throw new ArgumentException("图片过小。图片尺寸至少大于500*310像素。");

                if (img.RawFormat.Equals(ImageFormat.Jpeg))
                    mimeType = "image/jpeg";
                else if (img.RawFormat.Equals(ImageFormat.Png))
                    mimeType = "image/png";
                else
                    throw new NotSupportedException("不支持的文件格式。");
            }
        }

        /// <summary>
        /// 提交实名身份验证信息。
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task CommitForRealIdValidationAsync(Person person)
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
            person.RealIdCommitCount += 1;

            //清理任何审核标记，以便为此次审核做准备
            person.RealIdValid = null;
            person.WhenRealIdValid = null;
            person.RealIdValidBy = null;
            person.RealIdValidationMessage = null;

            //使用自动验证器尝试验证。
            try
            {
                if (await this.ValidateRealIdByOCRService(person))
                {
                    //如果验证通过，则设置RealIdValid=true
                    await this.ValidateRealId(person, true, "云识别");
                }
                //否则不设置，转人工服务。
            }
            catch (IDCardRecognizeException recognizeException)
            {
                Trace.TraceError("身份证识别异常：" + recognizeException.Message);
                //Do Nothing.
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                //Do nothing here.
            }

            await this.Store.UpdateAsync(person);
        }

        /// <summary>
        /// 通过OCR验证实名制信息。
        /// </summary>
        /// <param name="person"></param>
        /// <returns>若验证成功，返回true，否则返回false.</returns>
        protected virtual async Task<bool> ValidateRealIdByOCRService(Person person)
        {
            if (this.IDCardOCRService == null)
                return false;

            var frontFile = await this.FileStore.FindByIdAsync(person.IDCardFrontFile);
            IDCardFrontOCRResult frontResult;
            using (var ms = new MemoryStream())
            {
                await frontFile.WriteAsync(ms);
                ms.Position = 0;
                try
                {
                    frontResult = await this.IDCardOCRService.RecognizeIDCardFront(ms);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            var backFile = await this.FileStore.FindByIdAsync(person.IDCardBackFile);
            IDCardBackOCRResult backResult;
            using (var ms = new MemoryStream())
            {
                await backFile.WriteAsync(ms);
                ms.Position = 0;
                try
                {
                    backResult = await this.IDCardOCRService.RecognizeIDCardBack(ms);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            if (person.DisplayName != frontResult.Name)
                return false;
            //if (person.Address != frontResult.Address)
            //    return false;
            if (person.Ethnicity != frontResult.Nationality)
                return false;
            if (person.IDCardNumber != frontResult.IDCardNumber)
                return false;
            //if (person.Issuer != backResult.Issuer)
            //    return false;
            //if (person.IssueDate != backResult.IssueDate)
            //    return false;
            //if (person.ExpiresAt != backResult.ExpiresDate)
            //    return false;

            return true;
        }

        /// <summary>
        /// 手动验证实名信息。
        /// </summary>
        /// <param name="person"></param>
        /// <param name="accepted"></param>
        /// <param name="validateBy"></param>
        /// <param name="validationMessage"></param>
        /// <returns></returns>
        public async Task ValidateRealId(Person person, bool accepted, string validateBy, string validationMessage = null)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            if (!person.WhenRealIdCommited.HasValue)
                throw new InvalidOperationException("实名验证尚未提交。");

            person.RealIdValid = accepted;
            person.WhenRealIdValid = DateTime.Now;
            person.RealIdValidBy = validateBy;
            person.RealIdValidationMessage = validationMessage;

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

        /// <summary>
        /// 将实名认证信息退回给用户。
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task ReturnBackAsync(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            person.WhenRealIdCommited = null;
            await this.Store.UpdateAsync(person);
        }

        /// <summary>
        /// 删除用户。
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Person person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            if (!string.IsNullOrEmpty(person.IDCardFrontFile))
            {
                var file = await this.FileStore.FindByIdAsync(person.IDCardFrontFile);
                if (file != null)
                    await this.FileStore.DeleteAsync(file);
            }
            if (!string.IsNullOrEmpty(person.IDCardBackFile))
            {
                var file = await this.FileStore.FindByIdAsync(person.IDCardBackFile);
                if (file != null)
                    await this.FileStore.DeleteAsync(file);
            }
            await this.Store.DeleteAsync(person);
        }
    }
}
