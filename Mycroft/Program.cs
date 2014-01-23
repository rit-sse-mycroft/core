using Mycroft.App;
using Mycroft.Cmd.Msg;
using Mycroft.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mycroft
{
    class Program
    {
        /// <summary>
        /// Default port used in Mycroft communications
        /// </summary>
        public const int DEFAULT_PORT = 1847;

        static void Main(string[] args)
        {
            TcpServer server = null;

            if(UsingTls(args))
            {
                X509Certificate2 cert = null;
                var foundCert = TryGetX509Certificate(args, out cert);

                // We have a certificate, so create the server
                if (foundCert)
                {
                    server = new TlsServer(IPAddress.Any, DEFAULT_PORT, cert);
                }
            }
            else
            {
                //insecure version
                server = new TcpServer(IPAddress.Any, DEFAULT_PORT);
            }

            // If we can't start the server, we can't run anything
            if(server == null)
            {
                Environment.Exit(1);
            }

            // All systems go, start the server
            var registry = new Registry();
            var MessageArchive = new MessageArchive();
            var dispatcher = new Dispatcher(server, registry, MessageArchive);
            dispatcher.Run();
        }

        /// <summary>
        /// Determines if the server should be run using TLS
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Returns true if the flag "--no-tls" was not included</returns>
        private static bool UsingTls(string[] args)
        {
            return !args.Contains("--no-tls");
        }

        private static bool TryGetX509Certificate(string[] args, out X509Certificate2 cert)
        {

            var indexCertFlag = Array.IndexOf(args, "--cert");
            if (indexCertFlag >= 0)
            {
                // Make sure a certificate file was given
                var indexCertFile = indexCertFlag + 1;
                if (indexCertFile >= args.Length)
                {
                    Console.Error.WriteLine("Error: --cert parameter must include certificate file");
                    cert = null;
                    return false;
                }

                // Load the certificate file
                var certFile = args[indexCertFile];
                try
                {
                    cert = new X509Certificate2(certFile);
                    return true;
                }
                catch (CryptographicException e)
                {
                    Console.Error.WriteLine("Error: Failed to load certificate \"{0}\" - {1}", certFile, e.Message.Trim());
                    cert = null;
                    return false;
                }
            }

            // No file specified; load from certificate store
            // Accessing certificates may need to be abstracted for Mono
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            Debug.WriteLine(store.Certificates.Count);

            var thumbprint = TlsServer.FormatCertificateThumbprint(
                ConfigurationManager.AppSettings["CertThumbprint"]
            );

            // Use the settings file to figure out which certificate to use
            var collection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

            // Make sure the desired fingerprint exists
            if (collection.Count == 0)
            {
                Console.Error.WriteLine("Error: Certificate with thumbprint {0} not found. Please make sure it is installed to the root CA store.", thumbprint);
                cert = null;
                return false;
            }

            // Return the certificate
            cert = collection[0];
            return true;
        }
    }
}
