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

    }
}
