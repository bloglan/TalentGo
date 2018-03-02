namespace TalentGo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 表示一个招聘计划。
    /// </summary>
	[Table("RecruitmentPlan")]
    public partial class RecruitmentPlan
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public RecruitmentPlan()
        {
            this.Jobs = new HashSet<Job>();
            this.Notifications = new HashSet<Notification>();
            this.WhenCreated = DateTime.Now;
        }

        /// <summary>
        /// 计划Id。
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// 招聘计划名称。
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// 招聘简章
        /// </summary>
        [Required]
        public string Recruitment { get; set; }


        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime WhenCreated { get; protected set; }


        /// <summary>
        /// 发布人。
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Publisher { get; set; }

        /// <summary>
        /// 设置报名截止时间。
        /// </summary>
        public DateTime EnrollExpirationDate { get; set; }

        /// <summary>
        /// 获取审核提交的时间。
        /// </summary>
        public DateTime? WhenAuditCommited { get; protected set; }

        /// <summary>
        /// 设置声明考试的截止时间。
        /// </summary>
        public DateTime? AnnounceExpirationDate { get; set; }

        /// <summary>
        /// 获取发布时间。
        /// </summary>
        public DateTime? WhenPublished { get; set; }

        /// <summary>
        /// 设置考试开始时间。
        /// </summary>
        public DateTime? ExamStartTime { get; set; }

        /// <summary>
        /// 设置考试结束时间。
        /// </summary>
        public DateTime? ExamEndTime { get; set; }

        /// <summary>
        /// 设置考试地点。
        /// </summary>
        [StringLength(100)]
        public string ExamLocation { get; set; }


        internal void CompleteAudit()
        {
            if (this.WhenPublished.HasValue)
            {
                if (!this.WhenAuditCommited.HasValue)
                {
                    this.WhenAuditCommited = DateTime.Now;
                    return;
                }
            }
            throw new InvalidOperationException("操作无效，计划未发布或已完成审核。");
        }

        /// <summary>
        /// Get jobs of this plan.
        /// </summary>
        public virtual ICollection<Job> Jobs { get; protected set; }

        /// <summary>
        /// 通知公告。
        /// </summary>
        public virtual ICollection<Notification> Notifications { get; protected set; }
    }
}
