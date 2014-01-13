using System;
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
            var myServ = new TlsServer(IPAddress.Any, 1847, new X509Certificate2(X509Certificate2.CreateFromCertFile("mycroft.crt")));
            myServ.ClientConnected += myServ_ClientConnected;
            myServ.Start();
        }

        static void myServ_ClientConnected(object sender, TlsClientStream e)
        {
            Debug.Write("Hey World");
        }
    }
}
