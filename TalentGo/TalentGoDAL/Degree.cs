namespace TalentGo.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Degree")]
    public partial class Degree
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Degree()
        {
            EnrollmentData = new HashSet<EnrollmentData>();
        }

        [Key]
        [StringLength(15)]
        public string nid { get; set; }

        public DateTime WhenCreated { get; set; }

        public int PRI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnrollmentData> EnrollmentData { get; set; }
    }
}
