using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mycroft.Cmd;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Mycroft.Tests.Cmd
{
    [TestClass]
    public class TestCommand
    {
        [TestMethod]
        public void TestParse()
        {
            // sample input taken from the wiki
            var input = @"MSG_QUERY : {
                ""id"": ""uuid"",
                ""capability"": ""weather"",
                ""remoteProcedure"": ""get_temperature"",
                ""args"" : [""farenheit""],
                ""instanceId"":[""xxxx""],
                ""priority"": 30           
            }}";
            
            // JSON of "MSG_QUERY" should return "MSG"
            String msgQuery = Command.getType(input);
            Assert.AreEqual("MSG_QUERY", msgQuery, "Get type should return 'MSG_QUERY' ");
            try
            {
                // if this breaks, errors could lie in MSG command etc
                Command.Parse(input, null);
            }
            catch (SerializationException e)
            {
                throw new Exception("JSON could not be parsed into an object - serialization error", e);
            }
            
            //random input should return null because objects name is incorrect
            var input1 = @"CTL_FOOBAR : {
                ""id"": ""uuid"",
                ""capability"": ""weather"",
                ""remoteProcedure"": ""get_temperature"",
                ""args"" : [""farenheit""],
                ""instanceId"":[""xxxx""],
                ""priority"": 30           
            }}";
            Command returned = Command.Parse(input1, null);
            Assert.AreEqual(null, returned, "An incorrect class name should return a null value");
        }
    }
    class BaseCommand : Command
    {
    }
}

