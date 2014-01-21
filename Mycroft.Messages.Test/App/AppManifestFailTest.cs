using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Messages.App;

namespace Mycroft.Messages.Test.App
{
    [TestClass]
    public class AppManifestFailTest
    {
        string SampleMessage = @"
            {
              ""message"":""this is a message""
            }
        ";
        [TestMethod]
        public void TestAppManifestFailDeSerialization()
        {
            var appManifestFail = AppManifestFail.Deserialize(SampleMessage) as AppManifestFail;
            Assert.AreEqual("this is a message", appManifestFail.Message, "Message should match");
        }

        [TestMethod]
        public void TestAppManifestFailSerialization()
        {
            var appManifestFail = new AppManifestFail();
            appManifestFail.Message = "foo1";
            var json = appManifestFail.Serialize();

            Assert.IsTrue(json.IndexOf("\"message\":\"foo1\"") > 0, "should have a message foo1");
        }
    }
}
