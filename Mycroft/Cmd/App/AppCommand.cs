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
        public static Command Parse(String type, String json, AppInstance instance)
        {
            switch (type)
            {
                case "APP_UP":
                case "APP_DOWN":
                    return new DependencyChange(instance);
                case "APP_DESTROY":
                    return new Destroy(json, instance);
                case "APP_MANIFEST":
                    return Manifest.Parse(json, instance);
                default:
                    //data is incorrect - can't do anything with it
                    // TODO notify that is wrong
                    break;
            }
            return null ;
        }
    }
}
