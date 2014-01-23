using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Web.Helpers;
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
             ""other"" : {
               ""k"" : ""v""
             }
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
            var inner = new Dictionary<string, object>();
            inner["k"] = "v";
            data["other"] = inner;
            msgQuery.Data = data;

            string json = msgQuery.Serialize();
            Debug.WriteLine(json);

            Assert.IsFalse(json.IndexOf("\"data\":null") >= 0, "Sould not contain null for data");
            Assert.IsFalse(json.IndexOf("KeyValuePairOf") >= 0, "Should not have strange toString");
            Assert.IsFalse(json.IndexOf("\"data\":[]") >= 0, "Should not have empty array for data");
        }

        [TestMethod]
        public void TestMsgQueryDeserialization()
        {
            MsgQuery msgQuery = MsgQuery.Deserialize(SampleQuery) as MsgQuery;

            Assert.AreEqual("uuid", msgQuery.Id);
            Assert.AreEqual("weather", msgQuery.Capability);
            Assert.AreEqual("get_temperature", msgQuery.Action);
            Assert.AreEqual(30, msgQuery.Priority);
            Assert.AreEqual("xxxx", msgQuery.InstanceId[0]);
            Assert.AreEqual("xx2", msgQuery.InstanceId[1]);

            string json = msgQuery.Serialize();
            Debug.WriteLine(json);
            Assert.IsFalse(json.IndexOf("\"data\":{}") >= 0, "data should not be an empty object");
        }

        [TestMethod]
        public void TestMsgQueryUndirected()
        {
            string input1 = @"
             {
              ""id"" : ""uuid"",
              ""capability"" : ""weather"",
              ""action"" : ""get_temperature"",
              ""instanceId"" : [],
              ""data"" : {
                 ""scale"" : ""fahrenheit"",
                 ""other"" : {
                   ""k"" : ""v""
                 }
              },
              ""priority"" : 30
            }";
            string input2 = @"
             {
              ""id"" : ""uuid"",
              ""capability"" : ""weather"",
              ""action"" : ""get_temperature"",
              ""data"" : {
                 ""scale"" : ""fahrenheit"",
                 ""other"" : {
                   ""k"" : ""v""
                 }
              },
              ""priority"" : 30
            }";

            var msgQuery = MsgQuery.Deserialize(input1) as MsgQuery;
            Assert.AreEqual(0, msgQuery.InstanceId.Count, "should hvae 0 instance IDs");

            msgQuery = MsgQuery.Deserialize(input2) as MsgQuery;
            Assert.AreEqual(0, msgQuery.InstanceId.Count, "should hvae 0 instance IDs");
            
        }

        [TestMethod]
        public void TestMsgQueryInvalidJson()
        {
            try
            {
                MsgQuery.Deserialize("{");
                Assert.Fail("01 should have thrown exception");
            }
            catch (ParseException ex)
            {
                Assert.AreEqual("{", ex.Received);
            }

            try
            {
                MsgQuery.Deserialize("{}");
                Assert.Fail("02 should have thrown exception");
            }
            catch (ParseException ex)
            {
                Assert.AreEqual("{}", ex.Received);
            }
        }

        [TestMethod]
        public void TestMsgQueryNestedArray()
        {
            string input = @"
             {
              ""id"" : ""uuid"",
              ""capability"" : ""weather"",
              ""action"" : ""get_temperature"",
              ""data"" : {
                 ""text to say"" : [
                     {""foo"" : ""bar"", ""baz"" : ""bing""} 
                 ]
              },
              ""priority"" : 30
            }";

            var msgQuery = MsgQuery.Deserialize(input) as MsgQuery;

            string output = msgQuery.Serialize();

            Debug.WriteLine(output);

            Assert.IsFalse(output.Contains("\"text to say\":{}"), "text to say should not be empty");
        }

        [TestMethod]
        public void TestMsgQueryDataNotAnObject()
        {
            string input1 = @"
             {
              ""id"" : ""uuid"",
              ""capability"" : ""weather"",
              ""action"" : ""get_temperature"",
              ""data"" : ""foo"",
              ""priority"" : 30
            }";

            string input2 = @"
             {
              ""id"" : ""uuid"",
              ""capability"" : ""weather"",
              ""action"" : ""get_temperature"",
              ""data"" : 1,
              ""priority"" : 30
            }";

            var msgQ = MsgQuery.Deserialize(input1) as MsgQuery;
            var json = msgQ.Serialize();
            Assert.IsTrue(json.Contains("\"data\":\"foo\""), "data should be foo");

            msgQ = MsgQuery.Deserialize(input2) as MsgQuery;
            json = msgQ.Serialize();
            Assert.IsTrue(json.Contains("\"data\":1"), "data should be 1");
        }
    }
}
