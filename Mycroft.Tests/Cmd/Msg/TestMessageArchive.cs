using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Cmd.Msg;

namespace Mycroft.Tests.Cmd.Msg
{
    [TestClass]
    public class TestMessageArchive
    {
        MessageArchive archive;

        [TestInitialize]
        public void setup()
        {
            archive = new MessageArchive();
        }


        [TestMethod]
        public void TestIO()
        {

            //M

            //archive.PostMessage();



        }
    }
}
