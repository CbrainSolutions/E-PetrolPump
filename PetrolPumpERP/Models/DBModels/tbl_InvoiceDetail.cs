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
    
    public partial class tbl_InvoiceDetail
    {
        public int SRNO { get; set; }
        public Nullable<int> InvoiceNo { get; set; }
        public string ItemCode { get; set; }
        public int Quantity { get; set; }
        public string BatchNo { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public decimal Rate { get; set; }
        public decimal ProductAmount { get; set; }
        public Nullable<decimal> VAT { get; set; }
        public Nullable<decimal> VATAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
    }
}
