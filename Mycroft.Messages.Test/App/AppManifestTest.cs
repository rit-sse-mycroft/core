using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Messages.App;

namespace Mycroft.Messages.Test.App
{
    [TestClass]
    public class AppManifestTest
    {
        string SampleManifest = @"{
            ""version"": ""0.0.1"",
            ""name"": ""test-service"",
            ""displayName"": ""Mycroft test service"",
            ""instanceId"" : ""instance1"",
            ""capabilities"": {
                ""microphone"" : ""1.0.2"",
                ""speaker""    : ""4.2.1""
            },
            ""API"": 0,
            ""description"": ""It does odd stuff like testing or things"",
            ""dependencies"": {
                ""logger"": "">=1.2"",
                ""*"": """"
              }
            }";

        string MinimalManifest = @"{
            ""version"": ""0.0.1"",
            ""name"": ""test-service"",
            ""API"": 0,
            ""description"": ""It does odd stuff like testing or things""
            }";

        [TestMethod]
        public void TestAppManifestSerialization()
        {
            // just see if we can serialize and de-serialize two times
            var appManifest = AppManifest.DeSerialize(SampleManifest) as AppManifest;
            var str = appManifest.Serialize();
            appManifest = AppManifest.DeSerialize(str) as AppManifest;
            Assert.AreNotEqual(null, appManifest, "should still have a valid manifest");
            Assert.AreEqual(2, appManifest.Dependencies.Count, "should have 2 dependencies");
            Assert.AreEqual(2, appManifest.Capabilities.Count, "should have 2 capabilities");
        }

        [TestMethod]
        public void TestAppManifestDeSerialization()
        {
            var appManifest = AppManifest.DeSerialize(SampleManifest) as AppManifest;
            Assert.AreEqual("0.0.1", appManifest.Version, "version should be equal");
            Assert.AreEqual("test-service", appManifest.Name, "name should be equal");
            Assert.AreEqual("Mycroft test service", appManifest.DisplayName, "displayName should be equal");
            Assert.AreEqual("instance1", appManifest.InstanceId, "instanceId should be equal");
            Assert.AreEqual(0, appManifest.API, "API version should be equal");
            Assert.AreEqual("1.0.2", appManifest.Capabilities["microphone"], "microphone version should be the same");
            Assert.AreEqual("4.2.1", appManifest.Capabilities["speaker"], "speaker version should be the same");
            Assert.AreEqual(">=1.2", appManifest.Dependencies["logger"], "logger version should be the same");
            Assert.AreEqual("", appManifest.Dependencies["*"], "all-inclusive dependency version should be empty string");
        }

        [TestMethod]
        public void TestAppManifestMinimalDeSerialization()
        {
            var appManifest = AppManifest.DeSerialize(MinimalManifest) as AppManifest;
            Assert.AreEqual(0, appManifest.Dependencies.Count, "dependencies should be empty");
            Assert.AreEqual(0, appManifest.Capabilities.Count, "capabilities should be empty");
            Assert.AreEqual(null, appManifest.DisplayName, "displayName should be null");
            Assert.AreEqual(null, appManifest.InstanceId, "instanceId should be null");
        }
    }
}
