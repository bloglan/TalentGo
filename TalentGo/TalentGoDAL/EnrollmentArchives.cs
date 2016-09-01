namespace TalentGo.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EnrollmentArchives
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

        public virtual EnrollmentData EnrollmentData { get; set; }
    }
}
