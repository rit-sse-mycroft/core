using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Messages.Msg;

namespace Mycroft.Messages.Test.Msg
{
    [TestClass]
    public class MsgQuerySuccessTest
    {
        [TestMethod]
        public void TestMsgQuerySuccessInvalidJson()
        {
            try
            {
                MsgQuerySuccess.Deserialize("{");
                Assert.Fail("01 should have thrown an exception");
            }
            catch (ParseException ex)
            {
                Assert.AreEqual("{", ex.Received);
            }

            try
            {
                MsgQuerySuccess.Deserialize("{}");
                Assert.Fail("02 should have thrown an exception");
            }
            catch (ParseException ex)
            {
                Assert.AreEqual("{}", ex.Received);
            }
        }
    }
}
