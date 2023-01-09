using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    class ProxyLogger
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }


    /// <summary>
    /// 
    /// </summary>
    public class ProxyMerenje
    {
        public Models.Merenje MerenjeInfo { get; set; }
        public DateTime PoslednjiPutDobavljeno { get; set; }
        public DateTime PoslednjiPutProcitano { get; set; }
        public ProxyMerenje() { }
    }
}

