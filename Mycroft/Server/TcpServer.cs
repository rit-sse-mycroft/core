﻿using Mycroft.App;
using Mycroft.Cmd;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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

    public delegate void HandleClientConnected(CommandConnection client);

    /// <summary>
    /// Starts Mycroft's network communications and owns resources in the server
    /// </summary>
    public class TcpServer : ICommandable
    {
        private Thread listeningThread;
        private TcpListener tcpListener;
        private volatile bool cancelThread;
        private Logger Log;

        public TcpServer(IPAddress ip, int port)
        {
            tcpListener = new TcpListener(ip, port);
            Log = Logger.GetInstance();
        }

        /// <summary>
        /// Starts the server listening for network connections
        /// </summary>
        public void Start()
        {
            cancelThread = false;
            listeningThread = new Thread(Listen);
            listeningThread.Start();
        }

        private void Listen(object pars)
        {
            Log.Debug("Listening for connections...");
            tcpListener.Start();
            while (!cancelThread)
            {
                var tcpClient = tcpListener.AcceptTcpClient();
                var ip = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address;
                Log.Debug(String.Format(
                    "Client connected from IP {0}",
                    ip.ToString()
                ));

                try
                {
                    var connection = CreateConnection(tcpClient);

                    //create a thread to handle communication with connected client
                    var clientThread = new Thread(new ParameterizedThreadStart(OnClientConnected));
                    clientThread.Start(connection);
                }
                catch (NotSupportedException e)
                {
                    Log.Warning(String.Format(
                        "Connection to {0} failed: {1}",
                        ip,
                        e.Message
                    ));
                }
                catch (IOException e)
                {
                    Log.Warning(String.Format(
                        "Connection to {0} failed: {1}",
                        ip,
                        e.Message
                    ));
                }
            }
            tcpListener.Stop();
        }

        public void Stop()
        {
            cancelThread = true;
            listeningThread.Join();
        }

        /// <summary>
        /// Creates a connection object from a TcpClient
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        protected virtual CommandConnection CreateConnection(TcpClient client)
        {
            return new CommandConnection(client, client.GetStream());
        }

        /// <summary>
        /// Handle clients connecting to the server by creating an AppInstance for the connection
        /// </summary>
        /// <param name="stream">The stream over which the app is sending information</param>
        private void OnClientConnected(object connection)
        {
            Log.Debug("Client connected...");

            if (ClientConnected != null)
            {
                var cmdConnection = connection as CommandConnection;
                // Asynchronously run the event handler, generating the listener threads
                ClientConnected.BeginInvoke(cmdConnection, (IAsyncResult result) =>
                    {
                        try
                        {
                            cmdConnection.Close();
                        }
                        catch (InvalidOperationException) { }
                    },
                    null);
            }
        }

        /// <summary>
        /// Allow the server to be visited by commands
        /// </summary>
        /// <param name="command">The command that will operate on the server</param>
        public void Issue(Command command)
        {
            command.VisitServer(this);
        }

        public event HandleClientConnected ClientConnected;
    }
}
