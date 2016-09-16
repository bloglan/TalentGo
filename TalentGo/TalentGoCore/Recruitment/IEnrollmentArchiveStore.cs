using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
    public interface IEnrollmentArchiveStore : IEnrollmentStore
	{
        IQueryable<EnrollmentArchive> EnrollmentArchives { get; }

        Task<IQueryable<EnrollmentArchive>> GetEnrollmentArchives(Enrollment enrollment);

        Task AddArchiveToEnrollment(Enrollment enrollment, EnrollmentArchive archive);

        Task RemoveArchiveFromEnrollment(Enrollment enrollment, EnrollmentArchive archive);
	}
}
