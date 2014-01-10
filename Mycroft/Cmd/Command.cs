using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

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
            int index = input.IndexOf('{');
            String type;
            Object data;
            if (index >= 0)
            {
                type = input.Substring(0, index);
                String rawData = input.Substring(index + 1);
                var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(rawData));
                data = ser.ReadObject(memoryStream);
            }
            else
            {
                type = input; /// no body was supplied
            }
            if(type == "") /// malformed message
                           /// TODO alert client
            {

            }
            // Break the message body into the type token and the JSON blob,
            // then delegate to the specific command parser (MsgCmd.Parse(), AppCmd.Parse(), etc.)
            return null;
        }
    }
}
