using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Mycroft.Cmd;
using Mycroft.App;
using Mycroft.Cmd.Sys;
using Mycroft.Cmd.App;
using Mycroft.Cmd.Msg;
using Mycroft.Server;

namespace Mycroft.Cmd
{
    public abstract class Command
    {
        /// <summary>
        /// Parses a Mycroft command from a JSON object
        /// </summary>
        /// <returns>
        /// Returns the Command object that needs to be routed through the system
        /// </returns>
        public static Command Parse(String input, AppInstance instance)
        {
            // TODO error handling - catch exceptions, then create a new Command
            // that contains the error to send back

            // Break the message body into the type token and the JSON blob,
            // then delegate to the specific command parser (MsgCmd.Parse(), AppCmd.Parse(), etc.)
            String type = getType(input);
            if (type != null)
            {
                String rawData = input.Substring(input.IndexOf('{'));
                Console.Write(rawData);
                if (type.StartsWith("MSG"))
                {
                    return MsgCommand.Parse(type, rawData, instance);
                }
                else if (type.StartsWith("APP"))
                {
                    return AppCommand.Parse(type, rawData, instance);
                }
                else if (type.StartsWith("SYS"))
                {
                    return SysCommand.Parse(type, rawData, instance);
                }
            }
            //TODO standardize
            return null;
        }

        public static String getType(String input)
        {
            // get type is in a new method for testing purposes
            if (input.Length >= 3)
            {
                return input.Substring(0, 3);
            }
            //malformed json
            //TODO standardize
            return null;
        }


        public virtual void visitAppInstance(AppInstance appInstance)
        {

        }

        public virtual void visitServer(TcpServer server)
        {

        }



    }
}
