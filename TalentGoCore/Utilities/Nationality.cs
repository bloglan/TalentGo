using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TalentGo.Utilities
{
    
    /// <summary>
    /// 
    /// </summary>
    [Table("Nationality")]
    public partial class Nationality
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Name { get; set; }
    }
}
