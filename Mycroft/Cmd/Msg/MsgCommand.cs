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
        public static Command Parse(String type, String rawData, Object data, String instanceId)
        {

            switch (type)
            {
                case "MSG_BROADCAST":
                    //do message broadcast stuff
                    Broadcast.MsgBroadcast.broadcast(instanceId);
                    break;
                case "MSG_QUERY":
                    //do message query stuff
                    Query.MsgQuery.query(instanceId);
                    break;
                case "MSG_DIRECTQUERY":
                    //do message direct query stuff
                    DirectQuery.MsgDirectQuery.directQuery(instanceId);
                    break;
                case "MSG_REPLY":
                    //do message reply stuff
                    Reply.MsgReply.reply(instanceId);
                    break;
                case "MSG_REQUEST":
                    //do message request stuff
                    Request.MsgRequest.request(instanceId);
                    break;
                    default:
                //data is incorrect - can't do anything with it
                    break;
            }
        
 
            return null;
        }
    }
}
