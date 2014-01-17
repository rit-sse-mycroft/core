using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using Mycroft.Messages.Msg;

namespace Mycroft.Messages.Test.Msg
{
    [TestClass]
    public class MsgQueryTest
    {
        private string SampleQuery = @"
         {
          ""id"" : ""uuid"",
          ""capability"" : ""weather"",
          ""action"" : ""get_temperature"",
          ""instanceId"" : [""xxxx"", ""xx2""],
          ""data"" : {
             ""scale"" : ""fahrenheit"",
             ""other"" : ""thing""
          },
          ""priority"" : 30
        }";

        [TestMethod]
        public void TestMsgQuerySerialization()
        {
        }

        [TestMethod]
        public void TestMsgQueryDeserialization()
        {
            var settings = new DataContractJsonSerializerSettings();
            settings.UseSimpleDictionaryFormat = true;
            var serializer = new DataContractJsonSerializer(typeof(MsgQuery), settings);
            var memStream = new MemoryStream(Encoding.UTF8.GetBytes(SampleQuery));
            MsgQuery msgQuery = serializer.ReadObject(memStream) as MsgQuery;

            Assert.AreEqual("uuid", msgQuery.Id);
            Assert.AreEqual("weather", msgQuery.Capability);
            Assert.AreEqual("get_temperature", msgQuery.Action);
            Assert.AreEqual(30, msgQuery.Priority);
            Assert.AreEqual("xxxx", msgQuery.InstanceId[0]);
            Assert.AreEqual("xx2", msgQuery.InstanceId[1]);
            Assert.AreEqual("fahrenheit", msgQuery.Data["scale"]);
            Assert.AreEqual("thing", msgQuery.Data["other"]);
        }
    }
}
