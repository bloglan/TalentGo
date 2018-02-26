namespace TalentGo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("ArchiveCategory")]
    public partial class ArchiveCategory
    {
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ArchiveCategory()
        {
            ArchiveRequirements = new HashSet<ArchiveRequirement>();
            EnrollmentArchives = new HashSet<EnrollmentArchive>();
        }

        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(150)]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(10)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime WhenCreated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime WhenChanged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(150)]
        public string Requirements { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column(TypeName = "image")]
        public byte[] SampleImage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        public string MimeType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? MinWidth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? MinHeight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? MaxWidth { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? MaxHeight { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? MinFileSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? MaxFileSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArchiveRequirement> ArchiveRequirements { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnrollmentArchive> EnrollmentArchives { get; set; }
    }
}
