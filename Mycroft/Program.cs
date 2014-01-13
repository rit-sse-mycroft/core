﻿using System;
using System.Collections.Generic;
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
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            Debug.WriteLine(store.Certificates.Count);

            // Use the settings file to figure out which certificate to use
            var collection = store.Certificates.Find(
                X509FindType.FindByThumbprint,
                Settings.Default.CertThumbprint,
                false
            );

            foreach(var c in collection)
            {
                Debug.Write(c.Subject);
                Debug.Write(" ");
                Debug.WriteLine(c.Thumbprint);
            }

            X509Certificate2 cert = collection[0];
            
            var myServ = new TlsServer(IPAddress.Any, 1847, cert);
            myServ.ClientConnected += myServ_ClientConnected;
            myServ.Start();
        }

        static void myServ_ClientConnected(object sender, TlsClientStream e)
        {
            Debug.Write("Hey World");
        }
    }
}
