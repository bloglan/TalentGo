namespace TalentGo.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Article")]
    public partial class Article
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(150)]
        public string Summary { get; set; }

        [Required]
        public string MainContent { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime WhenCreated { get; set; }

        public DateTime WhenChanged { get; set; }

        public bool Visible { get; set; }

        public int? RelatedPlan { get; set; }

        public bool? IsPublic { get; set; }

        public virtual RecruitmentPlan RecruitmentPlan { get; set; }
    }
}
