using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    public interface IEnrollmentStore
    {

        IQueryable<EnrollmentData> Enrollments { get; }

        Task<EnrollmentData> FindByIdAsync(int PlanId, int UserId);

        Task CreateAsync(EnrollmentData Enrollment);

        Task UpdateAsync(EnrollmentData Enrollment);

        Task DeleteAsync(EnrollmentData Enrollment);
    }
}
