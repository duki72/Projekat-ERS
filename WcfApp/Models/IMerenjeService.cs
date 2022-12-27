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
        ICollection<Models.Merenje> GetMerenjes();
    }

}
