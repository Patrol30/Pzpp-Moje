//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pzpp
{
    using System;
    using System.Collections.Generic;
    
    public partial class Responses
    {
        public int Response_ID { get; set; }
        public Nullable<int> PC_ID { get; set; }
        public Nullable<bool> Value { get; set; }
        public Nullable<System.DateTimeOffset> Time { get; set; }
    
        public virtual Computers Computers { get; set; }
    }
}
