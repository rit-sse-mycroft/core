using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Messages.Msg;

namespace Mycroft.Messages.Test.Msg
{
    [TestClass]
    public class MsgGeneralFailureTest
    {

        [TestMethod]
        public void TestMsgGeneralFailureSerialization()
        {
            string target = "{\"received\":\"APP_MANFST {}\",\"message\":\"this is a message\"}";
            var msg = new MsgGeneralFailure();
            msg.Received = "APP_MANFST {}";
            msg.Message = "this is a message";
            string json = msg.Serialize();
            Assert.AreEqual(target, json);
        }

        [TestMethod]
        public void TestMsgGeneralFailureDeserialization()
        {
            string sampleMessage = @"
            {
              ""received"" : ""APP_MANFST {}"",
              ""message"" : ""this is a message""
            }
            ";
            var msg = MsgGeneralFailure.Deserialize(sampleMessage) as MsgGeneralFailure;
            Assert.AreEqual("APP_MANFST {}", msg.Received);
            Assert.AreEqual("this is a message", msg.Message);
        }
    }
}
