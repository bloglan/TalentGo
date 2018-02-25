using Microsoft.VisualStudio.TestTools.UnitTesting;
using TalentGo.Recruitment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using System.Fakes;

namespace TalentGo.Recruitment.Tests
{
    [TestClass()]
    public class EnrollmentTests
    {
        [TestMethod]
        public void CommitTest()
        {
            using (ShimsContext.Create())
            {
                DateTime testNow = new DateTime(2016, 9, 17, 21, 48, 22);
                ShimDateTime.NowGet = () => testNow;

                Enrollment enrollment = new Enrollment(null, null);
                PrivateObject p = new PrivateObject(enrollment);
                p.Invoke("Commit");

                Assert.IsNotNull(enrollment.WhenCommited);
                Assert.AreEqual(testNow, enrollment.WhenCommited.Value);

                try
                {
                    //Commit twice, and it will occur an InvalidOperationException.
                    p.Invoke("Commit");
                }
                catch (Exception ex)
                {
                    Assert.AreEqual(typeof(InvalidOperationException), ex.GetType());
                }

            }
                
        }

        [TestMethod()]
        public void AcceptTest()
        {
            using (ShimsContext.Create())
            {
                DateTime testNow = new DateTime(2016, 9, 17, 21, 48, 22);
                ShimDateTime.NowGet = () => testNow;

                Enrollment enrollment = new Enrollment(null, null);
                PrivateObject p = new PrivateObject(enrollment);
                p.Invoke("Commit");

                enrollment.Accept();
                Assert.IsNotNull(enrollment.Approved);
                if (enrollment.Approved.HasValue)
                {
                    Assert.IsTrue(enrollment.Approved.Value);
                }

            }
                
        }
    }
}