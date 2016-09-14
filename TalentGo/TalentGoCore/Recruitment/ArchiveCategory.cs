namespace TalentGo.Recruitment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ArchiveCategory")]
    public partial class ArchiveCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ArchiveCategory()
        {
            ArchiveRequirements = new HashSet<ArchiveRequirements>();
            EnrollmentArchives = new HashSet<EnrollmentArchives>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string Description { get; set; }

        [Required]
        [StringLength(10)]
        public string CreatedBy { get; set; }

        public DateTime WhenCreated { get; set; }

        public DateTime WhenChanged { get; set; }

        public bool Enabled { get; set; }

        [StringLength(150)]
        public string Requirements { get; set; }

        [Column(TypeName = "image")]
        public byte[] SampleImage { get; set; }

        [StringLength(50)]
        public string MimeType { get; set; }

        public int? MinWidth { get; set; }

        public int? MinHeight { get; set; }

        public int? MaxWidth { get; set; }

        public int? MaxHeight { get; set; }

        public int? MinFileSize { get; set; }

        public int? MaxFileSize { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArchiveRequirements> ArchiveRequirements { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnrollmentArchives> EnrollmentArchives { get; set; }
    }
}
