using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft;

namespace Mycroft.Tests
{
    [TestClass]
    public class TestLogger
    {
        [TestMethod]
        public void TestCreation()
        {
            var instance = Logger.GetInstance();
            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void TestLogging()
        {
            var instance = Logger.GetInstance();
            Assert.IsTrue(instance.Info("This is a log message"), "should succeed in logging");

            var filename = Path.Combine("logs", DateTime.Now.ToString("yyyy-MM-dd") + ".log");

            Assert.IsTrue(File.Exists(filename));
        }
    }
}
