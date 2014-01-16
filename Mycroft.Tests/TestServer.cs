using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft;
using System.Threading.Tasks;
using System.Reflection;

namespace Mycroft.Tests
{
    [TestClass]
    public class TestServer
    {
        class FakeServer : Server
        {
            public override Task Start()
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Tests that an instance ID can be successfully changed
        /// </summary>
        [TestMethod]
        public void TestChangeInstanceId_NoDuplicate()
        {
            Server server = new FakeServer();
            server.HandleClientConnected(null);
            var result = server.ChangeInstanceId("", "theNewId");
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests that an instance ID is not changed if the new ID already exists
        /// </summary>
        [TestMethod]
        public void TestChangeInstanceId_Duplicate()
        {
            Assert.IsTrue(false);
        }

        /// <summary>
        /// Tests that false is returned when the old ID doesn't exist
        /// </summary>
        [TestMethod]
        public void TestChangeInstanceId_DoesntExist()
        {
            Server server = new FakeServer();
            var result = server.ChangeInstanceId("missingId", "theNewId");
            Assert.IsFalse(result);
        }
    }
}
