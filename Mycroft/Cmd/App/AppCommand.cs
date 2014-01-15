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
        public static Command Parse(String type, String rawData, Object data)
        {
            if(rawData.Contains("{"))
            {
                switch(type)
                {
                    case "APP_UP":
                        //do app up stuff
                        Up.AppUp.up();
                        break;
                    case "APP_DOWN":
                        //do app down stuff
                        Down.AppDown.down();
                        break;
                    case "APP_CREATE":
                        //do app create stuff
                        Create.AppCreate.create();
                        break;
                    case "APP_DESTROY":
                        //do app destroy stuff
                        Destroy.AppDestroy.destroy();
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
