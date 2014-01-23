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
        private ConcurrentQueue<Command> DispatchQueue;
        private ConcurrentStack<Command> DispatchStack;
        private TcpServer Server;
        private Registry Registry;
        private MessageArchive MessageArchive;

        public Dispatcher(TcpServer server, Registry registry, MessageArchive messageArchive)
        {
            Server = server;
            Registry = registry;
            MessageArchive = messageArchive;
            DispatchQueue = new ConcurrentQueue<Command>();
            DispatchStack = new ConcurrentStack<Command>();
        }

        public void Run()
        {
            Server.ClientConnected += HandleNewClientConnection;
            Server.Start();

            Command currentCmd;
            while (true)
            {
                if (DispatchStack.TryPop(out currentCmd) || DispatchQueue.TryDequeue(out currentCmd))
                {
                    // Issue all the commands o/
                    Server.Issue(currentCmd);
                    MessageArchive.Issue(currentCmd);
                    Registry.Issue(currentCmd);
                    this.Issue(currentCmd);
                }
            }
        }

        public void Enqueue(Command cmd)
        {
            DispatchQueue.Enqueue(cmd);
        }

        public void PreemptQueue(Command cmd)
        {
            DispatchStack.Push(cmd);
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
