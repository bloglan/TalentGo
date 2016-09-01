using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentGo.Recruitment;

namespace TalentGo.EntityFramework
{
	public class RecruitmentStore :
		IDataStore<RecruitmentPlan, int>,
		IDataStore<EnrollmentData, Tuple<int, int>>,
		IEnrollmentArchiveStore
	{
		TalentGoDbContext dbContext;
		public RecruitmentStore(TalentGoDbContext DbContext)
		{
			this.dbContext = DbContext;
		}

		public DbContext DbContext
		{
			get
			{
				return this.dbContext;
			}
		}

		public Task CreateAsync(EnrollmentData Data)
		{
			throw new NotImplementedException();
		}

		public Task CreateAsync(EnrollmentArchives Data)
		{
			throw new NotImplementedException();
		}

		public async Task CreateAsync(Recruitment.RecruitmentPlan Data)
		{
			this.dbContext.RecruitmentPlan.Add(Data);
			await this.dbContext.SaveChangesAsync();
		}

		public Task DeleteAsync(EnrollmentData Data)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(EnrollmentArchives Data)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(Recruitment.RecruitmentPlan Data)
		{
			throw new NotImplementedException();
		}

		public Task<EnrollmentData> FindByKeyAsync(Tuple<int, int> Key)
		{

			return Task.FromResult(this.dbContext.EnrollmentData.SingleOrDefault(e => e.RecruitPlanID == Key.Item1 && e.UserID == Key.Item2));
		}

		public Task<Recruitment.RecruitmentPlan> FindByKeyAsync(int Key)
		{
			throw new NotImplementedException();
		}

		public Task<ICollection<EnrollmentArchives>> GetEnrollmentArchives(int PlanID, int UserID)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(EnrollmentData Data)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(EnrollmentArchives Data)
		{
			throw new NotImplementedException();
		}

		public Task UpdateAsync(Recruitment.RecruitmentPlan Data)
		{
			throw new NotImplementedException();
		}

		Task<EnrollmentArchives> IDataStore<EnrollmentArchives, int>.FindByKeyAsync(int Key)
		{
			throw new NotImplementedException();
		}
	}
}
