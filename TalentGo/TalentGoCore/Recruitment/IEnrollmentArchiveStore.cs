using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
	public interface IEnrollmentArchiveStore : IDataStore<EnrollmentArchives, int>
	{
		Task<ICollection<EnrollmentArchives>> GetEnrollmentArchives(int PlanID, int UserID);

	}
}
