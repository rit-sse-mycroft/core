using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Cmd.Sys;

namespace Mycroft.Tests.Cmd.Sys
{
    [TestClass]
    public class TestSysCommand
    {
        [TestMethod]
        public void TestMethod1()
        {
            //base case - shouldn't break with blank data
            Object fooData = null;
            String fooString = "";
            SysCommand.Parse(fooString, fooString, fooData, null);

            //command for message command should be called
            String appUpString = @"SYS_KILLAPP {""foo""";
            SysCommand.Parse(fooString, appUpString, fooData, null);
        }
    }
}
