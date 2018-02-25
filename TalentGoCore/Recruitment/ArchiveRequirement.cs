namespace TalentGo.Recruitment
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ArchiveRequirements")]
    public partial class ArchiveRequirement
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RecruitmentPlanID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ArchiveCategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string Requirements { get; set; }

        public virtual ArchiveCategory ArchiveCategory { get; set; }

        public virtual RecruitmentPlan RecruitmentPlan { get; set; }
    }
}
