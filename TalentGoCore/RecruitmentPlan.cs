namespace TalentGo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// ��ʾһ����Ƹ�ƻ���
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
        /// �ƻ�Id��
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// ��Ƹ�ƻ����ơ�
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// ��Ƹ����
        /// </summary>
        [Required]
        public string Recruitment { get; set; }


        /// <summary>
        /// ����ʱ�䡣
        /// </summary>
        public DateTime WhenCreated { get; protected set; }


        /// <summary>
        /// �����ˡ�
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Publisher { get; set; }

        /// <summary>
        /// ���ñ�����ֹʱ�䡣
        /// </summary>
        public DateTime EnrollExpirationDate { get; set; }

        /// <summary>
        /// ��ȡ����ύ��ʱ�䡣
        /// </summary>
        public DateTime? WhenAuditCommited { get; protected set; }

        /// <summary>
        /// �����������ԵĽ�ֹʱ�䡣
        /// </summary>
        public DateTime? AnnounceExpirationDate { get; set; }

        /// <summary>
        /// ��ȡ����ʱ�䡣
        /// </summary>
        public DateTime? WhenPublished { get; set; }

        /// <summary>
        /// ���ÿ��Կ�ʼʱ�䡣
        /// </summary>
        public DateTime? ExamStartTime { get; set; }

        /// <summary>
        /// ���ÿ��Խ���ʱ�䡣
        /// </summary>
        public DateTime? ExamEndTime { get; set; }

        /// <summary>
        /// ���ÿ��Եص㡣
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
            throw new InvalidOperationException("������Ч���ƻ�δ�������������ˡ�");
        }

        /// <summary>
        /// Get jobs of this plan.
        /// </summary>
        public virtual ICollection<Job> Jobs { get; protected set; }

        /// <summary>
        /// ֪ͨ���档
        /// </summary>
        public virtual ICollection<Notification> Notifications { get; protected set; }
    }
}
