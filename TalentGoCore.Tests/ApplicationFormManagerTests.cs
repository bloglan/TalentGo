using Microsoft.VisualStudio.TestTools.UnitTesting;
using TalentGo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentGoCore.Tests;
using Microsoft.QualityTools.Testing.Fakes;
using System.Fakes;

namespace TalentGo.Tests
{
    [TestClass()]
    public class ApplicationFormManagerTests
    {
        [TestMethod()]
        public async Task EnrollAsyncTest()
        {
            using (ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => { return new DateTime(2018, 3, 15, 16, 00, 00); };

                var job = new Job
                {
                    Name = "Job",
                    WorkLocation = "WorkLocation",
                };
                var jobPo = new PrivateObject(job);

                var plan = new RecruitmentPlan("Title", "Recruitment", new DateTime(2018, 3, 15, 17, 00, 00));
                jobPo.SetProperty(nameof(job.Plan), plan);

                var manager = new ApplicationFormManager(new StubApplicationFormStore(), new StubFileStore());
                var form = new ApplicationForm(job, new StubPerson());
                var formPo = new PrivateObject(form);
                formPo.SetProperty(nameof(form.HeadImageFile), "HeadImageFile");
                formPo.SetProperty(nameof(form.AcademicCertFiles), "Files");
                try
                {
                    await manager.EnrollAsync(form);
                    Assert.AreEqual(new DateTime(2018, 3, 15, 16, 00, 00), form.WhenCreated);
                }
                catch (InvalidOperationException)
                {

                    throw;
                }
            }

        }

        [TestMethod()]
        public async Task CommitAsyncTest()
        {
            var job = new Job
            {
                Name = "Job",
                WorkLocation = "WorkLocation",
            };
            var jobPo = new PrivateObject(job);

            var plan = new RecruitmentPlan("Title", "Recruitment", new DateTime(2018, 3, 15, 17, 00, 00));

            jobPo.SetProperty(nameof(job.Plan), plan);
            var manager = new ApplicationFormManager(new StubApplicationFormStore(), new StubFileStore());

            //准备报名表
            var form = new ApplicationForm(job, new StubPerson());
            var formPo = new PrivateObject(form);
            formPo.SetProperty(nameof(form.HeadImageFile), "HeadImageFile");
            formPo.SetProperty(nameof(form.AcademicCertFiles), "Files");

            var form1 = new ApplicationForm(job, new StubPerson());
            var form1Po = new PrivateObject(form1);
            form1Po.SetProperty(nameof(form1.HeadImageFile), "HeadImageFile");
            form1Po.SetProperty(nameof(form1.AcademicCertFiles), "Files");

            //创建
            await manager.EnrollAsync(form);
            await manager.EnrollAsync(form1);

            //首次提交
            using (ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(2018, 3, 15, 16, 0, 0);

                await manager.CommitAsync(form);
                Assert.AreEqual(new DateTime(2018, 3, 15, 16, 0, 0), form.WhenCommited.Value);
                //重复提交
                try
                {
                    await manager.CommitAsync(form);
                    Assert.Fail("重复提交未引发异常");
                }
                catch (Exception)
                {
                    //Test Pass
                }
            }

            //报名时间已过提交
            using (ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(2018, 3, 15, 18, 0, 0);
                try
                {
                    await manager.CommitAsync(form1);
                    Assert.Fail("超时提交未引发异常");
                }
                catch (InvalidOperationException)
                {
                    //Test Pass
                }
            }

            //已审查的报名表退回再提交（晚于截止时间）
            await manager.FileReviewAsync(form, false, "FileReviewedBy");
            await manager.ReturnBackAsync(form);
            using (ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(2018, 3, 15, 18, 0, 0);
                await manager.CommitAsync(form);
                Assert.AreEqual(new DateTime(2018, 3, 15, 18, 0, 0), form.WhenCommited.Value);
                Assert.IsFalse(form.WhenFileReviewed.HasValue);
                Assert.IsFalse(form.FileReviewAccepted.HasValue);
                Assert.IsNull(form.FileReviewedBy);
            }
        }


    }
}