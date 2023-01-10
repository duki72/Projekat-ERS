using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;

namespace UnitTestProject
{
    public class KlijentInputDummy : Client.KlijentInput
    {
        public override string ReadLine()
        {
            return "2";
        }
    }

    [TestClass]
    public class UnitTestClient
    {
        private Client.Klijent klijentService;
        private Mock<Models.Konekcije.IKonekcija<Models.IMerenjeService>> mockClient;

        public UnitTestClient()
        {
            SetUp();
        }

        public void SetUp()
        {
            mockClient = new Mock<Models.Konekcije.IKonekcija<Models.IMerenjeService>>();
            klijentService = new Client.Klijent(mockClient.Object, new KlijentInputDummy());
        }

        [TestMethod]
        public void testProvera()
        {

            mockClient.Setup(x => x.Service.GetAllById(2)).Returns(new List<Models.Merenje>());

            mockClient.Setup(x => x.Service.GetAzuriraneVrednostiUredjaja()).Returns(new List<double?>());
            mockClient.Setup(x => x.Service.GetAzuriranuVrednost(2)).Returns(0);
            mockClient.Setup(x => x.Service.GetDigitalni()).Returns(new List<Models.Merenje>());

            mockClient.Setup(x => x.Service.GetAnalogni()).Returns(new List<Models.Merenje>());

            Assert.DoesNotThrow(() => { klijentService.provera(1); });
            Assert.DoesNotThrow(() => { klijentService.provera(2); });
            Assert.DoesNotThrow(() => { klijentService.provera(3); });
            Assert.DoesNotThrow(() => { klijentService.provera(4); });
            Assert.DoesNotThrow(() => { klijentService.provera(5); });

            Exception ex = Assert.Throws<Exception>(() => { klijentService.provera(7); });
            Assert.That(ex.Message, Is.EqualTo("Niste izabrali nijednu od opcija. Ponovno biranje."));

        }
    }
}
