using System;
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


        protected override TcpConnection PrepClient(TcpClient client) {
            return new TlsConnection(client, cert);
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
            return Regex.Replace(thumbprint, @"[^a-zA-Z0-9]", "", RegexOptions.IgnoreCase).ToUpper();
        }
    }
}
