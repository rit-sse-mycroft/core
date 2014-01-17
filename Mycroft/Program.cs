using Mycroft.App;
using Mycroft.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft
{
    class Program
    {
        static void Main(string[] args)
        {
            // Accessing certificates may need to be abstracted for Mono
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            Debug.WriteLine(store.Certificates.Count);

            // Use the settings file to figure out which certificate to use
            var collection = store.Certificates.Find(
                X509FindType.FindByThumbprint,
                ConfigurationManager.AppSettings["CertThumbprint"],
                false
            );

            foreach(var c in collection)
            {
                Debug.Write(c.Subject);
                Debug.Write(" ");
                Debug.WriteLine(c.Thumbprint);
            }

            X509Certificate2 cert = collection[0];

            var registry = new Registry();
            
            var myServ = new TlsServer(IPAddress.Any, 1847, cert);
            //insecure version var myServ = new TcpServer(IPAddress.Any, 1847);
            var dispatcher = new Dispatcher(myServ, registry);
            dispatcher.Run();
        }
    }
}
