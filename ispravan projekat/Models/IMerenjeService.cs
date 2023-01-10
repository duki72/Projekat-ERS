using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [ServiceContract]
    public interface IMerenjeService
    {
        [OperationContract]
        ICollection<Models.Merenje> GetAllById(int id);


        [OperationContract]
        double GetAzuriranuVrednost(int id);

        [OperationContract]
        List<double?> GetAzuriraneVrednostiUredjaja();


        [OperationContract]
        List<Merenje> GetAnalogni();

        
        [OperationContract]
        List<Merenje> GetDigitalni();

    }

    [ServiceContract]
    public interface IServerMerenjeService
    {
        /// <summary>
        /// Vraca sve id-ijeve iz baze, da bi se ti objekti mogli pronaci
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IdDb -> Timestamp</returns>
        [OperationContract]
        IDictionary<int, long> GetAllTimestampsById(int id);

        [OperationContract]
        long GetLastTimestampById(int id);

        [OperationContract]
        Merenje GetLastMerenjeFromIdMerenje(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <returns>IdDevice -> Timestamp</returns>
        [OperationContract]
        IDictionary<long, long> GetTimestampPerDevice();

        [OperationContract]
        IDictionary<long, long> GetTimestampsAnalog();

        [OperationContract]
        IDictionary<long, long> GetTimestampsDigital();

        [OperationContract]
        Models.Merenje GetMerenjeByDbId(int dbId);


        [OperationContract]
        Merenje GetLastForDeviceId(int id);

    }

    [ServiceContract]
    public interface IWrite
    {
        [OperationContract]
        void WriteDevice(Models.Merenje merenje);
    }
}
