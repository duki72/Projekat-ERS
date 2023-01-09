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

