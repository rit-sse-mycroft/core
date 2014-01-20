using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Messages.App;
using System.Collections.Generic;

namespace Mycroft.Messages.Test.App
{
    [TestClass]
    public class AppDependencyTest
    {
        string SampleAppDependency = @"
        {
          ""Video"" : {
            ""GoogleTV"" : ""in use"",
            ""Projector"" : ""down""
          },
          ""Speaker"" : {
            ""SpeakerOne"" : ""up"",
            ""SpeakerTwo"" : ""up""
          }
        }";

        [TestMethod]
        public void TestAppDependencySerialization()
        {
            var dep = new AppDependency();
            dep.Dependencies = new Dictionary<string, Dictionary<string, string>>();
            var inner = new Dictionary<string, string>();
            inner["GoogleTV"] = "in use";
            dep.Dependencies["Video"] = inner;

            string json = dep.Serialize();

            Assert.IsTrue(json.IndexOf("\"GoogleTV\":\"in use\"") > 0, "should have google tv in use");
        }

        [TestMethod]
        public void TestAppDependencyDeSerialization()
        {
            var dep = AppDependency.DeSerialize(SampleAppDependency) as AppDependency;
            Assert.AreEqual(2, dep.Dependencies.Count, "should have 2 capabilities");
            Assert.AreEqual("in use", dep.Dependencies["Video"]["GoogleTV"]);
            Assert.AreEqual("down", dep.Dependencies["Video"]["Projector"]);

            Assert.AreEqual("up", dep.Dependencies["Speaker"]["SpeakerOne"]);
            Assert.AreEqual("up", dep.Dependencies["Speaker"]["SpeakerTwo"]);
        }
    }
}
