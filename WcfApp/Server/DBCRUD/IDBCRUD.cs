using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DBCRUD
{
    public class IDBCRUD
    {
        IDictionary<int, long> GetAllTimestampsById(int id);
        long GetLastTimestampById(int id);
        IDictionary<long, long> GetTimestampPerDevice();
        IDictionary<long, long> GetTimestampsAnalog();
        IDictionary<long, long> GetTimestampsDigital();
        Merenje GetMerenjeByDbId(int dbId);
        Merenje GetLastForDeviceId(int id);
        void WriteDevice(Server.Merenje merenje);
        Merenje GetLastMerenjeFromIdMerenje(int id);
    }
}
