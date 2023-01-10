using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using Server.DBCRUD;
using System;
using System.Collections.Generic;
using Assert = NUnit.Framework.Assert;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestServer 
    {  
        private Server.Conversion conversion = new Server.Conversion();
        private Server.MerenjeService serverService;
        private Mock<IDBCRUD> mockCrud;

        public UnitTestServer()
        {
            SetUp();
        }

        public void SetUp()
        {
            mockCrud = new Mock<IDBCRUD>();
            serverService = new Server.MerenjeService(mockCrud.Object);
        }

        [TestMethod]
        public void getTestById()
        {
            int id = 1;
            mockCrud.Setup(x => x.GetMerenjeByDbId(id)).Returns(new Server.Merenje()
            {
                idDb = id,
                idDevice = 2,
                idMerenja = 3,
                timestamp = 12345,
                tip = 0,
                vrednost = 12.25
            });

            var res = serverService.GetMerenjeByDbId(id);

            Assert.NotNull(res);
            Assert.AreEqual(res.IdDb, id);
            Assert.AreEqual(res.IdDevice, 2);
            Assert.AreEqual(res.IdMerenja, 3);
            Assert.AreEqual(res.Timestamp, 12345);
            Assert.AreEqual(res.Tip, (Models.MerenjeTip)0);
            Assert.AreEqual(res.Vrednost, 12.25);

            mockCrud.Setup(x => x.GetMerenjeByDbId(id)).Returns((Server.Merenje)null);
           
            Exception ex = Assert.Throws<Exception>(() => { serverService.GetMerenjeByDbId(1); });
            Assert.That(ex.Message, Is.EqualTo("Merenje not found"));
        }

        [TestMethod]
        public void getTestTimestampsAnalog()
        {
            mockCrud.Setup(x => x.GetTimestampsAnalog()).Returns(
                new Dictionary<long, long>(){{1,1234},{2,5678},{3,891011}});

            var res = serverService.GetTimestampsAnalog();

            Assert.NotNull(res);
            Assert.AreEqual(3, res.Count);
            Assert.AreEqual(1234, res[1]);
            Assert.AreEqual(5678, res[2]);
            Assert.AreEqual(891011, res[3]);


            mockCrud.Setup(x => x.GetTimestampsAnalog()).Returns((Dictionary<long,long>)null);
            Assert.IsNull(serverService.GetTimestampsAnalog());

        }

        [TestMethod]
        public void getTestTimestampsDigital()
        {
            mockCrud.Setup(x => x.GetTimestampsDigital()).Returns(
                new Dictionary<long, long>() { { 1, 1234 }, { 2, 5678 }, { 3, 891011 } });

            var res = serverService.GetTimestampsDigital();

            Assert.NotNull(res);
            Assert.AreEqual(3, res.Count);
            Assert.AreEqual(1234, res[1]);
            Assert.AreEqual(5678, res[2]);
            Assert.AreEqual(891011, res[3]);

            mockCrud.Setup(x => x.GetTimestampsDigital()).Returns((Dictionary<long, long>)null);
            Assert.IsNull(serverService.GetTimestampsDigital());

        }


        [TestMethod]
        public void getTestTimestampPerDevice()
        {
            mockCrud.Setup(x => x.GetTimestampPerDevice()).Returns(
                new Dictionary<long, long>() { { 1, 1234 }, { 2, 5678 }, { 3, 891011 } });

            var res = serverService.GetTimestampPerDevice();

            Assert.NotNull(res);
            Assert.AreEqual(3, res.Count);
            Assert.AreEqual(1234, res[1]);
            Assert.AreEqual(5678, res[2]);
            Assert.AreEqual(891011, res[3]);

            mockCrud.Setup(x => x.GetTimestampPerDevice()).Returns((Dictionary<long, long>)null);
            Assert.IsNull(serverService.GetTimestampPerDevice());
        }

        [TestMethod]
        public void getTestAllTimestampsById()
        {
            int id = 1;
            mockCrud.Setup(x => x.GetAllTimestampsById(id)).Returns(
                new Dictionary<int, long>() { { 1, 1234 }, { 2, 5678 }, { 3, 891011 } });

            var res = serverService.GetAllTimestampsById(id);

            Assert.NotNull(res); 
            Assert.AreEqual(3, res.Count);
            Assert.AreEqual(1234, res[1]);
            Assert.AreEqual(5678, res[2]);
            Assert.AreEqual(891011, res[3]);

            mockCrud.Setup(x => x.GetAllTimestampsById(id)).Returns((Dictionary<int,long>)null);
            Assert.IsNull(serverService.GetAllTimestampsById(id));
        }

        [TestMethod]
        public void getTestLastTimestampById()
        {
            int id = 1;
            long timestamp = 1234;
            mockCrud.Setup(x => x.GetLastTimestampById(id)).Returns(timestamp);

            var res = serverService.GetLastTimestampById(id);

            Assert.NotNull(res);
            Assert.AreEqual(res, timestamp);

            mockCrud.Setup(x => x.GetLastTimestampById(id)).Returns(-1);
            Exception ex = Assert.Throws<Exception>(() => { serverService.GetLastTimestampById(1); });
            Assert.That(ex.Message, Is.EqualTo("Last timestamp not found."));
        }

        [TestMethod]
        public void getTestWriteDevice()
        {
            long timestamp = 1234;

            Server.Merenje merenje = new Server.Merenje()
            {
                idDb = 1,
                idDevice= 2,
                idMerenja = 3,
                timestamp = 1234,
                tip = 0,
                vrednost = 12.25
            };

            mockCrud.Setup(x => x.WriteDevice(merenje));
            mockCrud.Setup(x => x.GetLastTimestampById((int)merenje.idMerenja)).Returns(timestamp);

            Assert.IsNotNull(serverService.GetLastTimestampById((int)merenje.idMerenja));
            Assert.AreEqual(timestamp,serverService.GetLastTimestampById((int)merenje.idMerenja));


            Server.Merenje merenje2 = new Server.Merenje()
            {
                idDb = 2,
                idDevice = 3,
                idMerenja = 4,
                timestamp = 678910,
                tip = 1,
                vrednost = 2.25
            };

            serverService.WriteDevice(conversion.ConversionMerenje(merenje2));

            mockCrud.Setup(x => x.WriteDevice(merenje2));
            mockCrud.Setup(x => x.GetLastTimestampById((int)merenje2.idMerenja)).Returns(null);

            Assert.IsNotNull(serverService.GetLastTimestampById((int)merenje2.idMerenja));
            Assert.AreNotEqual(678910, serverService.GetLastTimestampById((int)merenje2.idMerenja));
        }

        [TestMethod]
        public void getTestLastMerenjeFromIdMerenje()
        {
            int id = 2;
            mockCrud.Setup(x => x.GetLastMerenjeFromIdMerenje(id)).Returns(new Server.Merenje()
            {
                idDb = 1,
                idDevice = id,
                idMerenja = 3,
                timestamp = 12345,
                tip = 0,
                vrednost = 12.25
            });

            var res = serverService.GetLastMerenjeFromIdMerenje(id);

            Assert.NotNull(res);
            Assert.AreEqual(res.IdDb, 1);
            Assert.AreEqual(res.IdDevice, id);
            Assert.AreEqual(res.IdMerenja, 3);
            Assert.AreEqual(res.Timestamp, 12345);
            Assert.AreEqual(res.Tip, (Models.MerenjeTip)0);
            Assert.AreEqual(res.Vrednost, 12.25);

            mockCrud.Setup(x => x.GetLastMerenjeFromIdMerenje(id)).Returns((Server.Merenje)null);
            Exception ex = Assert.Throws<Exception>(() => { serverService.GetLastMerenjeFromIdMerenje(id); });
            Assert.That(ex.Message, Is.EqualTo("Merenje not found"));
        }

        [TestMethod]
        public void getTestLastForDeviceId()
        {
            int id = 2;
            mockCrud.Setup(x => x.GetLastForDeviceId(id)).Returns(new Server.Merenje()
            {
                idDb = 1,
                idDevice = id,
                idMerenja = 3,
                timestamp = 12345,
                tip = 0,
                vrednost = 12.25
             });

            var res = serverService.GetLastForDeviceId(id);

            Assert.NotNull(res);
            Assert.AreEqual(res.IdDb, 1);
            Assert.AreEqual(res.IdDevice, id);
            Assert.AreEqual(res.IdMerenja, 3);
            Assert.AreEqual(res.Timestamp, 12345);
            Assert.AreEqual(res.Tip, (Models.MerenjeTip)0);
            Assert.AreEqual(res.Vrednost, 12.25);

            mockCrud.Setup(x => x.GetLastForDeviceId(id)).Returns((Server.Merenje)null);
            Exception ex = Assert.Throws<Exception>(() => { serverService.GetLastForDeviceId(id); });
            Assert.That(ex.Message, Is.EqualTo("Nije nadjeno merenje za uredjaj."));
        }
    }
}
