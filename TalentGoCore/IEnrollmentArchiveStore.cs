using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEnrollmentArchiveStore : IApplicationFormStore, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<EnrollmentArchive> EnrollmentArchives { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollment"></param>
        /// <returns></returns>
        Task<IQueryable<EnrollmentArchive>> GetEnrollmentArchives(ApplicationForm enrollment);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollment"></param>
        /// <param name="archive"></param>
        /// <returns></returns>
        Task AddArchiveToEnrollment(ApplicationForm enrollment, EnrollmentArchive archive);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enrollment"></param>
        /// <param name="archive"></param>
        /// <returns></returns>
        Task RemoveArchiveFromEnrollment(ApplicationForm enrollment, EnrollmentArchive archive);
	}
}
