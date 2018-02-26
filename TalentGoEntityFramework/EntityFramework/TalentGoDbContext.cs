using System.Data.Entity;
using TalentGo.Identity;
using TalentGo.Utilities;
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
		public virtual DbSet<Degree> Degree { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public virtual DbSet<EducationBackground> EducationBackground { get; set; }
		//public virtual DbSet<EmailValidationSession> EmailValidationSession { get; set; }

            /// <summary>
            /// 
            /// </summary>
		public virtual DbSet<EnrollmentArchive> EnrollmentArchives { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public virtual DbSet<Enrollment> EnrollmentData { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public virtual DbSet<Major> Major { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public virtual DbSet<MajorCategory> MajorCategory { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public virtual DbSet<MobilePhoneValidationSession> MobilePhoneValidationSession { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public virtual DbSet<Nationality> Nationality { get; set; }

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

			modelBuilder.Entity<Degree>()
				.HasMany(e => e.EnrollmentData)
				.WithRequired(e => e.Degree1)
				.HasForeignKey(e => e.Degree)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<EducationBackground>()
				.HasMany(e => e.EnrollmentData)
				.WithRequired(e => e.EducationBackground1)
				.HasForeignKey(e => e.EducationBackground)
				.WillCascadeOnDelete(false);

			//modelBuilder.Entity<EmailValidationSession>()
			//	.Property(e => e.Email)
			//	.IsUnicode(false);

			//modelBuilder.Entity<EmailValidationSession>()
			//	.Property(e => e.HashedCode)
			//	.IsUnicode(false);

			modelBuilder.Entity<EnrollmentArchive>()
				.Property(e => e.MimeType)
				.IsUnicode(false);

			modelBuilder.Entity<Enrollment>()
				.Property(e => e.IDCardNumber)
				.IsUnicode(false);

			modelBuilder.Entity<Enrollment>()
				.Property(e => e.Mobile)
				.IsUnicode(false);

			modelBuilder.Entity<Enrollment>()
				.HasMany(e => e.EnrollmentArchives)
				.WithRequired(e => e.EnrollmentData)
				.HasForeignKey(e => new { e.RecruitPlanID, e.UserID })
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<MajorCategory>()
				.HasMany(e => e.EnrollmentData)
				.WithRequired(e => e.MajorCategory)
				.HasForeignKey(e => e.SelectedMajor)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<MajorCategory>()
				.HasMany(e => e.Major)
				.WithRequired(e => e.MajorCategory)
				.HasForeignKey(e => e.CategoryID);

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
