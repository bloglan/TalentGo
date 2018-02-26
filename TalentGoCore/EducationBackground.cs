namespace TalentGo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("EducationBackground")]
    public partial class EducationBackground
    {
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EducationBackground()
        {
            EnrollmentData = new HashSet<Enrollment>();
        }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [StringLength(15)]
        public string nid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime WhenCreated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PRI { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Enrollment> EnrollmentData { get; set; }
    }
}
