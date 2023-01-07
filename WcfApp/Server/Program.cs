using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            const short serverPort = 1234;
            var uris = new Uri[]
            {
                new Uri($"net.tcp://localhost:{serverPort}/MerenjeService")
            };
            IMerenjeService service = new MerenjeService();
            ServiceHost host = new ServiceHost(service, uris);
            var binding = new NetTcpBinding(SecurityMode.None);
            host.AddServiceEndpoint(typeof(IMerenjeService), binding, "");
            host.Opened += HostOpened;
            host.Open();
            Console.ReadLine();
        }
    }
}
