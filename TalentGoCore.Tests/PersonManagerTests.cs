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
            var user = new TestUser("530302198501150314", "Surname", "GivenName", "Mobile", "Email");
            
            await manager.CreateAsync(user);

            await manager.UpdateRealIdAsync(user, "张", "三", "汉", "地址", "Issuer", new DateTime(2008, 2, 21), new DateTime(2018, 2, 21));

            var po = new PrivateObject(user);
            po.SetProperty(nameof(user.IDCardFrontFile), "12345");
            po.SetProperty(nameof(user.IDCardBackFile), "12345");
            await manager.CommitForRealIdValidationAsync(user);
        }
    }
}