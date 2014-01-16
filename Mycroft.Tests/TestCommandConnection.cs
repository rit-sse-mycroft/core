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
        public async Task TestBodylessMessage(){
            var s = new MemoryStream(Encoding.UTF8.GetBytes("6\nAPP_UP"));
            var cmd = new CommandConnection(s);
            var msg = await cmd.GetCommandAsync();
            Trace.WriteLine(msg);
            if (msg != "APP_UP")
                throw new Exception("Incorrect message!");
        }

        [TestMethod]
        public async Task TestBodaciousMessage()
        {
            var input = "30\nMSG_BROADCAST {\"key\": \"value\"}";
            var s = new MemoryStream(Encoding.UTF8.GetBytes(input));
            var cmd = new CommandConnection(s);
            var msg = await cmd.GetCommandAsync();
            Trace.WriteLine(msg);
            Trace.WriteLine(input.Substring(3));
            if (msg != input.Substring(3))
                throw new Exception("Incorrect message!");
        }
           
    }
}
