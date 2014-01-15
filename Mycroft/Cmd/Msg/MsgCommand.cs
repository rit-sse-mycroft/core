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
        public static Command Parse(String rawData, Object data)
        {
            if (rawData.Contains("{"))
            {
                String appCommandType = rawData.Substring(0, rawData.IndexOf("{") - 1);
                switch (appCommandType)
                {
                    case "MSG_BROADCAST":
                        //do message broadcast stuff
                        break;
                    case "MSG_QUERY":
                        //do message query stuff
                        break;
                    case "MSG_DIRECTQUERY":
                        //do message direct query stuff
                        break;
                    case "MSG_REPLY":
                        //do message reply stuff
                        break;
                    case "MSG_REQUEST":
                        //do message request stuff
                        break;
                     default:
                    //data is incorrect - can't do anything with it
                       break;
                }
            }
            else
            {
                //notify that data cannot be used
            }
            return null;
        }

        public String guid { get; set; }
    }
}
