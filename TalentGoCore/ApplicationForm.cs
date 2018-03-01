using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
		}

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        [Key]
        public int Id { get; protected set; }

        /// <summary>
        /// related to recruitment plan by it's plan.
        /// </summary>
        [ForeignKey(nameof(Job))]
        public int JobId { get; protected set; }

        /// <summary>
        /// Gets recruitment plan of this enrollment.
        /// </summary>
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
        /// user's full name.
        /// </summary>
		[Display(Name = "姓名")]
		[Required]
		[StringLength(5, MinimumLength = 2)]
		public string Name { get; set; }

        /// <summary>
        /// Place of birth.
        /// </summary>
		[Display(Name = "籍贯")]
		[Required]
		[StringLength(50)]
		public string NativePlace { get; set; }

        /// <summary>
        /// Current home address.
        /// </summary>
		[Display(Name = "家庭现居地")]
		[Required]
		[StringLength(100)]
		public string Source { get; set; }

        /// <summary>
        /// Political of user.
        /// </summary>
		[Display(Name = "政治面貌")]
		[Required]
		[StringLength(15)]
		public string PoliticalStatus { get; set; }

        /// <summary>
        /// Health status of user.
        /// </summary>
		[Display(Name = "健康状况")]
		[Required]
		[StringLength(10)]
		public string Health { get; set; }

        /// <summary>
        /// Marital status of user.
        /// </summary>
		[Display(Name = "婚姻状况")]
		[Required]
		[StringLength(5)]
		public string Marriage { get; set; }

        /// <summary>
        /// Last graduated school.
        /// </summary>
		[Display(Name = "毕业院校")]
		[Required]
		[StringLength(50)]
		public string School { get; set; }

        /// <summary>
        /// Major of education.
        /// </summary>
		[Display(Name = "毕业专业")]
		[Required]
		[StringLength(50)]
		public string Major { get; set; }

        /// <summary>
        /// Year of graduated.
        /// </summary>
		[Display(Name = "毕业年度")]
		public int YearOfGraduated { get; set; }


        /// <summary>
        /// Educational background.
        /// </summary>
		[Display(Name = "学历")]
		[Required]
		[StringLength(15)]
		public string EducationBackground { get; set; }

        /// <summary>
        /// Educational Degree.
        /// </summary>
		[Display(Name = "学位")]
		[Required]
		[StringLength(15)]
		public string Degree { get; set; }

        /// <summary>
        /// Resume.
        /// </summary>
		[Display(Name = "简历")]
		[Required]
		[StringLength(200)]
		[DataType(DataType.MultilineText)]
		public string Resume { get; set; }

        /// <summary>
        /// Accomplishments.
        /// </summary>
		[Display(Name = "特长及自我介绍")]
		[StringLength(1000)]
		[DataType(DataType.MultilineText)]
		public string Accomplishments { get; set; }

        /// <summary>
        /// When created.
        /// </summary>
		[Display(Name = "创建时间")]
		public DateTime WhenCreated { get; protected set; }

        /// <summary>
        /// When changed.
        /// </summary>
		[Display(Name = "修改时间")]
		public DateTime? WhenChanged { get; internal set; }

        /// <summary>
        /// When enrollment commited. if value of null, means uncommited.
        /// </summary>
		[Display(Name = "提交时间")]
		public DateTime? WhenCommited { get; internal set; }

        /// <summary>
        /// 资料审查是否通过。
        /// </summary>
        public bool? FileReviewAccepted { get; internal set; }

        /// <summary>
        /// 资料审查时间。
        /// </summary>
        public DateTime? WhenFileReview { get; internal set; }

        /// <summary>
        /// When enrollment pass the audit. if value of null, means not audit yet.
        /// </summary>
		[Display(Name = "审核时间")]
		public DateTime? WhenAudit { get; protected set; }

        /// <summary>
        /// A value indicate wheather enrollment accepted or refused.
        /// </summary>
		[Display(Name = "已批准")]
		public bool? Approved { get; protected set; }

        /// <summary>
        /// Message of audit.
        /// </summary>
		[Display(Name = "审核消息")]
		[StringLength(50)]
		public string AuditMessage { get; protected set; }

        /// <summary>
        /// When user announced take exam. if value of null, means not announced yet.
        /// </summary>
		[Display(Name = "声明时间")]
		public DateTime? WhenAnnounced { get; internal set; }

        /// <summary>
        /// A value indicate wheather user determined to take exam or not.
        /// </summary>
		[Display(Name = "参加考试")]
		public bool? IsTakeExam { get; internal set; }

        /// <summary>
        /// 更改日志。
        /// </summary>
        public string ChangeLog { get; protected set; }


        /// <summary>
        /// Gets a bool value indicate wheather this enrollment has commited or not.
        /// </summary>
        [NotMapped]
        public bool HasCommited
        {
            get { return this.WhenCommited.HasValue; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsAccept"></param>
        protected virtual void SetAudit(bool IsAccept)
        {
            if (!this.HasCommited)
                throw new InvalidOperationException("未提交的报名不能设置审核标记。");

            if (this.WhenAudit.HasValue)
                throw new InvalidOperationException("已完成审核后不能再进行设置。");
            this.Approved = IsAccept;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Accept()
        {
            this.SetAudit(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Refuse()
        {
            this.SetAudit(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnsetAudit()
        {
            if (!this.WhenCommited.HasValue)
                throw new InvalidOperationException("未提交的报名不能设置审核标记。");

            if (this.WhenAudit.HasValue)
                throw new InvalidOperationException("已完成审核后不能再进行设置。");
            this.Approved = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Message"></param>
        public void SetAuditMessage(string Message)
        {
            if (!this.WhenCommited.HasValue)
                throw new InvalidOperationException("未提交的报名不能设置审核标记。");

            if (this.WhenAudit.HasValue)
                throw new InvalidOperationException("已完成审核后不能再进行设置。");

            this.AuditMessage = Message;
        }

        internal void CompleteAudit()
        {
            if (!this.Approved.HasValue)
                throw new InvalidOperationException("未设置审核标记的不能完成审核。");

            if (this.WhenAudit.HasValue)
                throw new InvalidOperationException("已完成审核的不能再次完成审核。");

            this.WhenAudit = DateTime.Now;
        }

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
