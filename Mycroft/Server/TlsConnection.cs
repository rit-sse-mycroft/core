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
    public class TlsConnection : TcpConnection
    {
        private X509Certificate Cert;
        public TlsConnection(TcpClient client, X509Certificate cert) : base(client)
        {
            this.Cert = cert;
        }


        public override Stream GetStream()
        {
            var sslStream = new SslStream(Client.GetStream());
            sslStream.AuthenticateAsServer(Cert);
            return sslStream;
        }

    }
}
