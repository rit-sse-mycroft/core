using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.App;

namespace Mycroft.Tests.App
{
    [TestClass]
    public class TestDependency
    {
        [TestMethod]
        public void TestDependencyCreation()
        {
            var dep = new Dependency("foo", ">=1.2.3");
            Assert.AreEqual("foo", dep.InnerCapability.Name);
            Assert.AreEqual(Dependency.VersionRange.GreaterEqual, dep.Range);

            dep = new Dependency("foo", ">1.2.3");
            Assert.AreEqual(Dependency.VersionRange.Greater, dep.Range);

            dep = new Dependency("foo", "1.2.3");
            Assert.AreEqual(Dependency.VersionRange.Exact, dep.Range);

            dep = new Dependency("foo", "<1.2.3");
            Assert.AreEqual(Dependency.VersionRange.Less, dep.Range);

            dep = new Dependency("foo", "<=1.2.3");
            Assert.AreEqual(Dependency.VersionRange.LessEqual, dep.Range);
        }

        [TestMethod]
        public void TestIncorrectDependencyCreation()
        {
            try
            {
                new Dependency("foo", "?1.2.3");
                Assert.Fail("01 should have thrown exception");
            }
            catch (FormatException) { }

            try
            {
                new Dependency("foo", "1..2");
                Assert.Fail("02 should have thrown exception");
            }
            catch (FormatException) { }

        }

        [TestMethod]
        public void TestDependencyMatching()
        {
            var dep = new Dependency("foo", ">=1.2.3");
            Assert.AreEqual("foo", dep.InnerCapability.Name);

            // test equals
            var cap = new Capability("foo", new Version("1.2.3"));
            Assert.IsTrue(dep.Matches(cap), "01 capability should match");

            // test wrong name
            cap = new Capability("bar", new Version("1.2.3"));
            Assert.IsFalse(dep.Matches(cap), "02 should not match");

            // test greater capability
            cap = new Capability("foo", new Version("2.0.0"));
            Assert.IsTrue(dep.Matches(cap), "03 should match");

            // test less capability
            cap = new Capability("foo", new Version("1.0.0"));
            Assert.IsFalse(dep.Matches(cap), "04 should not match");

            dep = new Dependency("foo", "<=1.2.3");

            // test greater capability
            cap = new Capability("foo", new Version("2.0.0"));
            Assert.IsFalse(dep.Matches(cap), "05 should not match");

            // test less capability
            cap = new Capability("foo", new Version("1.0.0"));
            Assert.IsTrue(dep.Matches(cap), "06 should match");

            dep = new Dependency("foo", "1.2.3");

            // test not exact match
            cap = new Capability("foo", new Version("1.0.0"));
            Assert.IsFalse(dep.Matches(cap), "07 should not match");

            // test exact match
            cap = new Capability("foo", new Version("1.2.3"));
            Assert.IsTrue(dep.Matches(cap), "08 should not match");

        }
    }
}
