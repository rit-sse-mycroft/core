using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Cmd;
using Mycroft.Cmd.App;

namespace Mycroft.Tests.Cmd.App
{
    [TestClass]
    public class TestAppCommand
    {
        [TestMethod]
        public void TestAppCommandParse()
        {
            //base case - shouldn't break with blank data
            Object fooData = null;
            String fooString = "";
            AppCommand.Parse(fooString, fooString, fooData, fooString);

            //command for app up should be called
            String appUpString = @"APP_UP {""foo""";
            AppCommand.Parse(fooString, appUpString, fooData, fooString);

        }
    }
}
