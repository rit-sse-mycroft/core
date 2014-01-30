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
using Mycroft.Messages;

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

            try
            {
                if (type != null)
                {
                    var startJson = input.IndexOf('{');
                    string rawData = startJson < 0 ? "" : input.Substring(startJson);

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
                throw new ParseException(input, "Unknown input");
            }
            catch (ParseException ex)
            {
                if (instance != null)
                {
                    var log = Logger.GetInstance();
                    log.Warning(String.Format(
                        "Failed to parse command from {0} for reason: {1}",
                        instance.InstanceId,
                        ex.Message
                    ));
                    log.Warning("Content received: " + input);
                }
                return new Msg.GeneralFailure(ex, instance);
            }
        }

        public static string getType(string input)
        {
            // Needs error handling?
            var firstSpace = input.IndexOf(" ");
            if(firstSpace < 0)
            {
                return input;
            }
            return input.Substring(0, firstSpace);
        }


        public virtual void VisitAppInstance(AppInstance appInstance) { }

        public virtual void VisitServer(TcpServer server) { }

        public virtual void VisitRegistry(Registry registry) { }

        public virtual void VisitMessageArchive(MessageArchive messageArchive) { }

        public virtual void VisitDispatcher(Dispatcher dispatcher) { }

    }
}
