using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Mycroft.Cmd;

namespace Mycroft.Cmd
{
    abstract class Command
    {
        /// <summary>
        /// Parses a Mycroft command from a JSON object
        /// </summary>
        /// <returns>
        /// Returns the Command object that needs to be routed through the system
        /// </returns>
        public static Command Parse(String input)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Dictionary<String, Object>)); 
            String type = getType(input);
            Object data;           
            String rawData = input.Substring(input.IndexOf('{') + 1);
            if (type != null)
            {
                
            var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(rawData));
            data = ser.ReadObject(memoryStream);
            if (type == "MsgCmd")
            {
                return Msg.MsgCommand.Parse(type, rawData);
            }
            else if (type == "AppCmd")
            {
                return App.AppCommand.Parse(type, rawData);
            }
            else if (type == "SysCmd")
            {
                return Sys.SysCommand.Parse(type, rawData);
            }
            }
            return null;
        }
            // Break the message body into the type token and the JSON blob,
            // then delegate to the specific command parser (MsgCmd.Parse(), AppCmd.Parse(), etc.)
         
        public static String getType(String input)
        {
            // get type is in a new method for testing purposes
            if (input.Length >= 2)
            {
                return input.Substring(0, 2);
            }
            //malformed json
            return null;
        }
    }
}
