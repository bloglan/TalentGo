namespace TalentGo.EntityFramework
{
	using Identity;
	using Microsoft.AspNet.Identity;
	using Recruitment;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("Users")]
	public partial class ApplicationUser : TargetUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApplicationUser()
        {
        }
    }
}
