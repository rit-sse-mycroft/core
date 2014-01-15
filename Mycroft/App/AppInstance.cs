using Mycroft.App.Connection;
using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// Dispatches messages through the system once received
        /// </summary>
        private Dispatcher dispatcher;

        /// <summary>
        /// Input stream that receives messages from the app instance
        /// </summary>
        private Stream stream;

        /// <summary>
        /// Set up a null AppInstance. An InstanceId is assigned to a new GUID, which
        /// will be reset if the instance sends a different ID in its manifest.
        /// </summary>
        public AppInstance(Stream stream, Dispatcher dispatcher)
        {
            this.stream = stream;
            this.dispatcher = dispatcher;
            connectionState = new ConnectedState();
            InstanceId = new Guid().ToString();
        }

    }
}
