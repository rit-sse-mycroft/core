using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.Server
{
    public class TlsClient : TcpClient
    {
        private TcpClient tcpClient;
        private X509Certificate cert;
        public TlsClient(TcpClient client, X509Certificate cert)
            : base(client.Client) //Close the underlying NetworkStream on close
        {
            this.cert = cert;
        }

        public Stream GetStream()
        {
            var sslStream = new SslStream(base.GetStream());
            sslStream.AuthenticateAsServer(cert);
            return sslStream;
        }

    }
}
