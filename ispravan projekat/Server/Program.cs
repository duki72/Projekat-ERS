using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        [ExcludeFromCodeCoverage]
        static void Main(string[] args)
        {
            Models.Konekcije.ServerKonekcija<IServerMerenjeService> konekcija = 
                new Models.Konekcije.ServerKonekcija<IServerMerenjeService>(
                    new string[] { Models.Konekcije.Konekcija.UriServer },
                    new MerenjeService(new DBCRUD.DBCRUD())
                );
            konekcija.Open();

            Models.Konekcije.ServerKonekcija<IWrite> konekcijaWrite =
                new Models.Konekcije.ServerKonekcija<IWrite>(
                    new string[] { Models.Konekcije.Konekcija.UriServerWrite },
                    new MerenjeService(new DBCRUD.DBCRUD())
                );
            konekcijaWrite.Open();
            Console.ReadLine();
        }
    }

}
