using Mycroft.App;
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
        public static Command Parse(String type, String rawData, Object data, AppInstance instance)
        {
        switch (type)
        {
            case "APP_UP":
                Up.up(instance.InstanceId);
                break;
            case "APP_DOWN":
                Down.down(instance.InstanceId);
                break;
            case "APP_CREATE":
                Create.create(instance.InstanceId);
                break;
            case "APP_DESTROY":
                Destroy.destroy(instance.InstanceId);
                break;
            default:
                //data is incorrect - can't do anything with it
                // TODO notify that is wrong
                break;
            }
        return null ;
        }
    }
}
