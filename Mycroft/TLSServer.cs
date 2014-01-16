﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mycroft
{
    class TlsServer : Server
    {
        private X509Certificate2 cert;
        private TcpListener tcpListener;

        public TlsServer (IPAddress addr, Int32 port, X509Certificate2 cert)
            : this(new TcpListener(addr, port), cert)
        { }

        public TlsServer(TcpListener listener, X509Certificate2 cert)
        {
            this.cert = cert;
            this.tcpListener = listener;
        }

        public override async Task Start()
        {
            tcpListener.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                var tlsClient = new TlsClientStream(tcpClient);

                //create a thread to handle communication with connected client
                var clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(tlsClient);
            }
        }

        private async void HandleClientComm(object client)
        {
            var tlsClient = (TlsClientStream) client;
            await tlsClient.AuthenticateAsServerAsync(cert);
            HandleClientConnected(tlsClient);
        }

        /// <summary>
        /// Formats a SHA-1 hash of the certificate (thumbprint) to be used for searching 
        /// in the user's certificate store
        /// </summary>
        /// <returns>
        /// Returns the thumbprint, stripped of non-alphanumeric characters and capitalized
        /// </returns>
        internal static string FormatCertificateThumbprint(string thumbprint)
        {
            return Regex.Replace(thumbprint, @"[^a-zA-Z0-9]","", RegexOptions.IgnoreCase).ToUpper();
        }
    }
}
