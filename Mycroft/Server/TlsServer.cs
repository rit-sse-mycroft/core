using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mycroft.Server
{
    class TlsServer : TcpServer
    {
        private X509Certificate cert;

        public TlsServer (IPAddress addr, Int32 port, X509Certificate cert)
            : base(addr, port)
        {
            this.cert = cert;
        }


        private override TcpClient PrepClient(TcpClient client) {
            return new TlsClient(client, cert);
        }
    }
}
