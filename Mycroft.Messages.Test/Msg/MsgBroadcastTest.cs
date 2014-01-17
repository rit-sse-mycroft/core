using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Web.Helpers;
using Mycroft.Messages.Msg;

namespace Mycroft.Messages.Test.Msg
{
    [TestClass]
    public class MsgBroadcastTest
    {
        private string SampleQuery = @"
            {
                ""id"" : ""uuid"",
                ""content"" : {
                    ""text"": ""pickle unicorn"",
                    ""thing"": {
                        ""other"" : ""blah""
                    }
                }
            }";

        [TestMethod]
        public void TestMsgBroadcastSerialization()
        {
            var msgBroadcast = new MsgBroadcast();
            msgBroadcast.Id = "uuid";
            var dct = new Dictionary<string, object>();
            dct.Add("text", "pickle unicorn");
            var innerDct = new Dictionary<string, object>();
            innerDct.Add("other", "blah");
            dct.Add("thing", innerDct);
            msgBroadcast.Content = dct;

            string json = msgBroadcast.Seralize();
            System.Diagnostics.Debug.WriteLine(json);

            Assert.IsTrue(json.IndexOf("\"other\":\"blah\"") > 0, "should have inner dict content");
        }

        [TestMethod]
        public void TestMsgBroadcastDeserialization()
        {
            var msgBroadcast = MsgBroadcast.DeSeralize(SampleQuery) as MsgBroadcast;
            Assert.AreEqual("uuid", msgBroadcast.Id, "should have correct uuid");
            Assert.AreNotEqual(null, msgBroadcast.Content as object, "should have content");
        }
    }
}