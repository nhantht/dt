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
    
    public partial class DeviceTypeURL
    {
        public decimal URLId { get; set; }
        public decimal DeviceTypeId { get; set; }
        public string PictureURL { get; set; }
        public string Title { get; set; }
    
        public virtual DeviceType DeviceType { get; set; }
        public virtual URL URL { get; set; }
    }
}
