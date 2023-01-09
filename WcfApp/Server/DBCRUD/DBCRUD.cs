using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.DBCRUD
{
    [ExcludeFromCodeCoverage]
    public class DBCRUD : IDBCRUD
    {
        public IDictionary<int, long> GetAllTimestampsById(int id)
        {
            try
            {
                using (var db = new MerenjeEntities())
                {
                    return db.Merenjes
                        .Where(x => x.idMerenja == id)
                        .ToDictionary(x => (int)x.idDb, x => x.timestamp);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Dictionary<int, long>();
            }
        }

        public Merenje GetLastForDeviceId(int id)
        {
            try
            {
                using (var db = new MerenjeEntities())
                {
                    Merenje merenje = db.Merenjes
                        .Where(x => x.idDevice == id)
                        .OrderByDescending(x => x.timestamp)
                        .FirstOrDefault();
                    return merenje;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public Merenje GetLastMerenjeFromIdMerenje(int id)
        {
            try
            {
                using (var db = new MerenjeEntities())
                {
                    return db.Merenjes
                        .Where(x => x.idMerenja == id)
                        .OrderByDescending(x => x.timestamp)
                        .FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public long GetLastTimestampById(int id)
        {
            try
            {
                using (var db = new MerenjeEntities())
                {
                    return db.Merenjes
                        .Where(x => x.idMerenja == id)
                        .OrderByDescending(x => x.timestamp)
                        .Select(x => x.timestamp)
                        .FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }
        }

        public Merenje GetMerenjeByDbId(int dbId)
        {
            try
            {
                using (var db = new MerenjeEntities())
                {
                    return db.Merenjes
                        .Where(x => x.idDb == dbId)
                        .FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public IDictionary<long, long> GetTimestampPerDevice()
        {
            Dictionary<long, long> retVal = new Dictionary<long, long>();
            try
            {
                using (var db = new MerenjeEntities())
                {
                    db.Merenjes.Select(x => x.idDevice)
                        .ToList()
                        .ForEach(idDev =>
                        {
                            var timestamp = db.Merenjes
                                .Where(x => x.idDevice == idDev)
                                .OrderByDescending(x => x.timestamp)
                                .Select(x => x.timestamp)
                                .FirstOrDefault();
                            
                            if (retVal.ContainsKey((long)idDev))
                            {
                                retVal[(long)idDev] = timestamp;
                            }
                            else
                            {
                                retVal.Add((long)idDev, timestamp);
                            }
                        });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return retVal;
        }

        public IDictionary<long, long> GetTimestampsAnalog()
        {
            try
            {
                using (var db = new MerenjeEntities())
                {
                    return db.Merenjes
                        .Where(x => x.tip == ((int)Models.MerenjeTip.ANALOGNO))
                        .ToDictionary(x => x.idDb, x => x.timestamp);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Dictionary<long, long>();
            }
        }

        public IDictionary<long, long> GetTimestampsDigital()
        {
            try
            {
                using (var db = new MerenjeEntities())
                {
                    return db.Merenjes
                        .Where(x => x.tip == ((int)Models.MerenjeTip.DIGITALNO))
                        .ToDictionary(x => x.idDb, x => x.timestamp);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Dictionary<long, long>();
            }
        }

        public void WriteDevice(Merenje merenje)
        {
            try
            {
                using (var db = new MerenjeEntities())
                {
                    db.Merenjes.Add(merenje);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
