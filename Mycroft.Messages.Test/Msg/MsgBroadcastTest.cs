using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace Mycroft.Messages.Test.Msg
{
    [TestClass]
    public class MsgBroadcastTest 
    {
        private string SampleQuery = @"
            MSG_BROADCAST {
                ""id"" : ""uuid"",
                ""content"" : {
                    ""text"": ""pickle unicorn""
                }
            }";

        [TestMethod]
        public void TestMsgBroadcastSerialization()
        {
        }

        [TestMethod]
        public void TestMsgBroadcastDeserialization()
        {
        
        }
    }
}
