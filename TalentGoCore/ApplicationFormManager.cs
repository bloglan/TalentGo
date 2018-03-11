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
            await file.ReadAsync(data);
            await this.fileStore.CreateAsync(file);

            var list = this.GetFileList(form.AcademicCertFiles);
            list.Add(file.Id);
            form.AcademicCertFiles = this.GetFilesProperty(list);

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

            var list = this.GetFileList(form.AcademicCertFiles);

            if (list.Contains(fileId))
            {
                list.Remove(fileId);
                var file = await this.fileStore.FindByIdAsync(fileId);
                if (file != null)
                {
                    await this.fileStore.DeleteAsync(file);
                }
            }
            form.AcademicCertFiles = this.GetFilesProperty(list);
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
            await file.ReadAsync(data);
            await this.fileStore.CreateAsync(file);

            var list = this.GetFileList(form.DegreeCertFiles);
            list.Add(file.Id);
            form.DegreeCertFiles = this.GetFilesProperty(list);

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

            var list = this.GetFileList(form.DegreeCertFiles);

            if (list.Contains(fileId))
            {
                list.Remove(fileId);
                var file = await this.fileStore.FindByIdAsync(fileId);
                if (file != null)
                {
                    await this.fileStore.DeleteAsync(file);
                }
            }
            form.DegreeCertFiles = this.GetFilesProperty(list);
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
            await file.ReadAsync(data);
            await this.fileStore.CreateAsync(file);

            var list = this.GetFileList(form.OtherFiles);
            list.Add(file.Id);
            form.OtherFiles = this.GetFilesProperty(list);

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

            var list = this.GetFileList(form.OtherFiles);

            if (list.Contains(fileId))
            {
                list.Remove(fileId);
                var file = await this.fileStore.FindByIdAsync(fileId);
                if (file != null)
                {
                    await this.fileStore.DeleteAsync(file);
                }
            }
            form.OtherFiles = this.GetFilesProperty(list);
            await this.applicationFormStore.UpdateAsync(form);
        }


        void EnsureImage(Stream data, out string mimeType)
        {
            using (var image = Image.FromStream(data))
            {
                if (image.RawFormat.Equals(ImageFormat.Jpeg))
                    mimeType = "image/jpeg";
                else if (image.RawFormat.Equals(ImageFormat.Png))
                    mimeType = "image/png";
                else
                    throw new NotSupportedException("File format not support.");

                if (image.Size.Width * image.Size.Height < 800 * 800)
                    throw new ArgumentException("Image size is small!");
            }
        }

        /// <summary>
        /// Get file list from files property.
        /// </summary>
        /// <param name="filesProperty"></param>
        /// <returns></returns>
        public List<string> GetFileList(string filesProperty)
        {
            if (string.IsNullOrEmpty(filesProperty))
                return new List<string>();
            var array = filesProperty.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            return new List<string>(array);
        }

        /// <summary>
        /// Get files property from file list.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public string GetFilesProperty(List<string> list)
        {
            if (list == null)
                return null;
            var builder = new StringBuilder();
            foreach (var item in list)
            {
                builder.Append(item + "|");
            }
            return builder.ToString();
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

            //TODO:检查传送资料是否齐全并满足规则。
            if (string.IsNullOrEmpty(form.HeadImageFile))
                throw new InvalidOperationException("Head image required.");

            var academicFileList = this.GetFileList(form.AcademicCertFiles);
            if (!academicFileList.Any())
                throw new InvalidOperationException("At least one academic file required.");

            if (!form.WhenCommited.HasValue)
                form.WhenCommited = DateTime.Now;

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
            fileList.Union(this.GetFileList(form.AcademicCertFiles));
            fileList.Union(this.GetFileList(form.DegreeCertFiles));
            fileList.Union(this.GetFileList(form.OtherFiles));
            if (!string.IsNullOrEmpty(form.HeadImageFile))
                fileList.Add(form.HeadImageFile);

            foreach(var fileId in fileList)
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
            form.FileReviewAccepted = null;
            form.WhenFileReview = null;
            form.Approved = null;
            form.WhenAudit = null;
            form.AuditMessage = null;
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

        /// <summary>
        /// 根据关键字、标记、获取满足条件的报名表。
        /// </summary>
        /// <param name="PlanID">指定的招聘计划。</param>
        /// <param name="AuditFilter">审核标记，null表示未审核，通过标记为true，false标记为未通过</param>
        /// <param name="AnounceFilter"></param>
        /// <param name="Keywords">关键字，若提供，可对姓名、身份证号码、移动电话号码、籍贯、生源地、学校、填写专业字段进行匹配搜索。否则查询全部。</param>
        /// <returns></returns>
        public IQueryable<ApplicationForm> GetCommitedEnrollmentData(int PlanID, AuditFilterType AuditFilter, AnnounceFilterType AnounceFilter, string Keywords)
        {
            //带分页
            //
            //先获得符合初始条件的集合
            var initSet = this.ApplicationForms.Commited().Where(e => e.JobId == PlanID);


            if (!string.IsNullOrEmpty(Keywords))
                initSet = initSet.Where(e =>
                    e.Person.DisplayName.StartsWith(Keywords) ||
                    e.Person.IDCardNumber.StartsWith(Keywords) ||
                    e.Person.Mobile.StartsWith(Keywords) ||
                    e.NativePlace.StartsWith(Keywords) ||
                    e.School.StartsWith(Keywords) ||
                    e.Major.StartsWith(Keywords)
                );

            switch (AuditFilter)
            {
                case AuditFilterType.All:
                    //DoNothing
                    break;
                case AuditFilterType.Approved:
                    initSet = initSet.Where(e => e.Approved.HasValue && e.Approved.Value);
                    break;
                case AuditFilterType.Rejective:
                    initSet = initSet.Where(e => e.Approved.HasValue && !e.Approved.Value);
                    break;
                case AuditFilterType.NotSet:
                    initSet = initSet.Where(e => !e.Approved.HasValue);
                    break;
            }

            switch (AnounceFilter)
            {
                case AnnounceFilterType.All:
                    //Do Nothing
                    break;
                case AnnounceFilterType.TakeExam:
                    initSet = initSet.Where(e => e.IsTakeExam.HasValue && e.IsTakeExam.Value);
                    break;
                case AnnounceFilterType.NotTakeExam:
                    initSet = initSet.Where(e => e.IsTakeExam.HasValue && !e.IsTakeExam.Value);
                    break;
                case AnnounceFilterType.NotAnnounced:
                    initSet = initSet.Where(e => !e.IsTakeExam.HasValue);
                    break;
            }

            return initSet;

        }

        /// <summary>
        /// 根据关键字、标记、排序指示获取满足条件的报名表。
        /// </summary>
        /// <param name="PlanID"></param>
        /// <param name=" AuditFilter"></param>
        /// <param name="AnnounceFilter"></param>
        /// <param name="Keywords"></param>
        /// <param name="OrderColumn"></param>
        /// <param name="DownDirection"></param>
        /// <param name="ItemCount"></param>
        /// <returns></returns>
		public IQueryable<ApplicationForm> GetCommitedEnrollmentData(int PlanID, AuditFilterType AuditFilter, AnnounceFilterType AnnounceFilter, string Keywords, string OrderColumn, bool DownDirection, out int ItemCount)
        {
            var resultSet = this.GetCommitedEnrollmentData(PlanID, AuditFilter, AnnounceFilter, Keywords);

            ItemCount = resultSet.Count();
            if (ItemCount == 0)
                return resultSet;

            //按字段排序
            if (string.IsNullOrEmpty(OrderColumn))
                OrderColumn = "WhenCommited";

            IOrderedQueryable<ApplicationForm> OrderedSet;
            if (DownDirection)
                OrderedSet = resultSet.OrderByDescending(OrderColumn);
            else
                OrderedSet = resultSet.OrderBy(OrderColumn);

            return OrderedSet;
        }

        /// <summary>
        /// 根据关键字、标记、排序指示和分页参数获取满足条件的指定页的报名表。
        /// </summary>
        /// <param name="PlanID"></param>
        /// <param name="AuditFilter"></param>
        /// <param name="AnnounceFilter"></param>
        /// <param name="Keywords"></param>
        /// <param name="OrderColumn"></param>
        /// <param name="DownDirection"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="ItemCount"></param>
        /// <returns></returns>
        public IQueryable<ApplicationForm> GetCommitedEnrollmentData(int PlanID, AuditFilterType AuditFilter, AnnounceFilterType AnnounceFilter, string Keywords, string OrderColumn, bool DownDirection, int PageIndex, int PageSize, out int ItemCount)
        {
            var result = this.GetCommitedEnrollmentData(PlanID, AuditFilter, AnnounceFilter, Keywords, OrderColumn, DownDirection, out ItemCount);
            if (ItemCount == 0)
            {
                return result;
            }

            //检查PageIndex和PageSize是否符合要求
            if (PageSize <= -1)
                PageSize = int.MaxValue;
            if (PageSize >= 0 && PageSize < 5)
                PageSize = 5;

            int PageCount = (int)Math.Ceiling((double)ItemCount / (double)PageSize);

            if (PageIndex < 0)
                PageIndex = 0;
            if (PageIndex >= PageCount)
                PageIndex = PageCount - 1;

            //返回指定分页的条目
            return result.Skip(PageIndex * PageSize).Take(PageSize);
        }

    }
}
