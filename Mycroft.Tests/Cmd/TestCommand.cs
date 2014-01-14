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
            Command nullReturned = Command.Parse("");    
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
                Command.Parse(input);
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                throw new Exception("JSON could not be parsed into an object");
            }
        }
    }
    class BaseCommand : Command
    {
    }
}

