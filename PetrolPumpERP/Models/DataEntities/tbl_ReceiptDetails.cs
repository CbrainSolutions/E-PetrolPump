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
    
    public partial class tbl_ReceiptDetails
    {
        public int ReceiptNo { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public Nullable<System.DateTime> VoucherDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ReceiptMode { get; set; }
        public string BankName { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<System.DateTime> ChequeDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
