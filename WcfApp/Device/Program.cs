using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Device
{
    public class Program
    {
        static void Main(string[] args)
        {
            
                Device device = new Device(
                    new Models.Konekcije.KlijentKonekcija<IWrite>(
                        Models.Konekcije.Konekcija.UriServerWrite
                    )
                );
                device.Run();
            


        }
    }
}
