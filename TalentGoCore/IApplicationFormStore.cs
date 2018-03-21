using System;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// An interface definition for support read/write enrollment from base data store.
    /// </summary>
    public interface IApplicationFormStore
    {
        /// <summary>
        /// Gets an queryable collection of enrollment.
        /// </summary>
        IQueryable<ApplicationForm> ApplicationForms { get; }

        /// <summary>
        /// Finds one enrollment by it's primary key.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ApplicationForm> FindByIdAsync(int id);

        /// <summary>
        /// Stores an enrollment into base data store.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task CreateAsync(ApplicationForm form);

        /// <summary>
        /// Updates an enrollment to base data store.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task UpdateAsync(ApplicationForm form);

        /// <summary>
        /// Deletes an enrollment from base data store.
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        Task DeleteAsync(ApplicationForm form);
    }
}
