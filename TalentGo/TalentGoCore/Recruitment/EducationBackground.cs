namespace TalentGo.Recruitment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EducationBackground")]
    public partial class EducationBackground
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EducationBackground()
        {
            EnrollmentData = new HashSet<Enrollment>();
        }

        [Key]
        [StringLength(15)]
        public string nid { get; set; }

        public DateTime WhenCreated { get; set; }

        public bool IsPublic { get; set; }

        public int PRI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Enrollment> EnrollmentData { get; set; }
    }
}
