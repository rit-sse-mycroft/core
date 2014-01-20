using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.App;
using System.Collections.Generic;

namespace Mycroft.Tests.App
{
    [TestClass]
    public class TestCapability
    {
        /// <summary>
        /// Tests that capabilities are correctly sorted by name
        /// </summary>
        [TestMethod]
        public void TestCompareTo_Name()
        {
            var a = new Capability("capA", new Version("1.0"));
            var b = new Capability("capB", new Version("1.0"));

            Assert.IsTrue(a.CompareTo(b) < 0);
            Assert.IsTrue(b.CompareTo(a) > 0);
        }

        /// <summary>
        /// Tests that capabilities with the same name are sorted by version
        /// </summary>
        [TestMethod]
        public void TestCompareTo_Version()
        {
            var a = new Capability("capA", new Version("1.0"));
            var b = new Capability("capA", new Version("1.1"));

            Assert.IsTrue(a.CompareTo(b) < 0);
            Assert.IsTrue(b.CompareTo(a) > 0);
        }

        /// <summary>
        /// Tests that capabilities can be equal
        /// </summary>
        [TestMethod]
        public void TestCompareTo_Equal()
        {
            var a = new Capability("capA", new Version("1.0"));
            var b = new Capability("capA", new Version("1.0"));

            Assert.IsTrue(a.CompareTo(b) == 0);
            Assert.IsTrue(a.Equals(b));
        }

        /// <summary>
        /// Tests that capabilities are sorted correctly
        /// </summary>
        [TestMethod]
        public void TestCompareTo_Sort()
        {
            var a = new Capability("capA", new Version("1.0"));
            var b = new Capability("capB", new Version("1.0"));
            var c = new Capability("capB", new Version("1.1"));

            var list = new List<Capability>() { c, b, a };
            list.Sort();

            Assert.AreSame(a, list[0]);
            Assert.AreSame(b, list[1]);
            Assert.AreSame(c, list[2]);
        }

    }
}
