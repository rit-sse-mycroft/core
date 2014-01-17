using Mycroft.Cmd;
using Mycroft.Cmd.App;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mycroft.App
{
    /// <summary>
    /// Represents an instance of an app that is connected to Mycroft
    /// </summary>
    public class AppInstance : ICommandable
    {
        private ReaderWriterLockSlim readwrite = new ReaderWriterLockSlim();

        /// <summary>
        /// The name of the app that's running.
        /// </summary>
        public String Name
        {
            get { return Read(() => _name); }
            private set { Write(() => _name = value); }
        }
        private String _name;

        /// <summary>
        /// The pretty name of the app
        /// </summary>
        public String DisplayName
        {
            get { return Read(() => _displayName); }
            private set { Write(() => _displayName = value); }
        }
        private String _displayName;

        /// <summary>
        /// The unique instance ID of the app
        /// </summary>
        public String InstanceId
        {
            get { return Read(() => _instanceId); }
            private set { Write(() => _instanceId = value); }
        }
        private String _instanceId;

        /// <summary>
        /// The Mycroft API version the app expects
        /// </summary>
        public uint? ApiVersion
        {
            get { return Read(() => _apiVersion); }
            private set { Write(() => _apiVersion = value); }
        }
        private uint? _apiVersion;

        /// <summary>
        /// The current status of the app
        /// </summary>
        public Status AppStatus
        {
            get { return Read(() => _status); }
            private set { Write(() => _status = value); }
        }
        private Status _status;

        /// <summary>
        /// The version of the app
        /// </summary>
        public Version Version
        {
            get { return Read(() => _version); }
            private set { Write(() => _version = value); }
        }
        private Version _version;

        /// <summary>
        /// The connection object that reads messages
        /// </summary>
        private readonly CommandConnection connection;

        /// <summary>
        /// Dispatches messages through the system once received
        /// </summary>
        private readonly Dispatcher dispatcher;

        /// <summary>
        /// Indicates that we should be listening for messages
        /// </summary>
        /// <remarks>Should always be accessed under a read/write lock</remarks>
        private volatile bool listening;

        /// <summary>
        /// Set up a null AppInstance. An InstanceId is assigned to a new GUID, which
        /// will be reset if the instance sends a different ID in its manifest.
        /// </summary>
        public AppInstance(Stream stream, Dispatcher dispatcher)
        {
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
            // Set that we're listening for commands
            Write(() => listening = true );

            while (Read(() => listening))
            {
                var message = connection.GetCommand();

                Debug.WriteLine("Message received by AppInstance " + InstanceId);
                Debug.WriteLine(message);

                // Make this command visit this instance before doing anything else
                var command = Command.Parse(message, this);
                if (CanUse(command))
                {
                    dispatcher.Enqueue(command);
                }
            }
        }

        /// <summary>
        /// Allow the AppInstance to be visited by commands
        /// </summary>
        /// <param name="command">The command that will operate on the AppInstance</param>
        public void Issue(Command command)
        {
            command.VisitAppInstance(this);
        }

        /// <summary>
        /// Sends a message to the connected app
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            Task writeTask = connection.SendMessageAsync(message);
        }

        /// <summary>
        /// Shuts down the app instance thread
        /// </summary>
        public void Disconnect()
        {
            Write(() => listening = false );
        }

        /// <summary>
        /// Checks if a command is valid to have received in the app's current state
        /// </summary>
        /// <param name="cmd">The command that was received </param>
        /// <returns>Returns true if the command is valid for the current state, false otherwise</returns>
        private bool CanUse(Command cmd)
        {
            return Read(() => {
                switch (AppStatus)
                {
                    case Status.Connected:
                        return (cmd is Manifest);

                    case Status.Active:
                    case Status.Inactive:
                    case Status.InUse:
                        return true;
                }
                return true;
            });
        }
    
        /// <summary>
        /// Lambda that can be used for locking
        /// </summary>
        private delegate T LockOperation<T>();
        private delegate void LockOperation();

        /// <summary>
        /// Executes the operation under a read lock
        /// </summary>
        /// <param name="op">The operation that will be locked</param>
        private T Read<T>(LockOperation<T> op)
        {
            try
            {
                readwrite.EnterReadLock();
                return op();
            }
            finally
            {
                readwrite.ExitReadLock();
            }
        }

        /// <summary>
        /// Locks on an operation that does not return
        /// </summary>
        /// <param name="op"></param>
        private void Read(LockOperation op){
            try
            {
                readwrite.EnterReadLock();
                op();
            }
            finally
            {
                readwrite.ExitReadLock();
            }
        }

        
        /// <summary>
        /// Runs the operation under a write lock
        /// </summary>
        /// <param name="op">The operation under the write lock</param>
        private T Write<T>(LockOperation<T> op)
        {
            try
            {
                readwrite.EnterWriteLock();
                return op();
            }
            finally
            {
                readwrite.ExitWriteLock();
            }
        }

        /// <summary>
        /// Locks on a write operation that doesn't return
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="op"></param>
        /// <returns></returns>
        private void Write(LockOperation op)
        {
            try
            {
                readwrite.EnterWriteLock();
                op();
            }
            finally
            {
                readwrite.ExitWriteLock();
            }
        }
    }
}
