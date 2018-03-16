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
    public class PersonManagerTests
    {
        [TestMethod()]
        public async Task CommitForRealIdValidationAsyncTest()
        {
            var manager = new PersonManager(new StubPersonStore(), new StubFileStore());
            var user = new TestUser
            {
                Surname = "张",
                GivenName = "三",
                Sex = Sex.Male,
                DateOfBirth = new DateTime(1985,1,15),
            };
            await manager.CreateAsync(user);

            await manager.CommitForRealIdValidationAsync(user);
        }
    }
}