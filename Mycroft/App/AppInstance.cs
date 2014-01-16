using Mycroft.Cmd;
using Mycroft.Cmd.App;
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
        public Status AppStatus { get; private set; }

        /// <summary>
        /// The version of the app
        /// </summary>
        public Version Version { get; private set; }

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
        /// Indicates that we should be listening for messages
        /// </summary>
        private volatile bool listening;

        private Object instanceLock = new Object();

        /// <summary>
        /// Set up a null AppInstance. An InstanceId is assigned to a new GUID, which
        /// will be reset if the instance sends a different ID in its manifest.
        /// </summary>
        public AppInstance(Stream stream, Dispatcher dispatcher)
        {
            this.stream = stream;
            this.dispatcher = dispatcher;
            connection = new CommandConnection(stream);
            InstanceId = new Guid().ToString();
            AppStatus = Status.Connected;
            listening = false;
        }

        /// <summary>
        /// Starts the AppInstance listening to things
        /// </summary>
        public void Listen()
        {
            listening = true;
            while (listening)
            {
                Task<string> messageTask = connection.GetCommandAsync();
                messageTask.Wait();
                lock (instanceLock)
                {
                    var message = messageTask.Result;

                    // Make this command visit this instance before doing anything else
                    var command = Command.Parse(message, this);
                    if (CanUse(command))
                    {
                        dispatcher.Enqueue(command);
                    }
                }
            }
        }

        /// <summary>
        /// Allow the AppInstance to be visited by commands
        /// </summary>
        /// <param name="command">The command that will operate on the AppInstance</param>
        public void Issue(Command command)
        {
            lock (instanceLock)
            {
                command.visitAppInstance(this);
            }
        }

        /// <summary>
        /// Shuts down the app instance thread
        /// </summary>
        public void Disconnect()
        {
            lock(instanceLock){
                listening = false;
            }
        }

        /// <summary>
        /// Checks if a command is valid to have received in the app's current state
        /// </summary>
        /// <param name="cmd">The command that was received </param>
        /// <returns>Returns true if the command is valid for the current state, false otherwise</returns>
        private bool CanUse(Command cmd)
        {
            lock (instanceLock)
            {
                switch (AppStatus)
                {
                    case Status.Connected:
                        return (cmd is Manifest);

                    case Status.Active:
                    case Status.Inactive:
                    case Status.InUse:
                        return true;
                }
            }
            return true;
        }
    }
}
