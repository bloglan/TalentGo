using System;
using System.Linq;
using System.Threading.Tasks;
using TalentGo.Recruitment;

namespace TalentGo.Utilities
{
    public class EnrollmentStore : IEnrollmentStore, IEnrollmentArchiveStore
    {
        TalentGoDbContext dbContext;
        public EnrollmentStore(TalentGoDbContext DbContext)
        {
            this.dbContext = DbContext;
        }

        public IQueryable<EnrollmentArchive> EnrollmentArchives
        {
            get
            {
                return this.dbContext.EnrollmentArchives.AsNoTracking();
            }
        }

        public IQueryable<Enrollment> Enrollments
        {
            get
            {
                return this.dbContext.EnrollmentData.AsNoTracking();
            }
        }

        public async Task<Enrollment> FindByIdAsync(int PlanId, int UserId)
        {
            return this.dbContext.EnrollmentData.FirstOrDefault(e => e.RecruitPlanID == PlanId && e.UserID == UserId);
        }


        public async Task CreateAsync(Enrollment Enrollment)
        {
            Enrollment.WhenCreated = DateTime.Now;
            Enrollment.WhenChanged = DateTime.Now;

            this.dbContext.EnrollmentData.Add(Enrollment);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Enrollment Enrollment)
        {
            var current = this.dbContext.EnrollmentData.FirstOrDefault(e => e.RecruitPlanID == Enrollment.RecruitPlanID && e.UserID == Enrollment.UserID);
            if (current != null)
            {
                var entry = this.dbContext.Entry<Enrollment>(current);

                entry.CurrentValues.SetValues(Enrollment);
                entry.Property(p => p.WhenCreated).IsModified = false;

                current.WhenChanged = DateTime.Now;

                await this.dbContext.SaveChangesAsync();
            }
        }


        public async Task DeleteAsync(Enrollment Enrollment)
        {
            var current = await this.FindByIdAsync(Enrollment.RecruitPlanID, Enrollment.UserID);
            if (current != null)
            {
                this.dbContext.EnrollmentData.Remove(current);
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task<IQueryable<EnrollmentArchive>> GetEnrollmentArchives(Enrollment enrollment)
        {
            return this.dbContext.EnrollmentArchives.Where(e => e.RecruitPlanID == enrollment.RecruitPlanID && e.UserID == enrollment.UserID);
        }

        public async Task AddArchiveToEnrollment(Enrollment enrollment, EnrollmentArchive archive)
        {
            var currentEnrollment = await this.FindByIdAsync(enrollment.RecruitPlanID, enrollment.UserID);
            if (enrollment != null)
            {
                archive.RecruitPlanID = currentEnrollment.RecruitPlanID;
                archive.UserID = currentEnrollment.UserID;
                archive.WhenCreated = DateTime.Now;
                archive.WhenChanged = DateTime.Now;

                this.dbContext.EnrollmentArchives.Add(archive);
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveArchiveFromEnrollment(Enrollment enrollment, EnrollmentArchive archive)
        {
            var currentEnrollment = await this.FindByIdAsync(enrollment.RecruitPlanID, enrollment.UserID);
            if (enrollment != null)
            {
                var currentArch = this.dbContext.EnrollmentArchives.FirstOrDefault(
                    e => e.RecruitPlanID == currentEnrollment.RecruitPlanID &&
                    e.UserID == currentEnrollment.UserID &&
                    e.id == archive.id);
                if (currentArch != null)
                {
                    this.dbContext.EnrollmentArchives.Remove(currentArch);
                    await this.dbContext.SaveChangesAsync();
                }
            }
        }

    }
}
