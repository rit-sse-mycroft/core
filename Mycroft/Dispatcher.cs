using Mycroft.App;
using Mycroft.Cmd;
using Mycroft.Cmd.Msg;
using Mycroft.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mycroft
{
    public class Dispatcher : ICommandable
    {
        private BlockingCollection<Command> DispatchQueue;
        private BlockingCollection<Command> DispatchPreemptStack;
        private TcpServer Server;
        private Registry Registry;
        private MessageArchive MessageArchive;
        private Logger Log;

        public Dispatcher(TcpServer server, Registry registry, MessageArchive messageArchive)
        {
            Server = server;
            Registry = registry;
            MessageArchive = messageArchive;
            DispatchQueue = new BlockingCollection<Command>(new ConcurrentQueue<Command>());
            DispatchPreemptStack = new BlockingCollection<Command>(new ConcurrentStack<Command>());
            Log = Logger.GetInstance();
        }

        public void Run()
        {
            Server.ClientConnected += HandleNewClientConnection;
            Server.Start();

            Log.Debug("Dispatcher running");

            Command currentCmd;
            while (true)
            {
                // The preempt stack should only be added to
                // from a command. Because of this, if the DispatchPreemptStack is empty
                // then we can ignore the preempt stack and block until the
                // next command is available through the standard queue.
                if (!DispatchPreemptStack.TryTake(out currentCmd))
                {
                    currentCmd = DispatchQueue.Take();
                }
                // Issue all the commands o/
                Server.Issue(currentCmd);
                MessageArchive.Issue(currentCmd);
                Registry.Issue(currentCmd);
                this.Issue(currentCmd);
            }
        }

        public void Enqueue(Command cmd)
        {
            DispatchQueue.Add(cmd);
        }

        public void PreemptQueue(Command cmd)
        {
            DispatchPreemptStack.Add(cmd);
        }

        /// <summary>
        /// Put a newly connected app in its own thread in the app thread pool
        /// </summary>
        /// <param name="connection"></param>
        private void HandleNewClientConnection(CommandConnection connection)
        {
            var instance = new AppInstance(connection, this);
            instance.Listen();
        }

        /// <summary>
        /// Applies a command to the registry
        /// </summary>
        /// <param name="command"></param>
        public void Issue(Command command)
        {
            command.VisitDispatcher(this);
        }
    }
}