using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Manifest;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Mycroft.Tests.Manifest
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
                    ""logging"": "">=1.2"",
                    ""*"": """"
            }}";

            Mycroft.Manifest.Manifest.parse(input);
        }
        [TestMethod]
        public void TestParseMissingVals()
        {
            var input = "{}";
            try
            {
                Mycroft.Manifest.Manifest.parse(input);
            }
            catch (SerializationException e)
            {
                Trace.Write(e.Message);
                Assert.IsTrue(e.Message.Contains("'API, description, displayName, name, version' were not found"));
                return;
            }
            throw new Exception("Missing values not caught");
        }
    }
}
