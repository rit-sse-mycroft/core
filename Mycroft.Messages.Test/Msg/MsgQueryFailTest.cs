using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Messages.Msg;

namespace Mycroft.Messages.Test.Msg
{
    [TestClass]
    public class MsgQueryFailTest
    {
        [TestMethod]
        public void TestMsgQueryFailInvalidJson()
        {
            try
            {
                MsgQueryFail.Deserialize("{");
                Assert.Fail("01 should have thrown exception");
            }
            catch (ParseException ex)
            {
                Assert.AreEqual("{", ex.Received);
            }

            try
            {
                MsgQueryFail.Deserialize("{}");
                Assert.Fail("02 should have thrown exception");
            }
            catch (ParseException ex)
            {
                Assert.AreEqual("{}", ex.Received);
            }
        }
    }
}
