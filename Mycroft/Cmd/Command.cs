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
            String type = getType(input);
            Object data;           
            String rawData = input.Substring(input.IndexOf('{') + 1);
            if (type != null)
            {
                data = getData(rawData);
                if (type == "MsgCmd")
                {
                    return Msg.MsgCommand.Parse(type, data);
                }
                else if (type == "AppCmd")
                {
                    return App.AppCommand.Parse(type, data);
                }
                else if (type == "SysCmd")
                {
                    return Sys.SysCommand.Parse(type, data);
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
                return input.Substring(0, 3);
            }
            //malformed json
            return null;
        }
        public static Object getData(String rawData)
        {
             DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Dictionary<String, Object>)); 
             var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(rawData));
             return ser.ReadObject(memoryStream);
            
        }
    }
}
