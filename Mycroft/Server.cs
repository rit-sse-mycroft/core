﻿using Mycroft.App;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mycroft
{
    /// <summary>
    /// Starts Mycroft's network communications and owns resources in the server
    /// </summary>
    public abstract class Server
    {
        private ReaderWriterLockSlim serverLock = new ReaderWriterLockSlim();

        /// <summary>
        /// The App instances that are running
        /// </summary>
        private IDictionary<string, AppInstance> instances;
        private IDictionary<string, Capability> capabilities;

        /// <summary>
        /// Connections that aren't yet initialized and don't have instanceIds
        /// </summary>
        private List<AppInstance> startedConnections;

        /// <summary>
        /// Routes messages through the rest of the system
        /// </summary>
        private Dispatcher dispatcher;

        public Server()
        {
            instances = new Dictionary<string, AppInstance>();
            capabilities = new Dictionary<string, Capability>();
        }

        /// <summary>
        /// Starts the server listening for network connections
        /// </summary>
        public abstract Task Start();

        /// <summary>
        /// Handle clients connecting to the server by creating an AppInstance for the connection
        /// </summary>
        /// <param name="stream">The stream over which the app is sending information</param>
        protected void HandleClientConnected(Stream stream)
        {
            try
            {
                serverLock.EnterWriteLock();
                
                //  Create a new AppInstance and add it to the instances collection
                var instance = new AppInstance(stream, dispatcher);
                instances.Add(instance.InstanceId, instance);
            }
            finally
            {
                serverLock.ExitWriteLock();
            }
            
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
