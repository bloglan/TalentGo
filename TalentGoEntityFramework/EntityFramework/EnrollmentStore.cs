using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class EnrollmentStore : IEnrollmentStore, IEnrollmentArchiveStore
    {
        DbContext dbContext;
        DbSet<Enrollment> set;
        DbSet<EnrollmentArchive> archiveSet;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DbContext"></param>
        public EnrollmentStore(DbContext DbContext)
        {
            this.dbContext = DbContext;
            this.set = this.dbContext.Set<Enrollment>();
            this.archiveSet = this.dbContext.Set<EnrollmentArchive>();
        }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<EnrollmentArchive> EnrollmentArchives
        {
            get
            {
                return this.archiveSet;
            }
        }

        /// <summary>
        /// Gets a queryable collections for enrollment without change tracking.
        /// </summary>
        public IQueryable<Enrollment> Enrollments
        {
            get
            {
                return this.set;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PlanId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<Enrollment> FindByIdAsync(int PlanId, int UserId)
        {
            return this.set.FirstOrDefault(e => e.RecruitPlanID == PlanId && e.UserID == UserId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Enrollment"></param>
        /// <returns></returns>
        public async Task CreateAsync(Enrollment Enrollment)
        {
            Enrollment.WhenCreated = DateTime.Now;
            Enrollment.WhenChanged = DateTime.Now;

            this.set.Add(Enrollment);
            await this.dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollment"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Enrollment enrollment)
        {
            this.dbContext.Entry(enrollment).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollment"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Enrollment enrollment)
        {
            this.set.Remove(enrollment);
            await this.dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollment"></param>
        /// <returns></returns>
        public async Task<IQueryable<EnrollmentArchive>> GetEnrollmentArchives(Enrollment enrollment)
        {
            return this.EnrollmentArchives.Where(e => e.RecruitPlanID == enrollment.RecruitPlanID && e.UserID == enrollment.UserID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollment"></param>
        /// <param name="archive"></param>
        /// <returns></returns>
        public async Task AddArchiveToEnrollment(Enrollment enrollment, EnrollmentArchive archive)
        {
            var currentEnrollment = await this.FindByIdAsync(enrollment.RecruitPlanID, enrollment.UserID);
            if (enrollment != null)
            {
                archive.RecruitPlanID = currentEnrollment.RecruitPlanID;
                archive.UserID = currentEnrollment.UserID;
                archive.WhenCreated = DateTime.Now;
                archive.WhenChanged = DateTime.Now;

                this.archiveSet.Add(archive);
                await this.dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollment"></param>
        /// <param name="archive"></param>
        /// <returns></returns>
        public async Task RemoveArchiveFromEnrollment(Enrollment enrollment, EnrollmentArchive archive)
        {
            var currentEnrollment = await this.FindByIdAsync(enrollment.RecruitPlanID, enrollment.UserID);
            if (enrollment != null)
            {
                var currentArch = this.archiveSet.FirstOrDefault(
                    e => e.RecruitPlanID == currentEnrollment.RecruitPlanID &&
                    e.UserID == currentEnrollment.UserID &&
                    e.id == archive.id);
                if (currentArch != null)
                {
                    this.archiveSet.Remove(currentArch);
                    await this.dbContext.SaveChangesAsync();
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~EnrollmentStore() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
