using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Messages.App;

namespace Mycroft.Messages.Test.App
{
    [TestClass]
    public class AppManifestOkTest
    {
        string SampleManifestOk = @"
        {
            ""instanceId"" : ""inst101""
        }
        ";

        [TestMethod]
        public void TestAppManifestOkDeSerialization()
        {
            var mfstOk = AppManifestOk.Deserialize(SampleManifestOk) as AppManifestOk;
            Assert.AreEqual("inst101", mfstOk.InstanceId);
        }

        [TestMethod]
        public void TestAppManifestOkSerialization()
        {
            var mfstOk = new AppManifestOk();
            mfstOk.InstanceId = "inst101";
            string json = mfstOk.Serialize();

            Assert.IsTrue(json.IndexOf("\"instanceId\":\"inst101\"") > 0, "Should have inst101 for instanceId");
        }
    }
}
