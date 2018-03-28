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

            modelBuilder.Entity<File>()
                .ToTable("File");

            modelBuilder.Entity<Notice>()
                .ToTable("Notice");

            modelBuilder.Entity<ExaminationPlan>()
                .ToTable("Examination");

            modelBuilder.Entity<ExaminationSubject>()
                .ToTable("ExaminationSubject");
		}
	}
}
