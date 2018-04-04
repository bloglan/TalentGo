using Microsoft.VisualStudio.TestTools.UnitTesting;
using TalentGo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentGoCore.Tests;

namespace TalentGo.Tests
{
    [TestClass()]
    public class RecruitmentPlanManagerTests
    {
        [TestMethod()]
        public async Task CompleteAuditAsyncTest()
        {
            //
            var plan = new RecruitmentPlan("Title", "Recruitment", DateTime.Now.AddDays(0));
            var manager = new RecruitmentPlanManager(new StubRecruitmentPlanStore());
            var job = new Job()
            {
                Name = "Job",
            };
            plan.Jobs.Add(job);
            var form = new ApplicationForm(job, new StubPerson());
            job.ApplicationForms.Add(form);
            new PrivateObject(plan).SetProperty(nameof(plan.EnrollExpirationDate), DateTime.Now);
            new PrivateObject(plan).SetProperty(nameof(plan.WhenPublished), DateTime.Now);

            await manager.CompleteAuditAsync(plan);

            Assert.IsNotNull(form.WhenAuditComplete);
            Assert.IsNotNull(plan.WhenAuditCommited);
        }
    }
}