using Mycroft.Cmd;
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
    public class AppInstance : ICommandable
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
        public String InstanceId { get; internal set; }

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
        public Version Version { get; private set; }

        /// <summary>
        /// Whether the app is registered and usable by the system
        /// </summary>
        public bool IsRegistered { get { return state.IsRegistered; } }

        /// <summary>
        /// Current status in being registered with the system
        /// </summary>
        private State state;

        /// <summary>
        /// The connection object that reads messages
        /// </summary>
        private CommandConnection connection;

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
            connection = new CommandConnection(stream);
            state = new ConnectedState(this, dispatcher);
            InstanceId = new Guid().ToString();
        }

        /// <summary>
        /// Starts the AppInstance listening to things
        /// </summary>
        public void Listen()
        {
            while (true)
            {
                Task<string> messageTask = connection.GetCommandAsync();
                messageTask.Wait();
                var message = messageTask.Result;

                // Make this command visit this instance before doing anything else
                var command = Command.Parse(message, this);
                dispatcher.Enqueue(command);
            }
        }

        /// <summary>
        /// Allow the AppInstance to be visited by commands
        /// </summary>
        /// <param name="command">The command that will operate on the AppInstance</param>
        public void Issue(Command command)
        {
            command.visitAppInstance(this);
        }

        /// <summary>
        /// Changes the State object used by the app instance
        /// </summary>
        /// <param name="newState">The new state that will be used</param>
        public void TransitionState(State newState)
        {
            state = newState;
        }

    }
}
