using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    public interface IEnrollmentArchiveStore : IEnrollmentStore
	{
        IQueryable<EnrollmentArchives> EnrollmentArchives { get; }

        Task<IQueryable<EnrollmentArchives>> GetEnrollmentArchives(EnrollmentData enrollment);

        Task AddArchiveToEnrollment(EnrollmentData enrollment, EnrollmentArchives archive);

        Task RemoveArchiveFromEnrollment(EnrollmentData enrollment, EnrollmentArchives archive);
	}
}
