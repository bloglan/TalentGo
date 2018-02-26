namespace TalentGo
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("ArchiveRequirements")]
    public partial class ArchiveRequirement
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RecruitmentPlanID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ArchiveCategoryID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Requirements { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ArchiveCategory ArchiveCategory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual RecruitmentPlan RecruitmentPlan { get; set; }
    }
}
