﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Cmd;
using System.IO;

namespace Mycroft.Tests.Cmd
{
    [TestClass]
    public class TestCommand
    {
        [TestMethod]
        public void TestParse()
        {
            // Blank manifest should return null
            Command nullReturned = Command.Parse("");    
            Assert.AreEqual(null, nullReturned, "Should return null");

            // JSON of "MSG_QUERY" should return "MSG"
            String msg_query = Command.getType("MSG_BROADCAST { foo : foobar }");
        }
    }
    class BaseCommand : Command
    {
    }
}

