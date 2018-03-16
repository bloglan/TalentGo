using Microsoft.VisualStudio.TestTools.UnitTesting;
using TalentGo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.Tests
{
    [TestClass()]
    public class FileIdListTests
    {
        [TestMethod()]
        public void FileIdListTest()
        {
            //
        }

        [TestMethod()]
        public void AddTest()
        {
            var fileidString = string.Empty;
            var list = new FileIdList(fileidString, (s) => fileidString = s)
            {
                "Test1",
                "Test2"
            };

            Assert.AreEqual("Test1|Test2", fileidString);
        }

        [TestMethod()]
        public void ClearTest()
        {
            var fileidString = string.Empty;
            var list = new FileIdList(fileidString, (s) => fileidString = s)
            {
                "Test1",
                "Test2"
            };
            list.Clear();
            Assert.AreEqual(string.Empty, fileidString);
        }

        [TestMethod()]
        public void ContainsTest()
        {
            var fileidString = string.Empty;
            var list = new FileIdList(fileidString, (s) => fileidString = s)
            {
                "Test1",
                "Test2"
            };
            Assert.IsTrue(list.Contains("Test2"));
        }

        [TestMethod()]
        public void CopyToTest()
        {
            var fileidString = string.Empty;
            var list = new FileIdList(fileidString, (s) => fileidString = s)
            {
                "Test1",
                "Test2"
            };
            var array = new string[2];
            list.CopyTo(array, 0);

            Assert.AreEqual("Test1", array[0]);
            Assert.AreEqual("Test2", array[1]);
        }

        [TestMethod()]
        public void GetEnumeratorTest()
        {
            //
        }

        [TestMethod()]
        public void IndexOfTest()
        {
            var fileidString = string.Empty;
            var list = new FileIdList(fileidString, (s) => fileidString = s)
            {
                "Test1",
                "Test2"
            };
            Assert.AreEqual(1, list.IndexOf("Test2"));
        }

        [TestMethod()]
        public void InsertTest()
        {
            //
        }

        [TestMethod()]
        public void RemoveTest()
        {
            //
        }

        [TestMethod()]
        public void RemoveAtTest()
        {
            //
        }
    }
}