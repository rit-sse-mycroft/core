using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Cmd
{
    abstract class Command
    {
        /// <summary>
        /// Parses a Mycroft command from a JSON object
        /// </summary>
        /// <returns>
        /// Returns the Command object that needs to be routed through the system
        /// </returns>
        public static Command Parse(String input)
        {
            // Break the message body into the type token and the JSON blob,
            // then delegate to the specific command parser (MsgCmd.Parse(), AppCmd.Parse(), etc.)
            return null;
        }
    }
}
