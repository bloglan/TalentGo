using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    /// <summary>
    /// An interface definition for support read/write enrollment from base data store.
    /// </summary>
    public interface IEnrollmentStore : IDisposable
    {
        /// <summary>
        /// Gets an queryable collection of enrollment.
        /// </summary>
        IQueryable<Enrollment> Enrollments { get; }

        /// <summary>
        /// Finds one enrollment by it's primary key.
        /// </summary>
        /// <param name="PlanId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<Enrollment> FindByIdAsync(int PlanId, int UserId);

        /// <summary>
        /// Stores an enrollment into base data store.
        /// </summary>
        /// <param name="Enrollment"></param>
        /// <returns></returns>
        Task CreateAsync(Enrollment Enrollment);

        /// <summary>
        /// Updates an enrollment to base data store.
        /// </summary>
        /// <param name="Enrollment"></param>
        /// <returns></returns>
        Task UpdateAsync(Enrollment Enrollment);

        /// <summary>
        /// Deletes an enrollment from base data store.
        /// </summary>
        /// <param name="Enrollment"></param>
        /// <returns></returns>
        Task DeleteAsync(Enrollment Enrollment);
    }
}
