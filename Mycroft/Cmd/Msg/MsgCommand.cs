using Mycroft.App;
using Mycroft.Messages.Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Cmd.Msg
{
   public class MsgCommand : Command
    {
        /// <summary>
        /// Parses JSON into message command objects
        /// </summary>
        /// <param name="messageType">The message type that determines the command to create</param>
        /// <param name="json">The JSON body of the message</param>
        /// <returns>Returns a command object for the parsed message</returns>
        public static Command Parse(String type, String rawData, AppInstance instance)
        {
            switch (type)
            {
                case "MSG_BROADCAST":
                    return new Broadcast(rawData, instance);
                case "MSG_QUERY":
                    return Query.Parse(rawData, instance);
                case "MSG_QUERY_SUCCESS":
                    return new Reply(rawData, instance);
                case "MSG_QUERY_FAIL":
                    return new QueryFail(rawData, instance);
                default:
                    //TODO: notify if data does not meet format
                    break;
            }
            return null;
        }

        public String guid { get; set; }
        public AppInstance FromInstance { get; protected set; }

    }
}
