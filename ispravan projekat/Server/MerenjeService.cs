using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static Server.Conversion;

namespace Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MerenjeService : Models.IServerMerenjeService, Models.IWrite
    {
        private Conversion conversion = new Conversion();
        private DBCRUD.IDBCRUD crud { get; set; } = null;
        public MerenjeService(DBCRUD.IDBCRUD crud) : base()
        {
            this.crud = crud;
        }

        [OperationBehavior]
        public IDictionary<int, long> GetAllTimestampsById(int id)
        {
            IDictionary<int, long> dict = crud.GetAllTimestampsById(id);
            ProxyLogger.log.Info($"Server citanje za {id},svi timestampovi:{dict}");

            return crud.GetAllTimestampsById(id);
        }

        [OperationBehavior]
        public long GetLastTimestampById(int id)
        {
            long lastTimestamp = crud.GetLastTimestampById(id);
            if (lastTimestamp == -1)
            {
                throw new Exception("Last timestamp not found.");
            }
            ProxyLogger.log.Info($"Server citanje za {id},poslednji timestamp:{lastTimestamp}");
            return lastTimestamp;
        }

        [OperationBehavior]
        public IDictionary<long, long> GetTimestampPerDevice()
        {
            IDictionary<long, long> dict = crud.GetTimestampPerDevice();
            ProxyLogger.log.Info($"Server citanje po device-u za sve timestampove:{dict}");

            return crud.GetTimestampPerDevice();
        }

        [OperationBehavior]
        public IDictionary<long, long> GetTimestampsAnalog()
        {
            IDictionary<long, long> dict = crud.GetTimestampsAnalog();
            ProxyLogger.log.Info($"Server citanje analognih merenja svih timestampova:{dict}");

            return crud.GetTimestampsAnalog();
        }

        [OperationBehavior]
        public IDictionary<long, long> GetTimestampsDigital()
        {

            IDictionary<long, long> dict = crud.GetTimestampsDigital();
            ProxyLogger.log.Info($"Server citanje digitalnih merenja svih timestampova:{dict}");

            return crud.GetTimestampsDigital();
        }

        [OperationBehavior]
        public void WriteDevice(Models.Merenje merenje)
        {
            crud.WriteDevice(
                new Merenje()
                {
                    idDevice = merenje.IdDevice,
                    idMerenja = merenje.IdMerenja,
                    timestamp = merenje.Timestamp,
                    tip = (int?)merenje.Tip,
                    vrednost = merenje.Vrednost
                }
            );

            ProxyLogger.log.Info($"Server upis novog merenja : {merenje}");
        }
        
        [OperationBehavior]
        public Models.Merenje GetMerenjeByDbId(int dbId)
        {
            try
            {
                ProxyLogger.log.Info($"Server citanje merenja po id-u : {dbId}");
                return conversion.ConversionMerenje(crud.GetMerenjeByDbId(dbId));

            }
            catch (Exception)
            {
                throw new Exception("Merenje not found");
            }
        }

        public Models.Merenje GetLastMerenjeFromIdMerenje(int id)
        {
            try
            {

                ProxyLogger.log.Info($"Server citanje merenja po id-u merenja : {id}");
                return conversion.ConversionMerenje(crud.GetLastMerenjeFromIdMerenje(id));
            }
            catch (Exception)
            {
                throw new Exception("Merenje not found");
            }
        }

        [OperationBehavior]
        public Models.Merenje GetLastForDeviceId(int id)
        {
            try
            {
                var merenje = crud.GetLastForDeviceId(id);
                ProxyLogger.log.Info($"Server citanje poslednje vrednosti za id: {id} device.");
                return conversion.ConversionMerenje(merenje);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("Nije nadjeno merenje za uredjaj.");
            }
        }

    }
}
