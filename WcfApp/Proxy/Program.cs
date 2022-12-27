using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("Proxy je spreman");

            const short serverPort = 1234;
            var uri = $"net.tcp://localhost:{serverPort}/MerenjeService";

            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            var channel = new ChannelFactory<IMerenjeService>(binding);
            var endpoint = new EndpointAddress(uri);
            var proxy = channel.CreateChannel(endpoint);
            var res = proxy.GetMerenjes();
            Console.WriteLine("Received:");
            res.ToList().ForEach(x => Console.WriteLine(x));
            Console.ReadLine();
        }
    }
}
