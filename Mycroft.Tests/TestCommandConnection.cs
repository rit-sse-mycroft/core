using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mycroft.App;
using System.IO;
using System.Diagnostics;

namespace Mycroft.Tests
{
    [TestClass]
    public class TestCommandConnection
    {
        [TestMethod]
        public void TestBodylessMessage(){
            var s = new MemoryStream(Encoding.UTF8.GetBytes("6\nAPP_UP"));
            var cmd = new CommandConnection(s);
            var msg = cmd.GetCommand();
            Trace.WriteLine(msg);
            Assert.AreEqual(msg, "APP_UP");
        }

        [TestMethod]
        public void TestBodaciousMessage()
        {
            var input = "30\nMSG_BROADCAST {\"key\": \"value\"}";
            var s = new MemoryStream(Encoding.UTF8.GetBytes(input));
            var cmd = new CommandConnection(s);
            var msg = cmd.GetCommand();
            Trace.WriteLine(msg);
            Trace.WriteLine(input.Substring(3));
            Assert.AreEqual(msg, input.Substring(3));
        }
           
    }
}
