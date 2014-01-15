using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Cmd.App
{
    class AppCommand : Command
    {
        /// <summary>
        /// Parses JSON into App command objects
        /// </summary>
        /// <param name="messageType">The message type that determines the command to create</param>
        /// <param name="json">The JSON body of the message</param>
        /// <returns>Returns a command object for the parsed message</returns>
        public static Command Parse(String rawData, Object data)
        {
            if(rawData.Contains("{"))
            {
                String appCommandType = rawData.Substring(0, rawData.IndexOf("{") - 1);
                switch(appCommandType)
                {
                    case "APP_UP":
                        //do app up stuff
                        break;
                    case "APP_DOWN":
                        //do app down stuff
                        break;
                    case "APP_CREATE":
                        //do app create stuff
                        break;
                    case "APP_DESTROY":
                        //do app destroy stuff
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
            return null ;
        }
    }
}
