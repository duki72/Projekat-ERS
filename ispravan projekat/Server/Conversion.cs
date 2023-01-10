using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Conversion
    {
        public class ProxyLogger
        {
            public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }


        public Models.Merenje ConversionMerenje(Merenje merenje)
        {
            return new Models.Merenje()
            {
                IdDb = (int)merenje.idDb,
                IdDevice = (int)merenje.idDevice,
                IdMerenja = (int)merenje.idMerenja,
                Timestamp = merenje.timestamp,
                Tip = (MerenjeTip)merenje.tip,
                Vrednost = (double)merenje.vrednost
            };
        }
    }
}
