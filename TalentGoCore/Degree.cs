namespace TalentGo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("Degree")]
    public partial class Degree
    {
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Degree()
        {
            EnrollmentData = new HashSet<ApplicationForm>();
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
        public int PRI { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationForm> EnrollmentData { get; set; }
    }
}
