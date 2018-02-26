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
	[Table("EnrollmentData")]
	public class Enrollment
	{
        /// <summary>
        /// Default ctor.
        /// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		protected Enrollment()
		{
			EnrollmentArchives = new HashSet<EnrollmentArchive>();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="user"></param>
        public Enrollment(RecruitmentPlan plan, Person user)
            : this()
        {
            this.RecruitmentPlan = plan;
            this.RecruitPlanID = plan.id;
            this.UserID = user.Id;
            this.User = user;
        }

        /// <summary>
        /// related to recruitment plan by it's plan.
        /// </summary>
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int RecruitPlanID { get; protected set; }

        /// <summary>
        /// related to a target user by its id.
        /// </summary>
		[Key]
		[Column(Order = 1)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int UserID { get; protected set; }

        /// <summary>
        /// user's full name.
        /// </summary>
		[Display(Name = "姓名")]
		[Required]
		[StringLength(5, MinimumLength = 2)]
		public string Name { get; set; }

        /// <summary>
        /// Sex of user.
        /// </summary>
		[Display(Name = "性别")]
		[Required]
		[StringLength(2)]
		public string Sex { get; set; }

        /// <summary>
        /// Birth date.
        /// </summary>
		[Display(Name = "出生日期")]
		[DataType(DataType.Date)]
		[Column(TypeName = "date")]
		public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Nationality.
        /// </summary>
		[Display(Name = "民族")]
		[Required]
		[StringLength(5)]
		public string Nationality { get; set; }

        /// <summary>
        /// Place of birth.
        /// </summary>
		[Display(Name = "籍贯")]
		[Required]
		[StringLength(50)]
		public string PlaceOfBirth { get; set; }

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
        /// Chinese ID card number.
        /// </summary>
		[Display(Name = "身份证号码")]
		[Required]
		[StringLength(18)]
		public string IDCardNumber { get; set; }

        /// <summary>
        /// Mobile phone number.
        /// </summary>
		[Display(Name = "手机号码")]
		[Required]
		[StringLength(15)]
		public string Mobile { get; set; }

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
        /// Major of enrollment.
        /// </summary>
		[Display(Name = "报考专业")]
		[Required]
		[StringLength(15)]
		public string SelectedMajor { get; set; }

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
		public DateTime WhenCreated { get; set; }

        /// <summary>
        /// When changed.
        /// </summary>
		[Display(Name = "修改时间")]
		public DateTime? WhenChanged { get; set; }

        /// <summary>
        /// When enrollment commited. if value of null, means uncommited.
        /// </summary>
		[Display(Name = "提交时间")]
		public DateTime? WhenCommited { get; protected set; }

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
		public DateTime? WhenAnnounced { get; protected set; }

        /// <summary>
        /// A value indicate wheather user determined to take exam or not.
        /// </summary>
		[Display(Name = "参加考试")]
		public bool? IsTakeExam { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
		public virtual Degree Degree1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public virtual EducationBackground EducationBackground1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<EnrollmentArchive> EnrollmentArchives { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public virtual MajorCategory MajorCategory { get; set; }

        /// <summary>
        /// Gets recruitment plan of this enrollment.
        /// </summary>
        [ForeignKey(nameof(RecruitPlanID))]
		public virtual RecruitmentPlan RecruitmentPlan { get; protected set; }

        /// <summary>
        /// Gets target user of this enrollment.
        /// </summary>
        [ForeignKey("UserID")]
		public virtual Person User { get; protected set; }

        /// <summary>
        /// Gets a bool value indicate wheather this enrollment has commited or not.
        /// </summary>
        [NotMapped]
        public bool HasCommited
        {
            get { return this.WhenCommited.HasValue; }
        }

        /// <summary>
        /// Commit this enrollment.
        /// </summary>
        internal virtual void Commit()
        {
            if (this.HasCommited)
                throw new InvalidOperationException("已提交的报名表不能重复提交");

            //为已提交文档顺次检查需求性是否满足？
            foreach(var requirement in this.RecruitmentPlan.ArchiveRequirements)
            {
                RequirementType reqType = (RequirementType)Enum.Parse(typeof(RequirementType), requirement.Requirements);
                if (reqType.IsRequried())
                    if (!this.EnrollmentArchives.Any(ea => ea.ArchiveCategoryID == requirement.ArchiveCategoryID))
                        throw new InvalidOperationException("无法提交，需求未满足。");
                if (!reqType.IsMultipleEnabled())
                    if (this.EnrollmentArchives.Count(ea => ea.ArchiveCategoryID == requirement.ArchiveCategoryID) > 1)
                        throw new InvalidOperationException("无法提交，需求未满足。");

            }
            
            //TODO:其他需要执行的检查。

            this.WhenCommited = DateTime.Now;
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
        /// 
        /// </summary>
        /// <param name="IsTakeExam"></param>
        public void Announce(bool IsTakeExam)
        {
            if (!this.WhenAudit.HasValue)
                throw new InvalidOperationException("未完成审核的不能进行声明。");

            if (!this.Approved.HasValue || !this.Approved.Value)
                throw new InvalidOperationException("未审核通过的不能进行声明。");

            this.IsTakeExam = IsTakeExam;
            this.WhenAnnounced = DateTime.Now;
        }
    }
}
