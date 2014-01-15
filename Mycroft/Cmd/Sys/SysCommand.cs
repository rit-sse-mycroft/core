﻿using System;
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
        public static Command Parse(String type, String rawData, Object data, String instanceId)
        {
          
            switch (type)
            {
                case "SYS_KILLAPP":
                    //do kill app stuff
                    KillAll.SysKillAll.killAll(instanceId);
                    break;
                case "SYS_SHUTDOWN":
                    //do shutdown stuff
                    ShutOff.SysShutOff.shutOff(instanceId);
                    break;
                case "SYS_LOCKDOWN":
                    //do lockdown stuff
                    Lockdown.SysLockdown.lockdown(instanceId);
                    break;
                case "SYS_UNLOCK":
                    //do unlock stuff
                    Unlock.SysUnlock.unlock(instanceId);
                    break;
                default:
                    //data is incorrect - can't do anything with it
                    break;
            }
            
        return null;
        }
    }
}
