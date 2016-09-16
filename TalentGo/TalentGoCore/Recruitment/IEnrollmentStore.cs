using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    public interface IEnrollmentStore
    {

        IQueryable<Enrollment> Enrollments { get; }

        Task<Enrollment> FindByIdAsync(int PlanId, int UserId);

        Task CreateAsync(Enrollment Enrollment);

        Task UpdateAsync(Enrollment Enrollment);

        Task DeleteAsync(Enrollment Enrollment);
    }
}
