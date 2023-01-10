using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    class Program
    {
        [ExcludeFromCodeCoverage]

        static void Main(string[] args)
        {
            var KonekcijaProxyAsClient = new Models.Konekcije.KlijentKonekcija<IServerMerenjeService>(
                Models.Konekcije.Konekcija.UriServer
            );

            var ProxyService = new ProxyService(KonekcijaProxyAsClient.Service);

            var KonekcijaProxyAsServer = new Models.Konekcije.ServerKonekcija<IMerenjeService>(
                new string[] { Models.Konekcije.Konekcija.UriProxyServer }, ProxyService);

            Console.WriteLine("Proxy se digao");
            Proxy proxy = new Proxy(KonekcijaProxyAsServer, KonekcijaProxyAsClient, new ProxyInput());
            proxy.StartProxy();
            proxy.StartProxyServices();
        }
    }
}
