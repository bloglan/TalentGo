namespace TalentGo.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EnrollmentData")]
    public partial class EnrollmentData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EnrollmentData()
        {
            EnrollmentArchives = new HashSet<EnrollmentArchives>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RecruitPlanID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        [Required]
        [StringLength(5)]
        public string Name { get; set; }

        [Required]
        [StringLength(2)]
        public string Sex { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(5)]
        public string Nationality { get; set; }

        [Required]
        [StringLength(50)]
        public string PlaceOfBirth { get; set; }

        [Required]
        [StringLength(100)]
        public string Source { get; set; }

        [Required]
        [StringLength(15)]
        public string PoliticalStatus { get; set; }

        [Required]
        [StringLength(10)]
        public string Health { get; set; }

        [Required]
        [StringLength(5)]
        public string Marriage { get; set; }

        [Required]
        [StringLength(18)]
        public string IDCardNumber { get; set; }

        [Required]
        [StringLength(15)]
        public string Mobile { get; set; }

        [Required]
        [StringLength(50)]
        public string School { get; set; }

        [Required]
        [StringLength(50)]
        public string Major { get; set; }

        public int YearOfGraduated { get; set; }

        [Required]
        [StringLength(15)]
        public string SelectedMajor { get; set; }

        [Required]
        [StringLength(15)]
        public string EducationBackground { get; set; }

        [Required]
        [StringLength(15)]
        public string Degree { get; set; }

        [Required]
        public string Resume { get; set; }

        public string Accomplishments { get; set; }

        public DateTime WhenCreated { get; set; }

        public DateTime? WhenChanged { get; set; }

        public DateTime? WhenCommited { get; set; }

        public DateTime? WhenAudit { get; set; }

        public bool? Approved { get; set; }

        [StringLength(50)]
        public string AuditMessage { get; set; }

        public DateTime? WhenAnnounced { get; set; }

        public bool? IsTakeExam { get; set; }

        public virtual Degree Degree1 { get; set; }

        public virtual EducationBackground EducationBackground1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnrollmentArchives> EnrollmentArchives { get; set; }

        public virtual MajorCategory MajorCategory { get; set; }

        public virtual RecruitmentPlan RecruitmentPlan { get; set; }

        public virtual ApplicationUser Users { get; set; }
    }
}
