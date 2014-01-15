using Mycroft.App;
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
        /// <summary>
        /// The App instances that are running
        /// </summary>
        private IDictionary<string, AppInstance> instances;
        private IDictionary<string, Capability> capabilities;

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
        public void HandleClientConnected(Stream stream)
        {
            //  Create a new AppInstance and add it to the instances collection
            var instance = new AppInstance(stream, dispatcher);
            instances.Add(instance.InstanceId, instance);
            instance.Listen();
        }

        /// <summary>
        /// Changes the instance ID of an AppInstance
        /// </summary>
        /// <param name="oldId">The old ID of the instance</param>
        /// <param name="newId">The new ID used to handle the instance</param>
        /// <returns>Returns true if the instance ID was successfully changed,
        /// false if the ID was in use</returns>
        internal bool ChangeInstanceId(string oldId, string newId)
        {
            // Make sure we don't have duplicates
            if (instances.Keys.Contains(newId))
            {
                return false;
            }

            // Swap the ID
            var instance = instances[oldId];
            instance.InstanceId = newId;
            instances.Remove(oldId);
            instances[newId] = instance;

            return true;
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
