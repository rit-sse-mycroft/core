using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mycroft.Messages.Test.Msg
{
    [TestClass]
    public class MsgQueryTest
    {
        private string SampleQuery = @"
         MSG_QUERY {
          ""id"" : ""uuid"",
          ""capability"" : ""weather"",
          ""action"" : ""get_temperature"",
          ""data"" : {
             ""scale"": ""fahrenheit"",
             ""other"" : ""thing""
          },
          ""instanceId"" : [""xxxx""],
          ""priority"" : 30
        }";

        [TestMethod]
        public void TestMsgQuerySerialization()
        {
        }

        [TestMethod]
        public void TestMsgQueryDeserialization()
        {
        }
    }
}
