using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Recruitment
{
	public interface IRecruitmentStore<T> : IDataStore<T, int> where T : RecruitmentPlan
	{
		//Task<T> FindByIdAsync(int Id);

		//Task CreateAsync(T Plan);

		//Task UpdateAsync(T Plan);

		//Task DeleteAsync(int Id);
	}
}
