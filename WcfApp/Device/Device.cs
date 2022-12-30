using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device
{
     class Device
    {
        Models.Konekcije.IKonekcija<IWrite> klijent = null;
        public Device(Models.Konekcije.IKonekcija<IWrite> konekcija)
        {
            klijent = konekcija;
        }

        public void Run()
        {
            Console.WriteLine("Device je spreman");

            int maxIdMerenja = 20;
            int maxIdDevice = 10;
            int maxVrednost = 100;

            while (true)
            {
                Random rand = new Random();
                Merenje merenje = new Merenje()
                {
                    IdMerenja = rand.Next(maxIdMerenja),
                    IdDevice = rand.Next(maxIdDevice),
                    Timestamp = DateTime.Now.Ticks,
                    Vrednost = rand.NextDouble() * maxVrednost,
                    Tip = (MerenjeTip)(rand.Next() % 2)
                };

                klijent.Service.WriteDevice(merenje);
                Task.Delay(10000).Wait();
            }
        }
    }
}
