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
		public virtual DbSet<Person> Person { get; set; }

        /// <summary>
        /// fluent api
        /// </summary>
        /// <param name="modelBuilder"></param>
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
            modelBuilder.Entity<ApplicationForm>()
                .ToTable("ApplicationForm");

            modelBuilder.Entity<Person>()
                .ToTable("People");

            modelBuilder.Entity<WebUser>()
                .ToTable("WebUser");

            modelBuilder.Entity<RecruitmentPlan>()
                .ToTable("RecruitmentPlan");

            modelBuilder.Entity<Notification>()
                .ToTable("Notification");

            modelBuilder.Entity<Job>()
                .ToTable("Job");
		}
	}
}
