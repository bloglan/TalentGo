namespace TalentGo.Utilities
{
    using System.Data.Entity;
    using TalentGo.Recruitment;
    public partial class TalentGoDbContext : DbContext
	{
		public TalentGoDbContext()
			: base("name=DefaultConnection")
		{
		}

		public virtual DbSet<ArchiveCategory> ArchiveCategory { get; set; }
		public virtual DbSet<ArchiveRequirements> ArchiveRequirements { get; set; }
		public virtual DbSet<Article> Article { get; set; }
		public virtual DbSet<Degree> Degree { get; set; }
		public virtual DbSet<EducationBackground> EducationBackground { get; set; }
		//public virtual DbSet<EmailValidationSession> EmailValidationSession { get; set; }
		public virtual DbSet<EnrollmentArchives> EnrollmentArchives { get; set; }
		public virtual DbSet<EnrollmentData> EnrollmentData { get; set; }
		public virtual DbSet<Major> Major { get; set; }
		public virtual DbSet<MajorCategory> MajorCategory { get; set; }
		public virtual DbSet<MobilePhoneValidationSession> MobilePhoneValidationSession { get; set; }
		public virtual DbSet<Nationality> Nationality { get; set; }
		public virtual DbSet<RecruitmentPlan> RecruitmentPlan { get; set; }
		public virtual DbSet<UserLogins> UserLogins { get; set; }
		public virtual DbSet<TargetUser> Users { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ArchiveCategory>()
				.Property(e => e.MimeType)
				.IsUnicode(false);

			modelBuilder.Entity<ArchiveRequirements>()
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

			modelBuilder.Entity<EnrollmentArchives>()
				.Property(e => e.MimeType)
				.IsUnicode(false);

			modelBuilder.Entity<EnrollmentData>()
				.Property(e => e.IDCardNumber)
				.IsUnicode(false);

			modelBuilder.Entity<EnrollmentData>()
				.Property(e => e.Mobile)
				.IsUnicode(false);

			modelBuilder.Entity<EnrollmentData>()
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

			modelBuilder.Entity<RecruitmentPlan>()
				.HasMany(e => e.Article)
				.WithOptional(e => e.RecruitmentPlan)
				.HasForeignKey(e => e.RelatedPlan)
				.WillCascadeOnDelete();

			modelBuilder.Entity<RecruitmentPlan>()
				.HasMany(e => e.EnrollmentData)
				.WithRequired(e => e.RecruitmentPlan)
				.HasForeignKey(e => e.RecruitPlanID)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<TargetUser>()
				.Property(e => e.IDCardNumber)
				.IsUnicode(false);

			modelBuilder.Entity<TargetUser>()
				.Property(e => e.Mobile)
				.IsUnicode(false);

			modelBuilder.Entity<TargetUser>()
				.Property(e => e.HashPassword)
				.IsUnicode(false);

			modelBuilder.Entity<TargetUser>()
				.Property(e => e.RegisterationDelegate)
				.IsUnicode(false);

			modelBuilder.Entity<TargetUser>()
				.Property(e => e.DelegateInfo)
				.IsUnicode(false);

			modelBuilder.Entity<TargetUser>()
				.Property(e => e.UserName)
				.IsUnicode(false);

			modelBuilder.Entity<TargetUser>()
				.HasMany(e => e.EnrollmentData)
				.WithRequired(e => e.Users)
				.HasForeignKey(e => e.UserID)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<TargetUser>()
				.HasMany(e => e.UserLogins)
				.WithRequired(e => e.Users)
				.HasForeignKey(e => e.UserId);
		}
	}
}
