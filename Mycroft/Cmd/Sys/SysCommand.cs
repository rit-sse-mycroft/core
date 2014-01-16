using Mycroft.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Cmd.Sys
{
    class SysCommand : Command
    {
        /// <summary>
        /// Parses JSON into system command objects
        /// </summary>
        /// <param name="messageType">The message type that determines the command to create</param>
        /// <param name="json">The JSON body of the message</param>
        /// <returns>Returns a command object for the parsed message</returns>
        public static Command Parse(String type, String rawData, Object data, AppInstance instance)
        { 
            switch (type)
            {
                case "SYS_KILLAPP":
                    KillAll.killAll(instance.InstanceId);
                    break;
                case "SYS_SHUTDOWN":
                    ShutOff.shutOff(instance.InstanceId);
                    break;
                case "SYS_LOCKDOWN":
                    Lockdown.lockdown(instance.InstanceId);
                    break;
                case "SYS_UNLOCK":
                    SysUnlock.unlock(instance.InstanceId);
                    break;
                default:
                    //TODO: notify if data does not conform
                    break;
            }
            return null;
        }
    }
}
