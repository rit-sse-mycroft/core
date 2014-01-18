﻿using System;
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
            MsgQuery msgQuery = MsgQuery.DeSerialize(SampleQuery) as MsgQuery;

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
    }
}
