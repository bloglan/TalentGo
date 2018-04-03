namespace TalentGo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// ��ʾһ����Ƹ�ƻ���
    /// </summary>
    public partial class RecruitmentPlan
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        protected RecruitmentPlan()
        {
            this.Jobs = new HashSet<Job>();
            this.Notifications = new HashSet<Notification>();
            this.WhenCreated = DateTime.Now;
        }

        /// <summary>
        /// Initialize recruitment with title, recruitment and enroll expiration time.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="recruitment"></param>
        /// <param name="enrollExpirationTime"></param>
        public RecruitmentPlan(string title, string recruitment, DateTime enrollExpirationTime)
            : this()
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("Title is null or empty");
            if (string.IsNullOrEmpty(recruitment))
                throw new ArgumentException("Recruitment is null or empty.");

            this.Title = title;
            this.Recruitment = recruitment;
            this.EnrollExpirationDate = enrollExpirationTime;
        }

        /// <summary>
        /// �ƻ�Id��
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// ��Ƹ�ƻ����ơ�
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// ��Ƹ����
        /// </summary>
        public string Recruitment { get; set; }


        /// <summary>
        /// ����ʱ�䡣
        /// </summary>
        public DateTime WhenCreated { get; protected set; }


        /// <summary>
        /// ���ñ�����ֹʱ�䡣
        /// </summary>
        public DateTime EnrollExpirationDate { get; set; }

        /// <summary>
        /// ��ȡ����ύ��ʱ�䡣
        /// </summary>
        public DateTime? WhenAuditCommited { get; protected set; }

        /// <summary>
        /// ��ȡ����ʱ�䡣
        /// </summary>
        public DateTime? WhenPublished { get; set; }


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
