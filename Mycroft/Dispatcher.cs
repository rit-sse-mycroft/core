using Mycroft.App;
using Mycroft.Cmd;
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
        private TcpServer Server;
        private Registry Registry;

        public Dispatcher(TcpServer server, Registry registry)
        {
            Server = server;
            Registry = registry;
            DispatchQueue = new ConcurrentQueue<Command>();
        }

        public void Run()
        {
            Server.ClientConnected += HandleNewClientConnection;
            Server.Start();

            Command currentCmd;
            while (true)
            {
                if (DispatchQueue.TryDequeue(out currentCmd))
                {
                    // Issue all the commands o/
                    Server.Issue(currentCmd);
                    Registry.Issue(currentCmd);
                    this.Issue(currentCmd);
                }
            }
        }

        public void Enqueue(Command cmd)
        {
            DispatchQueue.Enqueue(cmd);
        }

        /// <summary>
        /// Put a newly connected app in its own thread in the app thread pool
        /// </summary>
        /// <param name="connection"></param>
        private void HandleNewClientConnection(TcpConnection connection)
        {
            var instance = new AppInstance(connection.Client, this);
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
