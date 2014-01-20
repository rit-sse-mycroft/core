using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Messages.App;

namespace Mycroft.Messages.Test.App
{
    [TestClass]
    public class AppInUseTest
    {
        string SampleMessage = @"
        {
            ""priority"" : 10
        }
        ";
        [TestMethod]
        public void TestAppInUseDeSerialization()
        {
            var appInUse = AppInUse.DeSerialize(SampleMessage) as AppInUse;
            Assert.AreEqual(10, appInUse.Priority);
        }

        [TestMethod]
        public void TestAppInUseSerialization()
        {
            var appInUse = new AppInUse();
            appInUse.Priority = 10;

            string json = appInUse.Serialize();
            System.Diagnostics.Debug.WriteLine(json);
            Assert.IsTrue(json.IndexOf("\"priority\":10") > 0, "should have priority 10");
        }
    }
}
