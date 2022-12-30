using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Konekcije
{
    public abstract class IKonekcija<T>
    {
        public abstract T Service { get; set; }
    }
}
