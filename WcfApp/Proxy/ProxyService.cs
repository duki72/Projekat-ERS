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

