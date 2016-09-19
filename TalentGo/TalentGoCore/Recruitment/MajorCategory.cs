namespace TalentGo.Recruitment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MajorCategory")]
    public partial class MajorCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MajorCategory()
        {
            EnrollmentData = new HashSet<Enrollment>();
            Major = new HashSet<Major>();
        }

        [Key]
        [StringLength(15)]
        public string nid { get; set; }

        public DateTime WhenCreated { get; set; }

        public int PRI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Enrollment> EnrollmentData { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Major> Major { get; set; }
    }
}