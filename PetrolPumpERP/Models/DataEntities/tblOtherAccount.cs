//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PetrolPumpERP.Models.DataEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblOtherAccount
    {
        public long OtherAccountId { get; set; }
        public string AccountName { get; set; }
        public Nullable<long> LedgerId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}
