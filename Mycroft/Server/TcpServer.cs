using Mycroft.App;
using Mycroft.Cmd;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mycroft.Server
{

    public delegate void HandleClientConnected(TcpClient client);
    /// <summary>
    /// Starts Mycroft's network communications and owns resources in the server
    /// </summary>
    public class TcpServer : ICommandable
    {
        private Thread listeningThread;
        private TcpListener tcpListener;
        private bool cancelThread;
        public TcpServer(IPAddress ip, int port)
        {
            tcpListener = new TcpListener(ip, port);
        }

        /// <summary>
        /// Starts the server listening for network connections
        /// </summary>
        public void Start()
        {
            cancelThread = false;
            listeningThread = new Thread(new ParameterizedThreadStart(Listen));
        }

        private void Listen(object pars)
        {
            while (!cancelThread)
            {
                var tcpClient = tcpListener.AcceptTcpClient();

                //create a thread to handle communication with connected client
                var clientThread = new Thread(new ParameterizedThreadStart(OnClientConnected));
                clientThread.Start(PrepClient(tcpClient));
            }
        }

        public void Stop()
        {
            cancelThread = true;
            listeningThread.Join();
        }

        protected TcpClient PrepClient(TcpClient client)
        {
            return client;
        }

        /// <summary>
        /// Handle clients connecting to the server by creating an AppInstance for the connection
        /// </summary>
        /// <param name="stream">The stream over which the app is sending information</param>
        private void OnClientConnected(object tcpClient)
        {
            if (ClientConnected != null)
                ClientConnected(PrepClient((TcpClient)tcpClient));
        }

        /// <summary>
        /// Allow the server to be visited by commands
        /// </summary>
        /// <param name="command">The command that will operate on the server</param>
        public void Issue(Command command)
        {
            command.visitServer(this);
        }

        public event HandleClientConnected ClientConnected;
    }
}
