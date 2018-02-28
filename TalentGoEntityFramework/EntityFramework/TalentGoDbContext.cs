using System.Data.Entity;
using TalentGo.Identity;
using TalentGo.Web;

namespace TalentGo.EntityFramework
{
    /// <summary>
    /// A DbContext definitions for TalentGo database.
    /// </summary>
    public partial class TalentGoDbContext : DbContext
	{
        /// <summary>
        /// Default ctor.
        /// </summary>
		public TalentGoDbContext()
			: base("name=DefaultConnection")
		{
		}

        /// <summary>
        /// 
        /// </summary>
		public virtual DbSet<ArchiveCategory> ArchiveCategory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DbSet<ArchiveRequirement> ArchiveRequirements { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public virtual DbSet<Article> Article { get; set; }


            /// <summary>
            /// 
            /// </summary>
		public virtual DbSet<EnrollmentArchive> EnrollmentArchives { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public virtual DbSet<ApplicationForm> EnrollmentData { get; set; }


        /// <summary>
        /// 
        /// </summary>
		public virtual DbSet<MobilePhoneValidationSession> MobilePhoneValidationSession { get; set; }


        /// <summary>
        /// 
        /// </summary>
		public virtual DbSet<RecruitmentPlan> RecruitmentPlan { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
		public virtual DbSet<UserLogin> UserLogins { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public virtual DbSet<Person> Users { get; set; }

        /// <summary>
        /// fluent api
        /// </summary>
        /// <param name="modelBuilder"></param>
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ArchiveCategory>()
				.Property(e => e.MimeType)
				.IsUnicode(false);

			modelBuilder.Entity<ArchiveRequirement>()
				.Property(e => e.Requirements)
				.IsUnicode(false);



			modelBuilder.Entity<EnrollmentArchive>()
				.Property(e => e.MimeType)
				.IsUnicode(false);

			modelBuilder.Entity<ApplicationForm>()
				.Property(e => e.IDCardNumber)
				.IsUnicode(false);

			modelBuilder.Entity<ApplicationForm>()
				.Property(e => e.Mobile)
				.IsUnicode(false);




			modelBuilder.Entity<MobilePhoneValidationSession>()
				.Property(e => e.Mobile)
				.IsUnicode(false);

			modelBuilder.Entity<MobilePhoneValidationSession>()
				.Property(e => e.UsedFor)
				.IsUnicode(false);

			modelBuilder.Entity<MobilePhoneValidationSession>()
				.Property(e => e.Email)
				.IsUnicode(false);

			//modelBuilder.Entity<RecruitmentPlan>()
			//	.HasMany(e => e.Article)
			//	.WithOptional(e => e.RecruitmentPlan)
			//	.HasForeignKey(e => e.RelatedPlan)
			//	.WillCascadeOnDelete();

			//modelBuilder.Entity<RecruitmentPlan>()
			//	.HasMany(e => e.EnrollmentData)
			//	.WithRequired(e => e.RecruitmentPlan)
			//	.HasForeignKey(e => e.RecruitPlanID)
			//	.WillCascadeOnDelete(false);

			modelBuilder.Entity<Person>()
				.Property(e => e.IDCardNumber)
				.IsUnicode(false);

			modelBuilder.Entity<Person>()
				.Property(e => e.Mobile)
				.IsUnicode(false);

			modelBuilder.Entity<WebUser>()
				.Property(e => e.HashPassword)
				.IsUnicode(false);

			modelBuilder.Entity<WebUser>()
				.Property(e => e.UserName)
				.IsUnicode(false);

            //modelBuilder.Entity<TargetUser>()
            //    .HasMany(e => e.EnrollmentData)
            //    .WithRequired(e => e.User)
            //    .HasForeignKey(e => e.UserID)
            //    .WillCascadeOnDelete(false);

		}
	}
}
