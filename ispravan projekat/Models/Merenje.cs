using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public enum MerenjeTip
    {
        ANALOGNO = 0,
        DIGITALNO = 1
    }

    [DataContract]
    public class Merenje
    {
        [DataMember]
        public int IdMerenja { get; set; }
        [DataMember]
        public MerenjeTip Tip { get; set; }
        [DataMember]
        public int IdDb { get; set; }
        [DataMember]
        public int IdDevice { get; set; }
        [DataMember]
        public double Vrednost { get; set; }
        [DataMember]
        public long Timestamp { get; set; }

        public override string ToString()
        {
            return $@"
            Merenje {IdMerenja} 
            Timestamp: {Timestamp}, Vrednost: {Vrednost}, Tip: {Tip.ToString()}
            ";
        }
    }

}
