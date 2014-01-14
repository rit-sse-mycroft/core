using Mycroft.App;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mycroft
{
    /// <summary>
    /// Starts Mycroft's network communications and owns resources in the server
    /// </summary>
    public abstract class Server
    {
        /// <summary>
        /// The App instances that are running
        /// </summary>
        private ConcurrentDictionary<string, AppInstance> instances;
        private ConcurrentDictionary<string, Capability> capabilities;

        /// <summary>
        /// Event handler for client connections
        /// </summary>
        public event EventHandler<Stream> ClientConnected;

        public Server()
        {
            instances = new ConcurrentDictionary<string, AppInstance>();
            capabilities = new ConcurrentDictionary<string, Capability>();
        }

        /// <summary>
        /// Starts the server listening for network connections
        /// </summary>
        public abstract void Start();


        /// <summary>
        /// Handle clients connecting to the server by creating an AppInstance for the connection
        /// </summary>
        /// <param name="stream">The stream over which the app is sending information</param>
        protected void HandleClientConnected(Stream stream)
        {
            // TODO - create an AppInstance manages the connection
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
