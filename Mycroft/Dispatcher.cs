using Mycroft.App;
using Mycroft.Cmd;
using Mycroft.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft
{
    public class Dispatcher
    {
        private ConcurrentQueue<Command> DispatchQueue;
        private TcpServer Server;
        public Dispatcher(TcpServer server, Registry registry)
        {
            Server = server;
            DispatchQueue = new ConcurrentQueue<Command>();
        }

        public void Run()
        {
            Command currentCmd;
            while (true)
            {
                if (DispatchQueue.TryDequeue(out currentCmd))
                {
                    // Issue all the commands o/
                    Server.Issue(currentCmd);
                }
            }
        }
        public void Enqueue(Command cmd)
        {
            DispatchQueue.Enqueue(cmd);
        }
    }
}
