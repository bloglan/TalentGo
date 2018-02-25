namespace TalentGo.Utilities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    /// <summary>
    /// 
    /// </summary>
    [Table("MobilePhoneValidationSession")]
    public partial class MobilePhoneValidationSession
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Mobile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(50)]
        public string UsedFor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(10)]
        public string ValidateCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime WhenCreated { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(150)]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastTryTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? ManualRequired { get; set; }
    }
}
