using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Runtime.Serialization;
using Mycroft.Cmd.App;

namespace Mycroft.Tests.Cmd.App
{
    [TestClass]
    public class TestManifest
    {
        [TestMethod]
        public void TestParseBasic()
        {
            var input = @"{
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
                    ""logging"": ""1.2"",
                    ""*"": ""*""
            }}";
            AppCommand cmd = AppCommand.Parse("APP_MANIFEST", input, null) as AppCommand;
            Assert.IsInstanceOfType(cmd, typeof(Create));
        }

        [TestMethod]
        public void TestParseMissingVals()
        {
            var input = "{}";
            AppCommand cmd = AppCommand.Parse("APP_MANIFEST", input, null) as AppCommand;
            Assert.IsInstanceOfType(cmd, typeof(ManifestFail));
            var manifestFail = cmd as ManifestFail;
            string msg = manifestFail.Fail.Message;
            Trace.Write(msg);
            Assert.IsTrue(msg.Contains("API"), "should complain about API");
            Assert.IsTrue(msg.Contains("description"), "should complain about description");
            Assert.IsTrue(msg.Contains("displayName"), "should complain about displayName");
            Assert.IsTrue(msg.Contains("name"), "should complain about name");
            Assert.IsTrue(msg.Contains("version"), "should complain about version");
        }

        [TestMethod]
        public void TestNonSemanticVersions()
        {
            var input = @"{
                ""version"": ""s0.0.1"",
                ""name"": ""test-service"",
                ""displayName"": ""Mycroft test service"",
                ""instanceId"" : ""instance1"",
                ""capabilities"": {
                    ""microphone"" : "".0.2"",
                    ""speaker""    : ""4.2.1.3""
                },
                ""API"": 0,
                ""description"": ""It does odd stuff like testing or things"",
                ""dependencies"": {
                    ""logging"": ""HEY!"",
                    ""*"": ""*""
            }}";
            AppCommand cmd = AppCommand.Parse("APP_MANIFEST", input, null) as AppCommand;
            Assert.IsInstanceOfType(cmd, typeof(ManifestFail));
            var manifestFail = cmd as ManifestFail;
            string msg = manifestFail.Fail.Message;
            Trace.Write(msg);
            Assert.IsTrue(msg.Contains("version"), "should complain about version");
            Assert.IsTrue(msg.Contains("capabilities"), "should complain about capabilities)");
            Assert.IsTrue(msg.Contains("dependencies"), "should complain about depdendencies");
        }
    }
}
