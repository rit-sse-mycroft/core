using Mycroft.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Cmd.Msg
{
    class MsgCommand : Command
    {
        /// <summary>
        /// Parses JSON into message command objects
        /// </summary>
        /// <param name="messageType">The message type that determines the command to create</param>
        /// <param name="json">The JSON body of the message</param>
        /// <returns>Returns a command object for the parsed message</returns>
        public static Command Parse(String type, String rawData, Object data, AppInstance instance)
        {
            switch (type)
            {
                case "MSG_BROADCAST":
                    Broadcast.broadcast(instance.InstanceId);
                    break;
                case "MSG_QUERY":
                    Query.query(instance.InstanceId);
                    break;
                case "MSG_DIRECTQUERY":
                    DirectQuery.directQuery(instance.InstanceId);
                    break;
                case "MSG_REPLY":
                    Reply.reply(instance.InstanceId);
                    break;
                case "MSG_REQUEST":
                    Request.request(instance.InstanceId);
                    break;
                default:
                    //TODO: notify if data does not meet format
                break;
            }
            return null;
        }

        public String guid { get; set; }
    }
}
