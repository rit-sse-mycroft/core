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

namespace Mycroft
{
    class TlsServer
    {
        private X509Certificate2 cert;
        private TcpListener tcpListener;
        private Thread listenThread;

        public TlsServer (IPAddress addr, Int32 port, X509Certificate2 cert)
        {
           this.cert = cert;
           this.tcpListener = new TcpListener(addr, port);
        }

        public TlsServer(TcpListener listener, X509Certificate2 cert)
        {
            this.cert = cert;
            this.tcpListener = listener;
        }

        public void Start()
        {
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }

        private void ListenForClients()
        {
            this.tcpListener.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient tcpClient = this.tcpListener.AcceptTcpClient();
                TlsClientStream tlsClient = new TlsClientStream(tcpClient);
                //create a thread to handle communication 
                //with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(tlsClient);
            }
        }

        private async void HandleClientComm(object client)
        {
            TlsClientStream tlsClient = (TlsClientStream)client;
            await tlsClient.AuthenticateAsServerAsync(cert);
            OnClientConnected(tlsClient);
        }

        protected virtual void OnClientConnected(TlsClientStream conn)
        {
            EventHandler<TlsClientStream> handler = ClientConnected;
            if (handler != null)
            {
                handler(this, conn);
            }
        }

        public event EventHandler<TlsClientStream> ClientConnected;
    }
}
