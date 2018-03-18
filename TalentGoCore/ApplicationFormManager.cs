using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TalentGo.Linq;
using System.Transactions;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Text;

namespace TalentGo
{
    /// <summary>
    /// 表示招聘报名管理器
    /// </summary>
    public class ApplicationFormManager
    {
        IApplicationFormStore applicationFormStore;
        IFileStore fileStore;

        /// <summary>
        /// 构造函数。使用给定的报名表存储、招聘计划管理器和目标用户管理器初始化报名管理器。
        /// </summary>
        /// <param name="applicationFormStore"></param>
        /// <param name="fileStore"></param>
		public ApplicationFormManager(IApplicationFormStore applicationFormStore, IFileStore fileStore)
        {
            this.applicationFormStore = applicationFormStore;
            this.fileStore = fileStore;
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<ApplicationForm> ApplicationForms
        {
            get { return this.applicationFormStore.ApplicationForms; }
        }

        /// <summary>
        /// Find application form by id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<ApplicationForm> FindByIdAsync(int Id)
        {
            return await this.applicationFormStore.FindByIdAsync(Id);
        }

        /// <summary>
        /// 为指定的用户和招聘计划创建报名表。
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task EnrollAsync(ApplicationForm form)
        {
            //根据要求，一个用户只能参与一个报名。
            var currentPlanId = form.Job.PlanId;
            var currentPersonId = form.PersonId;

            if (this.applicationFormStore.ApplicationForms.Any(a => a.Job.PlanId == currentPlanId && a.PersonId == currentPersonId))
            {
                throw new InvalidOperationException("在一个招聘计划内只能创建一份报名表。");
            }

            await this.applicationFormStore.CreateAsync(form);
        }

        /// <summary>
        /// Upload head image.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> UploadHeadImageAsync(ApplicationForm form, Stream data)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (!string.IsNullOrEmpty(form.HeadImageFile))
                throw new InvalidOperationException("Head image file exists.");

            this.EnsureImage(data, out string mimeType);

            var file = new File(Guid.NewGuid().ToString(), mimeType);
            data.Position = 0;
            await file.ReadAsync(data);
            await this.fileStore.CreateAsync(file);

            form.HeadImageFile = file.Id;
            await this.applicationFormStore.UpdateAsync(form);

            return file.Id;
        }


        /// <summary>
        /// Remove HeadImage.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task RemoveHeadImageAsync(ApplicationForm form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            if (string.IsNullOrEmpty(form.HeadImageFile))
                return;

            var file = await this.fileStore.FindByIdAsync(form.HeadImageFile);
            await this.fileStore.DeleteAsync(file);
            form.HeadImageFile = null;
            await this.applicationFormStore.UpdateAsync(form);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> UploadAcademicCertFileAsync(ApplicationForm form, Stream data)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (form.WhenCommited.HasValue)
                throw new InvalidOperationException("Cannot upload file to application form that commited.");

            this.EnsureImage(data, out string mimeType);

            var file = new File(Guid.NewGuid().ToString(), mimeType);
            data.Position = 0;
            await file.ReadAsync(data);
            await this.fileStore.CreateAsync(file);

            form.AcademicCertFileList.Add(file.Id);

