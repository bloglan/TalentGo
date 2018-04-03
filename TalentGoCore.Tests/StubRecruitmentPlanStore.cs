using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TalentGo.Tests
{
    internal class StubRecruitmentPlanStore : IRecruitmentPlanStore
    {
        List<RecruitmentPlan> innerList;

        public StubRecruitmentPlanStore()
        {
            this.innerList = new List<RecruitmentPlan>();
        }

        public IQueryable<RecruitmentPlan> Plans => this.innerList.AsQueryable();

        public Task CreateAsync(RecruitmentPlan plan)
        {
            var last = this.innerList.OrderBy(r => r.Id).LastOrDefault();
            var lastid = last == null ? 0 : last.Id;
            new PrivateObject(plan).SetProperty(nameof(plan.Id), lastid + 1);
            this.innerList.Add(plan);

            return Task.FromResult(0);
        }

        public Task DeleteAsync(RecruitmentPlan plan)
        {
            this.innerList.Remove(plan);
            return Task.FromResult(0);
        }

        public Task<RecruitmentPlan> FindByIdAsync(int id)
        {
            return Task.FromResult(this.innerList.FirstOrDefault(r => r.Id == id));
        }

        public Task UpdateAsync(RecruitmentPlan plan)
        {
            //do nothing here.
            return Task.FromResult(0);
        }
    }
}