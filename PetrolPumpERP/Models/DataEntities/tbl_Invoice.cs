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
    
    public partial class tbl_Invoice
    {
        public long InvoiceNo { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public long CustomerLedgerId { get; set; }
        public bool ISCASH { get; set; }
        public string CustName { get; set; }
        public string CustAddress { get; set; }
        public string CustContactNo { get; set; }
        public decimal NetVAT { get; set; }
        public decimal NetInvoiceAmount { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public decimal NetAmount { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> IsRoundOff { get; set; }
        public Nullable<long> SwipeMachineId { get; set; }
    }
}
