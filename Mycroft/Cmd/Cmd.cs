using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Cmd
{
    abstract class Cmd
    {
        /// <summary>
        /// Parses a Mycroft command from a JSON object
        /// </summary>
        /// <returns>
        /// Returns the Command object that needs to be routed through the system
        /// </returns>
        public static Cmd Parse()
        {
            return null;
        }
    }
}
