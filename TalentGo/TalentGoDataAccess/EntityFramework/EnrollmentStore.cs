using System;
using System.Linq;
using System.Threading.Tasks;
using TalentGo.Recruitment;

namespace TalentGo.EntityFramework
{
    public class EnrollmentStore : IEnrollmentStore, IEnrollmentArchiveStore
    {
        TalentGoDbContext dbContext;
        public EnrollmentStore(TalentGoDbContext DbContext)
        {
            this.dbContext = DbContext;
        }

        public IQueryable<EnrollmentArchives> EnrollmentArchives
        {
            get
            {
                return this.dbContext.EnrollmentArchives.AsNoTracking();
            }
        }

        public IQueryable<EnrollmentData> Enrollments
        {
            get
            {
                return this.dbContext.EnrollmentData.AsNoTracking();
            }
        }

        public async Task<EnrollmentData> FindByIdAsync(int PlanId, int UserId)
        {
            return this.dbContext.EnrollmentData.FirstOrDefault(e => e.RecruitPlanID == PlanId && e.UserID == UserId);
        }


        public async Task CreateAsync(EnrollmentData Enrollment)
        {
            Enrollment.WhenCreated = DateTime.Now;
            Enrollment.WhenChanged = DateTime.Now;

            this.dbContext.EnrollmentData.Add(Enrollment);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(EnrollmentData Enrollment)
        {
            var current = this.dbContext.EnrollmentData.FirstOrDefault(e => e.RecruitPlanID == Enrollment.RecruitPlanID && e.UserID == Enrollment.UserID);
            if (current != null)
            {
                var entry = this.dbContext.Entry<EnrollmentData>(current);

                entry.CurrentValues.SetValues(Enrollment);
                entry.Property(p => p.WhenCreated).IsModified = false;

                current.WhenChanged = DateTime.Now;

                await this.dbContext.SaveChangesAsync();
            }
        }


        public async Task DeleteAsync(EnrollmentData Enrollment)
        {
            var current = await this.FindByIdAsync(Enrollment.RecruitPlanID, Enrollment.UserID);
            if (current != null)
            {
                this.dbContext.EnrollmentData.Remove(current);
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task<IQueryable<EnrollmentArchives>> GetEnrollmentArchives(EnrollmentData enrollment)
        {
            return this.dbContext.EnrollmentArchives.Where(e => e.RecruitPlanID == enrollment.RecruitPlanID && e.UserID == enrollment.UserID);
        }

        public async Task AddArchiveToEnrollment(EnrollmentData enrollment, EnrollmentArchives archive)
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

        public async Task RemoveArchiveFromEnrollment(EnrollmentData enrollment, EnrollmentArchives archive)
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
