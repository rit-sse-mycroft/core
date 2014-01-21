using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Cmd.Msg;
using System.Threading;

namespace Mycroft.Tests.Cmd.Msg
{
    [TestClass]
    public class TestMessageArchive
    {
        MessageArchive archive;
        MsgCommand testMsg;

        [TestInitialize]
        public void setup()
        {
            int hours = 0;
            int minutes = 0;
            int seconds = 1;


            archive = new MessageArchive(hours,minutes,seconds);
            testMsg = new StubMsgCommand();
            testMsg.guid = "TESTGUIDBECAUSSEITSASTRING";
        }


        [TestMethod]
        public void TestinCache()
        {
            archive.TryPostMessage(testMsg);
            Assert.AreSame(testMsg,archive[testMsg.guid],"tests you get the same object back from the archive");
        }



        [TestMethod]
        public void TestCacheTimeout()
        {
            archive.TryPostMessage(testMsg);

            Thread.Sleep(2000);

            var tmpmsg = archive[testMsg.guid];

            Assert.IsNull(tmpmsg);
            //Assert.AreSame(testMsg, tmpmsg, "tests you should not get the same object back from the archive after the cache expires");
        }



    }


    class StubMsgCommand : MsgCommand
    {

    }
    



}
