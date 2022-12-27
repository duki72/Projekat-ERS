using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MerenjeService : IMerenjeService
    {
        [OperationBehavior]
        public ICollection<Models.Merenje> GetMerenjes()
        {
            using (var db = new MerenjeResEntities())
            {
                return db.Merenjes.ToList().Select(x =>
                {
                    return new Models.Merenje()
                    {
                        Id = (int)x.id,
                        Timestamp = x.timestamp,
                        Tip = (MerenjeTip)x.tip,
                        Vrednost = (double)x.vrednost
                    };
                }).ToList();
            }

        }
    }
}
