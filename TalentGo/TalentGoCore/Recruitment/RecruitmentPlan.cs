namespace TalentGo.Recruitment
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;
	using Utilities;

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

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Recruitment { get; set; }

        public int Year { get; set; }

        public DateTime WhenCreated { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsPublic { get; set; }

        [Required]
        [StringLength(20)]
        public string Publisher { get; set; }

        public DateTime? EnrollExpirationDate { get; set; }

        public DateTime? WhenAuditCommited { get; set; }

        public DateTime? AnnounceExpirationDate { get; set; }

        public DateTime? WhenPublished { get; set; }

        public DateTime? ExamStartTime { get; set; }

        public DateTime? ExamEndTime { get; set; }

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
