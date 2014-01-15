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
                    ""logging"": ""1.2"",
                    ""*"": ""*""
            }}";
            try
            {
                Mycroft.Manifest.Manifest.Parse(input);
            }
            catch (Mycroft.Manifest.ManifestValidationException e)
            {
                foreach (var item in e.Fields)
                {
                    foreach(var err in item.Value)
                        Trace.WriteLine(err);
                }
                throw e;
            }
        }
        [TestMethod]
        public void TestParseMissingVals()
        {
            var input = "{}";
            try
            {
                Mycroft.Manifest.Manifest.Parse(input);
            }
            catch (SerializationException e)
            {
                Trace.Write(e.Message);
                Assert.IsTrue(e.Message.Contains("'API, description, displayName, name, version' were not found"));
                return;
            }
            throw new Exception("Missing values not caught");
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

            try
            {
                Mycroft.Manifest.Manifest.Parse(input);
            }
            catch (Mycroft.Manifest.ManifestValidationException err)
            {
                if (!err.Fields.ContainsKey("version"))
                    throw new Exception("Validator failed to catch app version error");
                if (!err.Fields.ContainsKey("capabilities") || err.Fields["capabilities"].Count != 2)
                    throw new Exception("Validator failed to catch semantic version errors in capabilities");
                if (!err.Fields.ContainsKey("dependencies") || err.Fields["dependencies"].Count != 1)
                    throw new Exception("Validator failed to catch semantic version errors in dependencies");
                return;
            }

            throw new Exception("Oh god, we didn't catch anything!");

        }
    }
}
