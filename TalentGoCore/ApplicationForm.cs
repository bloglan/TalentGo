using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;

namespace TalentGo
{
    /// <summary>
    /// 表示一个报名表。
    /// </summary>
	public class ApplicationForm
    {
        /// <summary>
        /// Default ctor.
        /// </summary>
		protected ApplicationForm()
        {
            this.WhenCreated = DateTime.Now;
            this.WhenChanged = DateTime.Now;
            this.ChangeLog = string.Empty;
            this.AcademicCertFiles = string.Empty;
            this.DegreeCertFiles = string.Empty;
            this.OtherFiles = string.Empty;
        }

        /// <summary>
        /// 根据指定工作和用户初始化报名表。
        /// </summary>
        /// <param name="job"></param>
        /// <param name="person"></param>
        public ApplicationForm(Job job, Person person)
            : this()
        {
            this.Job = job;
            this.JobId = job.Id;
            this.PersonId = person.Id;
            this.Person = person;
        }

        /// <summary>
        /// 报名表Id.
        /// </summary>
        [Key]
        public int Id { get; protected set; }

        /// <summary>
        /// related to recruitment plan by it's plan.
        /// </summary>
        public int JobId { get; protected set; }

        /// <summary>
        /// PlanId
        /// </summary>
        public int PlanId { get; protected set; }

        /// <summary>
        /// Gets recruitment plan of this enrollment.
        /// </summary>
        [ForeignKey(nameof(JobId) + "," + nameof(PlanId))]
        public virtual Job Job { get; protected set; }


        /// <summary>
        /// related to a target user by its id.
        /// </summary>
        [ForeignKey(nameof(Person))]
        public Guid PersonId { get; protected set; }

        /// <summary>
        /// Gets target user of this enrollment.
        /// </summary>
        public virtual Person Person { get; protected set; }


        /// <summary>
        /// Place of birth.
        /// </summary>
		public string NativePlace { get; set; }

        /// <summary>
        /// Current home address.
        /// </summary>
		public string Source { get; set; }

        /// <summary>
        /// Political of user.
        /// </summary>
		public string PoliticalStatus { get; set; }

        /// <summary>
        /// Health status of user.
        /// </summary>
		public string Health { get; set; }

        /// <summary>
        /// Marital status of user.
        /// </summary>
		public string Marriage { get; set; }

        /// <summary>
        /// Last graduated school.
        /// </summary>
		public string School { get; set; }

        /// <summary>
        /// 主修专业
        /// </summary>
		public string Major { get; set; }

        /// <summary>
        /// 报名专业。
        /// </summary>
        public string SelectedMajor { get; set; }

        /// <summary>
        /// Year of graduated.
        /// </summary>
		[Display(Name = "毕业年度")]
        public int YearOfGraduated { get; set; }


        /// <summary>
        /// Educational background.
        /// </summary>
		public string EducationalBackground { get; set; }

        /// <summary>
        /// 学历证书编号。
        /// </summary>
        public string AcademicCertNumber { get; set; }

        /// <summary>
        /// Educational Degree.
        /// </summary>
        public string Degree { get; set; }

        /// <summary>
        /// 学位证书编号。
        /// </summary>
        public string DegreeCertNumber { get; set; }
        /// <summary>
        /// Resume.
        /// </summary>
        public string Resume { get; set; }

        /// <summary>
        /// Accomplishments.
        /// </summary>
		public string Accomplishments { get; set; }

        /// <summary>
        /// 证件照。
        /// </summary>
        public string HeadImageFile { get; internal set; }

        /// <summary>
        /// 学历证书文件。
        /// </summary>
        public string AcademicCertFiles { get; internal set; }

        FileIdList academicCertFileList;

