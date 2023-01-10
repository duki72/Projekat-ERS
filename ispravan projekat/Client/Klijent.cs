using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class KlijentInput
    {
        public virtual string ReadLine()
        {
            return Console.ReadLine();
        }
    }

    public class Klijent
    {
        Models.Konekcije.IKonekcija<Models.IMerenjeService> klijent = null;
        KlijentInput input = null;
        public Klijent(Models.Konekcije.IKonekcija<Models.IMerenjeService> konekcija, KlijentInput klijentInput)
        {
            klijent = konekcija;
            input = klijentInput;
        }

        [ExcludeFromCodeCoverage]
        public void Run()
        {
            int res = 0;
            while (res != 6)
            {
                Console.WriteLine("Izaberu jednu od opcija:");
                Console.WriteLine("\t1 - Svi podaci za dati Id");
                Console.WriteLine("\t2 - Poslednja azurirana vrednost za dati Id");
                Console.WriteLine("\t3 - Poslednje azurirane vrednosti svakog uredjaja");
                Console.WriteLine("\t4 - Svi podaci analognih merenja");
                Console.WriteLine("\t5 - Svi podaci digitalnih merenja");
                Console.WriteLine("\t6 - Izadji");
                Console.Write("Tvoja opcija je? ");
                res = Convert.ToInt32(input.ReadLine());
                // Use a switch statement to do the math.
                provera(res);
            }
        }


        public void provera(int br)
        {
            switch (br)
            {
                case 1:
                    {
                        Console.WriteLine("Unesi ID:");
                        int id = Convert.ToInt32(input.ReadLine());
                        var results = klijent.Service.GetAllById(id);
                        Console.WriteLine("Rezultati:");
                        results.ToList().ForEach(x => Console.WriteLine(x));
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Unesi ID:");
                        int id = Convert.ToInt32(input.ReadLine());
                        var result = klijent.Service.GetAzuriranuVrednost(id);
                        Console.WriteLine($"Azurirana vrednost:{result}");
                        break;
                    }
                case 3:
                    {
                        Console.WriteLine("Rezultati:");
                        var results = klijent.Service.GetAzuriraneVrednostiUredjaja();
                        results.ToList().ForEach(x => Console.WriteLine(x));
                        break;
                    }
                case 4:
                    {
                        Console.WriteLine("Rezultati:");
                        var results = klijent.Service.GetAnalogni();
                        results.ToList().ForEach(x => Console.WriteLine(x));
                        break;
                    }
                case 5:
                    {
                        Console.WriteLine("Rezultati:");
                        var results = klijent.Service.GetDigitalni();
                        results.ToList().ForEach(x => Console.WriteLine(x));
                        break;
                    }
                case 6:
                    {
                        break;
                    }
                default:
                    {
                        throw new Exception("Niste izabrali nijednu od opcija. Ponovno biranje.");
                    }
                    
            }
        }




    }
}
