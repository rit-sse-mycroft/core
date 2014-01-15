using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft
{
    public class TlsClientStream : SslStream
    {
        private TcpClient tcpClient;
        public TlsClientStream(TcpClient client)
            : base(client.GetStream(), false) //Close the underlying NetworkStream on close
        {
            this.tcpClient = client;
        }

        public override void Close()
        {
            tcpClient.Close();
            base.Close();
        }

    }
}