        /// <summary>
        /// 学历证书文件列表。
        /// </summary>
        [NotMapped]
        public FileIdList AcademicCertFileList
        {
            get
            {
                if (academicCertFileList == null)
                {
                    this.academicCertFileList = new FileIdList(this.AcademicCertFiles, (s) => this.AcademicCertFiles = s);
                }
                return this.academicCertFileList;
            }
        }

        /// <summary>
        /// 学位证书文件。
        /// </summary>
        public string DegreeCertFiles { get; internal set; }

        FileIdList degreeCertFileList;

        /// <summary>
        /// 获取学位证书列表。
        /// </summary>
        [NotMapped]
        public FileIdList DegreeCertFileList
        {
            get
            {
                if (this.degreeCertFileList == null)
                    this.degreeCertFileList = new FileIdList(this.DegreeCertFiles, (s) => this.DegreeCertFiles = s);
                return this.degreeCertFileList;
            }
        }

        /// <summary>
        /// 其他材料。
        /// </summary>
        public string OtherFiles { get; internal set; }

        FileIdList otherFileList;

        /// <summary>
        /// 获取其他文件列表。
        /// </summary>
        [NotMapped]
        public FileIdList OtherFileList
        {
            get
            {
                if (this.otherFileList == null)
                {
                    this.otherFileList = new FileIdList(this.OtherFiles, (s) => this.OtherFiles = s);
                }
                return this.otherFileList;
            }
        }

        /// <summary>
        /// When created.
        /// </summary>
		public DateTime WhenCreated { get; protected set; }

        /// <summary>
        /// When changed.
        /// </summary>
		public DateTime? WhenChanged { get; internal set; }

        /// <summary>
        /// When enrollment commited. if value of null, means uncommited.
        /// </summary>
		public DateTime? WhenCommited { get; internal set; }

        /// <summary>
        /// 提交计数器。
        /// </summary>
        public virtual int CommitCount { get; internal set; }

        /// <summary>
        /// 资料审查是否通过。
        /// </summary>
        public bool? FileReviewAccepted { get; internal set; }

        /// <summary>
        /// 资料审查消息。
        /// </summary>
        public virtual string FileReviewMessage { get; internal set; }

        /// <summary>
        /// 资料审查人。
        /// </summary>
        public string FileReviewedBy { get; internal set; }

        /// <summary>
        /// 资料审查时间。
        /// </summary>
        public DateTime? WhenFileReviewed { get; internal set; }

        /// <summary>
        /// A value indicate wheather enrollment accepted or refused.
        /// </summary>
		public bool AuditFlag { get; internal set; }

        /// <summary>
        /// Message of audit.
        /// </summary>
		[StringLength(50)]
        public string AuditMessage { get; internal set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditBy { get; internal set; }

        /// <summary>
        /// 设置审核标记的时间。
        /// </summary>
        public DateTime? WhenAudit { get; internal set; }

        /// <summary>
        /// 完成审核时间。
        /// </summary>
        public DateTime? WhenAuditComplete { get; internal set; }

        /// <summary>
        /// 标记
        /// </summary>
        public string Tags { get; internal set; }

        /// <summary>
        /// When user announced take exam. if value of null, means not announced yet.
        /// </summary>
		public DateTime? WhenAnnounced { get; internal set; }

        /// <summary>
        /// A value indicate wheather user determined to take exam or not.
        /// </summary>
		public bool? IsTakeExam { get; internal set; }

        /// <summary>
        /// 更改日志。
        /// </summary>
        public string ChangeLog { get; protected set; }

        /// <summary>
        /// Log message with current datetime.
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            this.Log(DateTime.Now, message);
        }

        /// <summary>
        /// Log message with message.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="message"></param>
        public void Log(DateTime dateTime, string message)
        {
            this.ChangeLog += string.Format("[{0}]{1}\r\n", dateTime, message);
        }

        /// <summary>
        /// log message from format and arguments with datetime.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Log(DateTime dateTime, string format, params object[] args)
        {
            var message = string.Format(format, args);
            this.Log(dateTime, message);
        }
    }
}
