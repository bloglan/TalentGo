using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
    /// <summary>
    /// Person Store.
    /// </summary>
    public interface IPersonStore
    {
        /// <summary>
        /// Get people collection
        /// </summary>
        IQueryable<Person> People { get; }

        /// <summary>
        /// find person by id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<Person> FindByIdAsync(Guid Id);

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task CreateAsync(Person person);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task UpdateAsync(Person person);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task DeleteAsync(Person person);
    }
}
