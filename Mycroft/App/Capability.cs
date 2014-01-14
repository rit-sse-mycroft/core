using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.App
{
    /// <summary>
    /// An capability that represents a dependency between apps
    /// </summary>
    class Capability
    {
        /// <summary>
        /// The name of the capability
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The version of the capability
        /// </summary>
        public Version Version { get; set; }
    }
}
