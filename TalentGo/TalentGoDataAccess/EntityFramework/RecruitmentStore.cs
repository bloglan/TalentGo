using System.Linq;
using System.Threading.Tasks;
using TalentGo.Recruitment;

namespace TalentGo.Utilities
{
    public class RecruitmentStore : IRecruitmentPlanStore, IArchiveRequirementStore
    {
        TalentGoDbContext db;
        public RecruitmentStore(TalentGoDbContext DbContext)
        {
            this.db = DbContext;
        }

        public IQueryable<RecruitmentPlan> RecruitmentPlans
        {
            get
            {
                return this.db.RecruitmentPlan.AsNoTracking();
            }
        }


        public async Task CreateAsync(RecruitmentPlan Plan)
        {
            this.db.RecruitmentPlan.Add(Plan);
            await this.db.SaveChangesAsync();
        }

        public async Task UpdateAsync(RecruitmentPlan Plan)
        {
            RecruitmentPlan old = await this.FindByIdAsync(Plan.id);
            if (old != null)
            {
                var entry = this.db.Entry<RecruitmentPlan>(old);
                entry.CurrentValues.SetValues(Plan);
                entry.Property(p => p.WhenCreated).IsModified = false;

                await this.db.SaveChangesAsync();
            }
            
        }


        public async Task DeleteAsync(RecruitmentPlan Plan)
        {
            var current = await this.FindByIdAsync(Plan.id);
            if (current != null)
            {
                this.db.RecruitmentPlan.Remove(current);
                await this.db.SaveChangesAsync();
            }
            
        }

        public async Task<RecruitmentPlan> FindByIdAsync(int Id)
        {
            return this.db.RecruitmentPlan.FirstOrDefault(plan => plan.id == Id);
        }

        public async Task<IQueryable<ArchiveRequirement>> GetArchiveRequirementsAsync(RecruitmentPlan plan)
        {
            return this.db.ArchiveRequirements.Where(ar => ar.RecruitmentPlanID == plan.id);
        }

        public async Task AddArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirement requirement)
        {
            this.db.ArchiveRequirements.Add(requirement);
            await this.db.SaveChangesAsync();
        }

        public async Task UpdateArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirement requirement)
        {
            var current = (await this.GetArchiveRequirementsAsync(plan)).FirstOrDefault(r => r.ArchiveCategoryID == requirement.ArchiveCategoryID && r.RecruitmentPlanID == requirement.RecruitmentPlanID);
            if (current != null)
            {
                var entry = this.db.Entry<ArchiveRequirement>(current);
                entry.CurrentValues.SetValues(requirement);

                await this.db.SaveChangesAsync();
            }
        }


        public async Task RemoveArchiveRequirementAsync(RecruitmentPlan plan, ArchiveRequirement requirement)
        {
            var current = (await this.GetArchiveRequirementsAsync(plan)).FirstOrDefault(r => r.ArchiveCategoryID == requirement.ArchiveCategoryID && r.RecruitmentPlanID == requirement.RecruitmentPlanID);
            if (current != null)
            {
                this.db.ArchiveRequirements.Remove(current);
                await this.db.SaveChangesAsync();
            }
        }


    }
}
