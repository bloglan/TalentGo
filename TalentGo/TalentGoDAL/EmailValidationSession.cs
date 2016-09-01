namespace TalentGo.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmailValidationSession")]
    public partial class EmailValidationSession
    {
        public int id { get; set; }

        [Required]
        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string HashedCode { get; set; }

        public int UserID { get; set; }

        public DateTime WhenCreated { get; set; }

        public bool IsValid { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
