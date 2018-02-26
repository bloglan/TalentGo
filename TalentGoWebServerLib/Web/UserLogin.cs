namespace TalentGo.Web
{
    using Recruitment;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 
    /// </summary>
    [Table("UserLogins")]
    public partial class UserLogin
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public string LoginProvider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Column(Order = 2)]
        public string ProviderKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Person Users { get; set; }
    }
}
