using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Server
{
    public class TcpConnection
    {
        public TcpClient Client { get; private set; }

        public TcpConnection(TcpClient client)
        {
            Client = client;
        }

        public virtual Stream GetStream()
        {
            return Client.GetStream();
        }

    }
}
