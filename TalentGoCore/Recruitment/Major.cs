namespace TalentGo.Recruitment
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Major")]
    public partial class Major
    {
        [Key]
        [StringLength(15)]
        public string nid { get; set; }

        [Required]
        [StringLength(15)]
        public string CategoryID { get; set; }

        public DateTime WhenCreated { get; set; }

        public int PRI { get; set; }

        public virtual MajorCategory MajorCategory { get; set; }
    }
}
