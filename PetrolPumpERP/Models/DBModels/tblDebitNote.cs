//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PetrolPumpERP.Models.DBModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblDebitNote
    {
        public int DNNo { get; set; }
        public Nullable<System.DateTime> DNDate { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> Tax1 { get; set; }
        public Nullable<int> Tax2 { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
    }
}
