using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Cmd.Msg;

namespace Mycroft.Tests.Cmd.Msg
{
    [TestClass]
    public class TestMsgCommand
    {
        [TestMethod]
        public void TestMethod1()
        {
            //base case - shouldn't break with blank data
            Object fooData = null;
            String fooString = "";
            MsgCommand.Parse(fooString, fooString, fooData, null);

            //command for message command should be called
            String appUpString = @"MSG_COMMAND {""foo""";
            MsgCommand.Parse(fooString, appUpString, fooData, null);
        }
    }
}
