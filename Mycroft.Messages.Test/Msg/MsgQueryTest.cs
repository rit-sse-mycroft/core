using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Diagnostics;
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
            var msgQuery = new MsgQuery();
            msgQuery.Id = "uuid";
            msgQuery.Capability = "weather";
            msgQuery.Action = "get_temperature";
            var instanceIds = new List<string>();
            instanceIds.Add("xxxx");
            instanceIds.Add("xx2");
            msgQuery.InstanceId = instanceIds;
            var data = new Dictionary<string, object>();
            data["scale"] = "fahrenheit";
            data["other"] = "thing";

            Stream memoryStream = new MemoryStream();
            var settings = new DataContractJsonSerializerSettings();
            settings.UseSimpleDictionaryFormat = true;
            var serializer = new DataContractJsonSerializer(typeof(MsgQuery), settings);
            serializer.WriteObject(memoryStream, msgQuery);

            memoryStream.Seek(0, 0);
            long length = memoryStream.Length;
            byte[] buff = new byte[length];
            memoryStream.Read(buff, 0, (int)length);
            string json = Encoding.UTF8.GetString(buff);
            Debug.WriteLine(json);
            Assert.IsFalse(json.IndexOf("\"data\":null") >= 0);
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
