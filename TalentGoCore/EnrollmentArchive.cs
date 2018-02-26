namespace TalentGo
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("EnrollmentArchives")]
    public partial class EnrollmentArchive
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RecruitPlanID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ArchiveCategoryID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(50)]
        public string MimeType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column(TypeName = "image")]
        [Required]
        public byte[] ArchiveData { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime WhenCreated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? WhenChanged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ArchiveCategory ArchiveCategory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Enrollment EnrollmentData { get; set; }
    }
}
