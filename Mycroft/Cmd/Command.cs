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
            // Break the message body into the type token and the JSON blob,
            // then delegate to the specific command parser (MsgCmd.Parse(), AppCmd.Parse(), etc.)
            String type = getType(input);

            if (type != null)
            {
                String rawData = input.Substring(input.IndexOf('{'));
                Console.Write(rawData);
                Object data = getData(rawData);
                if (type == "MSG")
                {
                    return Msg.MsgCommand.Parse(type, data);
                }
                else if (type == "APP")
                {
                    return App.AppCommand.Parse(rawData, data);
                }
                else if (type == "SYS")
                {
                    return Sys.SysCommand.Parse(type, data);
                }
            }
            //TODO standardize
            return null;
        }
         
        public static String getType(String input)
        {
            // get type is in a new method for testing purposes
            if (input.Length >= 2)
            {
                return input.Substring(0, 3);
            }
            //malformed json
            //TODO standardize
            return null;
        }
        public static Object getData(String rawData)
        {
             var settings = new DataContractJsonSerializerSettings();
             settings.UseSimpleDictionaryFormat = true;
             var serializer = new DataContractJsonSerializer(typeof(Object), settings);
             Object data;
             var memStream = new MemoryStream(Encoding.UTF8.GetBytes(rawData));
             data = serializer.ReadObject(memStream) as Object;
             return data;
        }
    }
}
