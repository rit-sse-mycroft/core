using Mycroft.App.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.App
{
    /// <summary>
    /// Represents an instance of an app that is connected to Mycroft
    /// </summary>
    class AppInstance
    {
        /// <summary>
        /// The name of the app that's running.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// The pretty name of the app
        /// </summary>
        public String DisplayName { get; private set; }

        /// <summary>
        /// The unique instance ID of the app
        /// </summary>
        public String InstanceId { get; private set; }

        /// <summary>
        /// The Mycroft API version the app expects
        /// </summary>
        public uint? ApiVersion { get; private set; }

        /// <summary>
        /// The current status of the app
        /// </summary>
        public Status Status { get; private set; }

        /// <summary>
        /// The version of the app
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Connection, managed through the State pattern
        /// </summary>
        private State connectionState;


        public AppInstance()
        {
            connectionState = new ConnectedState();
        }

    }
}
