using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    [ServiceContract]
    public interface IProxyService : Models.IMerenjeService
    {
        [OperationContract]
        void CheckRemovals();
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ProxyService : IProxyService
    {
        Models.IServerMerenjeService ServerService = null;
        private List<ProxyMerenje> LocalStorage = new List<ProxyMerenje>();

        public ProxyService(Models.IServerMerenjeService serverService)
        {
            ServerService = serverService;
        }

        public List<ProxyMerenje> GetLocalStorage()
        {
            lock (LocalStorage)
            {
                return LocalStorage;
            }
        }
        public void SetLocalStorage(List<ProxyMerenje> merenja)
        {
            lock (LocalStorage)
            {
                LocalStorage = merenja;
            }
        }


        [OperationBehavior]
        public void CheckRemovals()
        {
            ProxyLogger.log.Info("Trying to remove instances that are older than 1 day in proxy service.");
            lock (LocalStorage)
            {
                LocalStorage.RemoveAll(x => x.PoslednjiPutProcitano.CompareTo(DateTime.Now.AddDays(-1)) < 0);
            }
        }

        public void UpdateLocalStorageWithNewMerenje(Models.Merenje merenje)
        {
            ProxyLogger.log.Info($"Proxy service updating local storage with new merenje:{merenje}");
            lock (LocalStorage)
            {
                LocalStorage.RemoveAll(x => x.MerenjeInfo.IdDb == merenje.IdDb);
                LocalStorage.Add(new ProxyMerenje()
                {
                    MerenjeInfo = merenje,
                    PoslednjiPutDobavljeno = DateTime.Now,
                    PoslednjiPutProcitano = new DateTime(merenje.Timestamp)
                });
            }
        }

        [OperationBehavior]
        public ICollection<Merenje> GetAllById(int id)
        {
            var lastUpdatedForId = ServerService.GetAllTimestampsById(id);
            ProxyLogger.log.Info($"Proxy retreived timestamps from server for merenje id: {id}");

            List<Merenje> retVal = new List<Merenje>();

            foreach (var kv in lastUpdatedForId)
            {
                ProxyMerenje local = null;
                lock (LocalStorage)
                {
                    if (LocalStorage.Count(x => x.MerenjeInfo.IdDb == id) > 0)
                    {
                        local = LocalStorage.Find(x => x.MerenjeInfo.IdDb == id);
                    }
                }

                if (local != null)
                {
                    // local copy exists but it's not up to date
                    if (local.PoslednjiPutDobavljeno.Ticks < kv.Value)
                    {
                        ProxyLogger.log.Info($"Proxy's merenje:{local} is NOT up to date, retreiving new one from server.");
                        // but it's not up to date
                        var newMerenje = GetMerenjeByDbId(kv.Key);
                        UpdateLocalStorageWithNewMerenje(newMerenje);
                        retVal.Add(newMerenje);
                    }
                    // local copy exists and it's up to date
                    else
                    {
                        ProxyLogger.log.Info($"Proxy's merenje:{local} is up to date.");
                        retVal.Add(local.MerenjeInfo);
                    }
                }
                else
                {
                    ProxyLogger.log.Info($"Proxy's doesn't have local copy, retreivin new one from server.");
                    // get new object from db with current iddb 
                    var newMerenje = GetMerenjeByDbId(kv.Key);
                    UpdateLocalStorageWithNewMerenje(newMerenje);
                    retVal.Add(newMerenje);
                }
            }

            return retVal;
        }

        [OperationBehavior]
        private Merenje GetMerenjeByDbId(int dbId)
        {
            return ServerService.GetMerenjeByDbId(dbId);
        }

        [OperationBehavior]
        public double GetAzuriranuVrednost(int id)
        {
            try
            {
                ProxyLogger.log.Info($"Proxy's retreiveing up to date version for:{id}.");

                ProxyMerenje localLastInstance = null;
                lock (LocalStorage)
                {
                    if (LocalStorage.Where(x => x.MerenjeInfo.IdMerenja == id).Count() > 0)
                    {
                        localLastInstance = LocalStorage
                            .Where(x => x.MerenjeInfo.IdMerenja == id)
                            .OrderByDescending(x => x.MerenjeInfo.Timestamp)
                            .FirstOrDefault();
                    }
                }

                if (localLastInstance != null)
                {
                    var dbLasttimestamp = ServerService.GetLastTimestampById(id);
                    if (localLastInstance.MerenjeInfo.Timestamp < dbLasttimestamp)
                    {
                        ProxyLogger.log.Info($"Proxy's merenje:{localLastInstance} is NOT up to date, retreiving new one from server.");
                        var newMerenje = GetMerenjeByDbId(localLastInstance.MerenjeInfo.IdDb);
                        UpdateLocalStorageWithNewMerenje(newMerenje);
                        return newMerenje.Vrednost;
                    }
                    else
                    {
                        ProxyLogger.log.Info($"Proxy's merenje:{localLastInstance} is up to date.");
                        // we have the same value
                        return localLastInstance.MerenjeInfo.Vrednost;
                    }
                }
                else
                {
                    // TO DO: Get Last merenje object by id
                    var newMerenje = ServerService.GetLastMerenjeFromIdMerenje(id);
                    ProxyLogger.log.Info($"Proxy's doesn't have local copy, retreivin new one from server.");
                    UpdateLocalStorageWithNewMerenje(newMerenje);
                    // we need new value from db
                    return newMerenje.Vrednost;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ProxyLogger.log.Error("Error occured in proxy service", e);
                throw e;
            }
        }

    }
}
        

