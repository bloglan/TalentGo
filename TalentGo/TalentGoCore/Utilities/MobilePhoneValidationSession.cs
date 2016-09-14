namespace TalentGo.Utilities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MobilePhoneValidationSession")]
    public partial class MobilePhoneValidationSession
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string Mobile { get; set; }

        [Required]
        [StringLength(50)]
        public string UsedFor { get; set; }

        [Required]
        [StringLength(10)]
        public string ValidateCode { get; set; }

        public DateTime WhenCreated { get; set; }

        public bool IsValid { get; set; }

        public DateTime ExpirationDate { get; set; }

        [StringLength(150)]
        public string Email { get; set; }

        public DateTime? LastTryTime { get; set; }

        public bool? ManualRequired { get; set; }
    }
}
