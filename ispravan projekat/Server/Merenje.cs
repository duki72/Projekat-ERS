//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Server
{
    using System;
    using System.Collections.Generic;
    
    public partial class Merenje
    {
        public Nullable<int> tip { get; set; }
        public Nullable<double> vrednost { get; set; }
        public long timestamp { get; set; }
        public long idDb { get; set; }
        public Nullable<long> idMerenja { get; set; }
        public Nullable<long> idDevice { get; set; }
    }
}