            await this.applicationFormStore.UpdateAsync(form);
            return file.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task RemoveAcademicFileAsync(ApplicationForm form, string fileId)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));
            if (string.IsNullOrEmpty(fileId))
                throw new ArgumentException("fileId is null or empty.");

            var list = form.AcademicCertFileList;

            if (list.Contains(fileId))
            {
                list.Remove(fileId);
                var file = await this.fileStore.FindByIdAsync(fileId);
                if (file != null)
                {
                    await this.fileStore.DeleteAsync(file);
                }
            }
            await this.applicationFormStore.UpdateAsync(form);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> UploadDegreeCertFileAsync(ApplicationForm form, Stream data)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (form.WhenCommited.HasValue)
                throw new InvalidOperationException("Cannot upload file to application form that commited.");

            this.EnsureImage(data, out string mimeType);

            var file = new File(Guid.NewGuid().ToString(), mimeType);
            data.Position = 0;
            await file.ReadAsync(data);
            await this.fileStore.CreateAsync(file);

            form.DegreeCertFileList.Add(file.Id);

            await this.applicationFormStore.UpdateAsync(form);
            return file.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task RemoveDegreeCertFileAsync(ApplicationForm form, string fileId)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));
            if (string.IsNullOrEmpty(fileId))
                throw new ArgumentException("fileId is null or empty.");

            var list = form.DegreeCertFileList;

            if (list.Contains(fileId))
            {
                list.Remove(fileId);
                var file = await this.fileStore.FindByIdAsync(fileId);
                if (file != null)
                {
                    await this.fileStore.DeleteAsync(file);
                }
            }
            await this.applicationFormStore.UpdateAsync(form);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> UploadOtherFileAsync(ApplicationForm form, Stream data)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (form.WhenCommited.HasValue)
                throw new InvalidOperationException("Cannot upload file to application form that commited.");

            this.EnsureImage(data, out string mimeType);

            var file = new File(Guid.NewGuid().ToString(), mimeType);
            data.Position = 0;
            await file.ReadAsync(data);
            await this.fileStore.CreateAsync(file);

            form.OtherFileList.Add(file.Id);

            await this.applicationFormStore.UpdateAsync(form);
            return file.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task RemoveOtherFileAsync(ApplicationForm form, string fileId)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));
            if (string.IsNullOrEmpty(fileId))
                throw new ArgumentException("fileId is null or empty.");

            var list = form.OtherFileList;

            if (list.Contains(fileId))
            {
                list.Remove(fileId);
                var file = await this.fileStore.FindByIdAsync(fileId);
                if (file != null)
                {
                    await this.fileStore.DeleteAsync(file);
                }
            }
            await this.applicationFormStore.UpdateAsync(form);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mimeType"></param>
        void EnsureImage(Stream data, out string mimeType)
        {
            using (var image = Image.FromStream(data))
            {
                if (image.RawFormat.Equals(ImageFormat.Jpeg))
                    mimeType = "image/jpeg";
                else if (image.RawFormat.Equals(ImageFormat.Png))
                    mimeType = "image/png";
                else
                    throw new NotSupportedException("不支持的文件格式。");

                if (image.Size.Width * image.Size.Height < 800 * 800)
                    throw new ArgumentException("图片过小。");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task ModifyAsync(ApplicationForm form)
        {
            if (form == null)
                throw new ArgumentNullException();

            if (form.WhenCommited.HasValue)
                throw new InvalidOperationException("已提交的报名表不能修改。");

            form.WhenChanged = DateTime.Now;

            await this.applicationFormStore.UpdateAsync(form);
        }

        /// <summary>
        /// 提交报名资料。
        /// </summary>
        /// <returns></returns>
        public async Task CommitAsync(ApplicationForm form)
        {
            if (form == null)
                throw new ArgumentNullException();

            if (form.Id == 0)
                await this.EnrollAsync(form);

            if (form.WhenCommited.HasValue)
                throw new InvalidOperationException("Application form has been commited.");

            //如果没有FileReview标记，则不允许在超过报名时间提交。
            if (!form.FileReviewAccepted.HasValue && form.Job.Plan.EnrollExpirationDate < DateTime.Now)
            {
                throw new InvalidOperationException("报名截止时间已过，不能再提交。");
            }

            //检查传送资料是否齐全并满足规则。
            if (string.IsNullOrEmpty(form.HeadImageFile))
                throw new InvalidOperationException("需要证件照。");

            if (!form.AcademicCertFileList.Any())
                throw new InvalidOperationException("至少需要上传一份学历证明文件。");

            //设置提交时间。
            form.WhenCommited = DateTime.Now;
            //清除资料审查标记，以便将此报名表自再次加入资料审查列表中。
            form.WhenFileReview = null;
            form.FileReviewAccepted = null;

            await this.applicationFormStore.UpdateAsync(form);
        }

        /// <summary>
        /// File review.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="accepted"></param>
        /// <returns></returns>
        public async Task FileReviewAsync(ApplicationForm form, bool accepted)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            if (!form.WhenCommited.HasValue)
                throw new InvalidOperationException("Form not commited.");

            if (form.WhenFileReview.HasValue)
                throw new InvalidOperationException("File has been reviewed.");

            form.FileReviewAccepted = accepted;
            form.WhenFileReview = DateTime.Now;

            await this.applicationFormStore.UpdateAsync(form);
        }

        /// <summary>
        /// 删除报名表。
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task DeleteAsync(ApplicationForm form)
        {
            //如果该报名表已经提交，则不能删除。
            if (form.WhenCommited.HasValue)
                throw new InvalidOperationException("已提交的报名表不能删除。");

            //与此报名表关联的文件。
            var fileList = new List<string>();
            fileList.Union(form.AcademicCertFileList);
            fileList.Union(form.DegreeCertFileList);
            fileList.Union(form.OtherFileList);
            if (!string.IsNullOrEmpty(form.HeadImageFile))
                fileList.Add(form.HeadImageFile);

            foreach (var fileId in fileList)
            {
                var file = await this.fileStore.FindByIdAsync(fileId);
                if (file != null)
                    await this.fileStore.DeleteAsync(file);
            }
            await this.applicationFormStore.DeleteAsync(form);
        }

        /// <summary>
        /// 将报名表退回给求职者。
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task ReturnBackAsync(ApplicationForm form)
        {
            if (form == null)
                throw new ArgumentNullException();

            form.WhenCommited = null;
            //form.FileReviewAccepted = null;
            //form.WhenFileReview = null;
            //form.Approved = null;
            //form.WhenAudit = null;
            //form.AuditMessage = null;
            form.Log("退回报名表。");

            await this.applicationFormStore.UpdateAsync(form);
        }


        /// <summary>
        /// 设置审核标记。
        /// </summary>
        /// <param name="form"></param>
        /// <param name="approved"></param>
        /// <returns></returns>
        public async Task SetAuditFlagAsync(ApplicationForm form, bool? approved)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            form.Approved = approved;

            await this.applicationFormStore.UpdateAsync(form);
        }

        /// <summary>
        /// 设置审核消息。
        /// </summary>
        /// <param name="form"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SetAuditMessageAsync(ApplicationForm form, string message)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            form.AuditMessage = message;

            await this.applicationFormStore.UpdateAsync(form);
        }

        /// <summary>
        /// 完成审核。
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public async Task CompleteAuditAsync(ApplicationForm form)
        {
            if (form == null)
                throw new ArgumentNullException();

            if (form.WhenAudit.HasValue)
                return;

            if (!form.WhenFileReview.HasValue)
                throw new InvalidOperationException("Can not complete audit before file review");

            if (form.FileReviewAccepted.HasValue && !form.FileReviewAccepted.Value)
                form.Approved = false; //前置条件 资料审查未通过，直接拒绝审核。

            if (!form.Approved.HasValue)
                form.Approved = false; //如果没有指定审核标记，完成时默认拒绝。

            form.WhenAudit = DateTime.Now;
            await this.applicationFormStore.UpdateAsync(form);
        }

        /// <summary>
        /// 声明是否参加考试。
        /// </summary>
        /// <param name="form"></param>
        /// <param name="IsTakeExam"></param>
        /// <returns></returns>
		public async Task AnnounceForExamAsync(ApplicationForm form, bool IsTakeExam)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            //必须是已提交的，通过审核的，尚未声明的，当前在声明有效期内的。
            if (!form.WhenAudit.HasValue)
                throw new InvalidOperationException("无效的操作，尚未审核。");

            if (!form.Approved.Value)
                throw new InvalidOperationException("无效的操作，审核未通过。");

            if (form.WhenAnnounced.HasValue)
                throw new InvalidOperationException("无效的操作，已进行了声明。不能重复声明。");

            if (form.Job.Plan.AnnounceExpirationDate.Value < DateTime.Now)
                throw new InvalidOperationException("无效的操作，已过声明截止时间。");

            form.IsTakeExam = IsTakeExam;
            form.WhenAnnounced = DateTime.Now;

            await this.applicationFormStore.UpdateAsync(form);
        }
    }
}
