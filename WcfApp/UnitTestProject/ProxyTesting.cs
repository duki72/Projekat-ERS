using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Moq;
using Proxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public class DummyProxyReadLine : Proxy.ProxyInput
    {
        public override void ReadLine()
        {}
    }


    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ProxyTesting
    {
        private Proxy.Proxy proxy;
        private Mock<Models.Konekcije.ServerKonekcija<IMerenjeService>> proxyAsServerMock; 
        private Mock<Models.Konekcije.ServerKonekcija<IServerMerenjeService>> proxyAsClientMock; 
        private Mock<DummyProxyReadLine> dummyInputMock; 
        public ProxyTesting()
        {
            proxyAsClientMock = new Mock<Models.Konekcije.ServerKonekcija<IServerMerenjeService>>();
            proxyAsServerMock = new Mock<Models.Konekcije.ServerKonekcija<IMerenjeService>>();
            dummyInputMock = new Mock<DummyProxyReadLine>();
            proxy = new Proxy.Proxy(proxyAsServerMock.Object, proxyAsClientMock.Object, dummyInputMock.Object);
        }
        
        [TestMethod]
        public void ProxyConstructingTest()
        {
            Assert.IsNotNull(proxy.KonekcijaProxyAsClient);
            Assert.IsNotNull(proxy.KonekcijaProxyAsServer);
            Assert.IsNotNull(proxy.ProxyService);
        }

        
        [TestMethod]
        public void ProxyStartingTest()
        {
            proxy.StartProxy();
            proxy.secondsToSleep = 0;
            Assert.IsTrue(proxy.ProxyWorking);
            proxy.StartProxyServices();
            Assert.IsTrue(proxy.ProxyTasks.Count == 1);
        }
    }

    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ProxyServiceTesting
    {
        private Proxy.ProxyService proxyService;
        private Mock<Models.IServerMerenjeService> serverServiceMock;
        public ProxyServiceTesting()
        {
            serverServiceMock = new Mock<IServerMerenjeService>();
            proxyService = new Proxy.ProxyService(serverServiceMock.Object);
        }

        [TestMethod]
        public void ProxyServiceCheckRemovalsGetSet()
        {
            List<ProxyMerenje> merenjas = new List<ProxyMerenje>()
            {
                new Proxy.ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 1,
                        IdDevice = 2,
                        IdMerenja = 3,
                        Timestamp = 4,
                        Tip = (MerenjeTip)1,
                        Vrednost = 6
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-2),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
                new Proxy.ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 11,
                        IdDevice = 22,
                        IdMerenja = 32,
                        Timestamp = 42,
                        Tip = (MerenjeTip)1,
                        Vrednost = 611
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-2),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
            };

            proxyService.SetLocalStorage(merenjas);
            Assert.IsTrue(proxyService.GetLocalStorage().Count == 2);
            proxyService.CheckRemovals();
            Assert.IsTrue(proxyService.GetLocalStorage().Count == 0);
        }

        [TestMethod]
        public void TestUpdating()
        {
            List<ProxyMerenje> merenjas = new List<ProxyMerenje>()
            {
                new Proxy.ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 1,
                        IdDevice = 2,
                        IdMerenja = 3,
                        Timestamp = 4,
                        Tip = (MerenjeTip)1,
                        Vrednost = 6
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-2),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
                new Proxy.ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 11,
                        IdDevice = 22,
                        IdMerenja = 32,
                        Timestamp = 42,
                        Tip = (MerenjeTip)1,
                        Vrednost = 611
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-2),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
            };

            var newMerenjeInfo = new Merenje()
            {
                IdDb = 11,
                IdDevice = 22,
                IdMerenja = 32,
                Timestamp = 42,
                Tip = (MerenjeTip)1,
                Vrednost = 6122
            };

            proxyService.UpdateLocalStorageWithNewMerenje(newMerenjeInfo);
            Assert.IsTrue(proxyService
                .GetLocalStorage()
                .Find(x => x.MerenjeInfo.IdDb == newMerenjeInfo.IdDb).MerenjeInfo.Vrednost
                    == newMerenjeInfo.Vrednost
            );
        }

        [TestMethod]
        public void TestGetAllById()
        {
            Dictionary<int, long> dict = new Dictionary<int, long>();
            List <ProxyMerenje> merenjas = new List<ProxyMerenje>()
            {
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 1,
                        IdDevice = 2,
                        IdMerenja = 3,
                        Timestamp = 4,
                        Tip = (MerenjeTip)1,
                        Vrednost = 6
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-2),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 11,
                        IdDevice = 22,
                        IdMerenja = 32,
                        Timestamp = 42,
                        Tip = (MerenjeTip)1,
                        Vrednost = 611
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-2),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 121,
                        IdDevice = 22,
                        IdMerenja = 32,
                        Timestamp = DateTime.Now.AddDays(10).Ticks,
                        Tip = (MerenjeTip)1,
                        Vrednost = 611
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-1),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
            };

            merenjas.ForEach(x =>
            {
                dict.Add(x.MerenjeInfo.IdDb, x.MerenjeInfo.Timestamp);
            });
            
            serverServiceMock.Setup(x => x.GetAllTimestampsById(merenjas[0].MerenjeInfo.IdDb))
                             .Returns(dict);
            
            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[0].MerenjeInfo.IdDb))
                              .Returns(merenjas[0].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[1].MerenjeInfo.IdDb))
                              .Returns(merenjas[1].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[2].MerenjeInfo.IdDb))
                              .Returns(merenjas[0].MerenjeInfo);

            var res = proxyService.GetAllById(merenjas[0].MerenjeInfo.IdDb);

            Assert.IsNotNull(res);
            Assert.AreEqual(3, res.Count);
        }

        [TestMethod]
        public void TestGetAzuriranaVrednost()
        {
            Dictionary<int, long> dict = new Dictionary<int, long>();
            List<ProxyMerenje> merenjas = new List<ProxyMerenje>()
            {
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 1,
                        IdDevice = 2,
                        IdMerenja = 3,
                        Timestamp = 4,
                        Tip = (MerenjeTip)1,
                        Vrednost = 6
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-2),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 11,
                        IdDevice = 22,
                        IdMerenja = 32,
                        Timestamp = 42,
                        Tip = (MerenjeTip)1,
                        Vrednost = 611
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-2),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 121,
                        IdDevice = 22,
                        IdMerenja = 32,
                        Timestamp = DateTime.Now.AddDays(10).Ticks,
                        Tip = (MerenjeTip)1,
                        Vrednost = 611
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-1),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
            };

            merenjas.ForEach(x =>
            {
                dict.Add(x.MerenjeInfo.IdDb, x.MerenjeInfo.Timestamp);
            });

            serverServiceMock.Setup(x => x.GetLastTimestampById(merenjas[0].MerenjeInfo.IdDb))
                             .Returns(DateTime.Now.AddDays(2).Ticks);
            
            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[0].MerenjeInfo.IdDb))
                              .Returns(merenjas[0].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[1].MerenjeInfo.IdDb))
                              .Returns(merenjas[1].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[2].MerenjeInfo.IdDb))
                              .Returns(merenjas[0].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetLastMerenjeFromIdMerenje(merenjas[0].MerenjeInfo.IdMerenja))
                .Returns(merenjas[0].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetLastMerenjeFromIdMerenje(merenjas[1].MerenjeInfo.IdMerenja))
                .Returns(merenjas[1].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetLastMerenjeFromIdMerenje(merenjas[2].MerenjeInfo.IdMerenja))
                .Returns(merenjas[2].MerenjeInfo);

            double azurirana = proxyService.GetAzuriranuVrednost(merenjas[0].MerenjeInfo.IdMerenja);
            Assert.AreEqual(azurirana, 6);
            proxyService.SetLocalStorage(merenjas);
            azurirana = proxyService.GetAzuriranuVrednost(merenjas[1].MerenjeInfo.IdMerenja);
            serverServiceMock.Setup(x => x.GetLastTimestampById(merenjas[0].MerenjeInfo.IdMerenja))
                             .Returns(DateTime.Now.AddDays(200).Ticks);
            serverServiceMock.Setup(x => x.GetLastTimestampById(merenjas[1].MerenjeInfo.IdMerenja))
                             .Returns(DateTime.Now.AddDays(200).Ticks);
            serverServiceMock.Setup(x => x.GetLastTimestampById(merenjas[2].MerenjeInfo.IdMerenja))
                             .Returns(DateTime.Now.AddDays(200).Ticks);
            azurirana = proxyService.GetAzuriranuVrednost(merenjas[1].MerenjeInfo.IdMerenja);
            Assert.AreEqual(true, true);
        }



        [TestMethod]
        public void TestGetAzuriraneVrednostiUredjaja()
        {
            List<ProxyMerenje> merenjas = new List<ProxyMerenje>()
            {
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 1,
                        IdDevice = 2,
                        IdMerenja = 3,
                        Timestamp = 4,
                        Tip = (MerenjeTip)1,
                        Vrednost = 6
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-2),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 11,
                        IdDevice = 22,
                        IdMerenja = 32,
                        Timestamp = 42,
                        Tip = (MerenjeTip)1,
                        Vrednost = 611
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-2),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 121,
                        IdDevice = 232,
                        IdMerenja = 32,
                        Timestamp = DateTime.Now.AddDays(10).Ticks,
                        Tip = (MerenjeTip)1,
                        Vrednost = 611
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-1),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
            };

            Dictionary<long, long> dict = new Dictionary<long, long>();

            merenjas.ForEach(x =>
            {
                dict.Add(x.MerenjeInfo.IdDevice, x.MerenjeInfo.Timestamp);
            });

            serverServiceMock.Setup(x => x.GetTimestampPerDevice())
                             .Returns(dict);

            serverServiceMock.Setup(x => x.GetLastForDeviceId(merenjas[0].MerenjeInfo.IdDevice))
                              .Returns(merenjas[0].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetLastForDeviceId(merenjas[1].MerenjeInfo.IdDevice))
                              .Returns(merenjas[1].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetLastForDeviceId(merenjas[2].MerenjeInfo.IdDevice))
                              .Returns(merenjas[2].MerenjeInfo);
            
            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[0].MerenjeInfo.IdDb))
                              .Returns(merenjas[0].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[1].MerenjeInfo.IdDb))
                              .Returns(merenjas[1].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[2].MerenjeInfo.IdDb))
                              .Returns(merenjas[2].MerenjeInfo);

            var res = proxyService.GetAzuriraneVrednostiUredjaja();

            proxyService.SetLocalStorage(merenjas);

            res = proxyService.GetAzuriraneVrednostiUredjaja();

            dict[merenjas[2].MerenjeInfo.IdDevice] = DateTime.Now.AddDays(100).Ticks;
            serverServiceMock.Setup(x => x.GetTimestampPerDevice())
                             .Returns(dict);


            proxyService.SetLocalStorage(merenjas);

            res = proxyService.GetAzuriraneVrednostiUredjaja();

            Assert.IsNotNull(res);
            Assert.AreEqual(3, res.Count);
        }


        [TestMethod]
        public void TestGetDigital()
        {
            List<ProxyMerenje> merenjas = new List<ProxyMerenje>()
            {
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 1,
                        IdDevice = 2,
                        IdMerenja = 3,
                        Timestamp = 4,
                        Tip = (MerenjeTip)1,
                        Vrednost = 6
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-2),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 11,
                        IdDevice = 22,
                        IdMerenja = 32,
                        Timestamp = 42,
                        Tip = (MerenjeTip)1,
                        Vrednost = 611
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-2),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 121,
                        IdDevice = 232,
                        IdMerenja = 32,
                        Timestamp = DateTime.Now.AddDays(10).Ticks,
                        Tip = (MerenjeTip)1,
                        Vrednost = 611
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-1),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
            };

            Dictionary<long, long> dict = new Dictionary<long, long>();

            merenjas.ForEach(x =>
            {
                dict.Add(x.MerenjeInfo.IdDb, x.MerenjeInfo.Timestamp);
            });

            serverServiceMock.Setup(x => x.GetTimestampsDigital())
                             .Returns(dict);

            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[0].MerenjeInfo.IdDb))
                              .Returns(merenjas[0].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[1].MerenjeInfo.IdDb))
                              .Returns(merenjas[1].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[2].MerenjeInfo.IdDb))
                              .Returns(merenjas[2].MerenjeInfo);

            var res = proxyService.GetDigitalni();

            proxyService.SetLocalStorage(merenjas);

            res = proxyService.GetDigitalni();

            dict[merenjas[2].MerenjeInfo.IdDb] = DateTime.Now.AddDays(100).Ticks;
            serverServiceMock.Setup(x => x.GetTimestampPerDevice())
                             .Returns(dict);

            proxyService.SetLocalStorage(merenjas);

            res = proxyService.GetDigitalni();

            Assert.IsNotNull(res);
        }

        [TestMethod]
        public void TestGetAnalog()
        {
            List<ProxyMerenje> merenjas = new List<ProxyMerenje>()
            {
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 1,
                        IdDevice = 2,
                        IdMerenja = 3,
                        Timestamp = 4,
                        Tip = (MerenjeTip)0,
                        Vrednost = 6
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-2),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 11,
                        IdDevice = 22,
                        IdMerenja = 32,
                        Timestamp = 42,
                        Tip = (MerenjeTip)0,
                        Vrednost = 611
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-2),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
                new ProxyMerenje()
                {
                    MerenjeInfo = new Merenje()
                    {
                        IdDb = 121,
                        IdDevice = 232,
                        IdMerenja = 32,
                        Timestamp = DateTime.Now.AddDays(10).Ticks,
                        Tip = (MerenjeTip)0,
                        Vrednost = 611
                    },
                    PoslednjiPutDobavljeno = DateTime.Now.AddDays(-1),
                    PoslednjiPutProcitano = DateTime.Now.AddDays(-2)
                },
            };

            Dictionary<long, long> dict = new Dictionary<long, long>();

            merenjas.ForEach(x =>
            {
                dict.Add(x.MerenjeInfo.IdDb, x.MerenjeInfo.Timestamp);
            });

            serverServiceMock.Setup(x => x.GetTimestampsAnalog())
                             .Returns(dict);

            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[0].MerenjeInfo.IdDb))
                              .Returns(merenjas[0].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[1].MerenjeInfo.IdDb))
                              .Returns(merenjas[1].MerenjeInfo);
            serverServiceMock.Setup(x => x.GetMerenjeByDbId(merenjas[2].MerenjeInfo.IdDb))
                              .Returns(merenjas[2].MerenjeInfo);

            var res = proxyService.GetAnalogni();

            proxyService.SetLocalStorage(merenjas);

            res = proxyService.GetAnalogni();

            dict[merenjas[2].MerenjeInfo.IdDb] = DateTime.Now.AddDays(100).Ticks;
            serverServiceMock.Setup(x => x.GetTimestampPerDevice())
                             .Returns(dict);

            proxyService.SetLocalStorage(merenjas);

            res = proxyService.GetAnalogni();

            Assert.IsNotNull(res);
        }
    }
}
