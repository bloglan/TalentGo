namespace TalentGo.Recruitment
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("Major")]
    public partial class Major
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [StringLength(15)]
        public string nid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(15)]
        public string CategoryID { get; set; }

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
        public virtual MajorCategory MajorCategory { get; set; }
    }
}
