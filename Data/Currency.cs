//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Currency
    {
        public Currency()
        {
            this.PriceURLs = new HashSet<PriceURL>();
            this.URLs = new HashSet<URL>();
        }
    
        public decimal Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<PriceURL> PriceURLs { get; set; }
        public virtual ICollection<URL> URLs { get; set; }
    }
}