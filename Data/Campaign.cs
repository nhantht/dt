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
    
    public partial class Campaign
    {
        public Campaign()
        {
            this.Categories = new HashSet<Category>();
        }
    
        public decimal CampaignId { get; set; }
        public string Name { get; set; }
        public string CompanyId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
        public decimal Budget { get; set; }
    
        public virtual ICollection<Category> Categories { get; set; }
        public virtual User User { get; set; }
    }
}