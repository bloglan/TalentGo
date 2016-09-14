namespace TalentGo.Recruitment
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using Utilities;

    /// <summary>
    /// ��ʾһ����Ƹ�ƻ�
    /// </summary>
	[Table("RecruitmentPlan")]
    public partial class RecruitmentPlan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RecruitmentPlan()
        {
            ArchiveRequirements = new HashSet<ArchiveRequirements>();
            Article = new HashSet<Article>();
            EnrollmentData = new HashSet<EnrollmentData>();
        }

        /// <summary>
        /// �ƻ�Id
        /// </summary>
        public int id { get; set; }

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
        /// ���
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// ����ʱ�䡣
        /// </summary>
        public DateTime WhenCreated { get; set; }

        /// <summary>
        /// ����ʱ�䡣
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// �Ƿ񹫿���
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// �����ˡ�
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Publisher { get; set; }

        /// <summary>
        /// ���ñ�����ֹʱ�䡣
        /// </summary>
        public DateTime? EnrollExpirationDate { get; set; }

        /// <summary>
        /// ��ȡ����ύ��ʱ�䡣
        /// </summary>
        public DateTime? WhenAuditCommited { get; set; }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArchiveRequirements> ArchiveRequirements { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Article> Article { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnrollmentData> EnrollmentData { get; set; }
    }
}
