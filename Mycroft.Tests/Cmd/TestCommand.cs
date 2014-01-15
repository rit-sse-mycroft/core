using System;
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
            Command nullReturned = Command.Parse("", "");    
            Assert.AreEqual(null, nullReturned, "Should return null");

            
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
            Assert.AreEqual(msgQuery, "MSG", "Get type should return 'MSG' ");
            try
            {
                // if this breaks, errors could lie in MSG command etc
                Command.Parse(input, "1234");
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                throw new Exception("JSON could not be parsed into an object");
            }
            
            //random input should return null
            var input1 = @"CTL_FOOBAR : {
                ""id"": ""uuid"",
                ""capability"": ""weather"",
                ""remoteProcedure"": ""get_temperature"",
                ""args"" : [""farenheit""],
                ""instanceId"":[""xxxx""],
                ""priority"": 30           
            }}";
            Command returned = Command.Parse(input1, "1234");
            Assert.AreEqual(null, returned, "A random input should return a null value");
        }
    }
    class BaseCommand : Command
    {
    }
}

