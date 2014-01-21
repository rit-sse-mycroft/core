using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Messages.App;

namespace Mycroft.Messages.Test.App
{
    [TestClass]
    public class AppInUseTest
    {
        string SampleMessage = @"
        {
            ""priority"" : 10
        }
        ";
        [TestMethod]
        public void TestAppInUseDeSerialization()
        {
            var appInUse = AppInUse.Deserialize(SampleMessage) as AppInUse;
            Assert.AreEqual(10, appInUse.Priority);
        }

        [TestMethod]
        public void TestAppInUseSerialization()
        {
            var appInUse = new AppInUse();
            appInUse.Priority = 10;

            string json = appInUse.Serialize();
            System.Diagnostics.Debug.WriteLine(json);
            Assert.IsTrue(json.IndexOf("\"priority\":10") > 0, "should have priority 10");
        }

        [TestMethod]
        public void TestAppInUseInvalidInput()
        {
            try
            {
                AppInUse.Deserialize("}");
                Assert.Fail("01 Should have thrown a ParseException");
            }
            catch (ParseException ex)
            {
                Assert.AreEqual("}", ex.Received);
            }

            try
            {
                AppInUse.Deserialize("{}");
                Assert.Fail("02 Should have thrown a ParseException");
            }
            catch (ParseException ex)
            {
                Assert.AreEqual("{}", ex.Received);
            }
        }
    }
}
