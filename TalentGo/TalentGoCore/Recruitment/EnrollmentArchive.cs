namespace TalentGo.Recruitment
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EnrollmentArchives")]
    public partial class EnrollmentArchive
    {
        public int id { get; set; }

        public int RecruitPlanID { get; set; }

        public int UserID { get; set; }

        public int ArchiveCategoryID { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        public string MimeType { get; set; }

        [Column(TypeName = "image")]
        [Required]
        public byte[] ArchiveData { get; set; }

        public DateTime WhenCreated { get; set; }

        public DateTime? WhenChanged { get; set; }

        public virtual ArchiveCategory ArchiveCategory { get; set; }

        public virtual Enrollment EnrollmentData { get; set; }
    }
}
