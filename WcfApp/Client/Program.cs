using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        [ExcludeFromCodeCoverage]
        static void Main(string[] args)
        {
            Klijent klijent = new Klijent(
                new Models.Konekcije.KlijentKonekcija<Models.IMerenjeService>(
                    Models.Konekcije.Konekcija.UriProxyServer
                ),
                new KlijentInput()
            );
            klijent.Run();
        }
    }
}
