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
    
    public partial class tblStockDetail
    {
        public int StockId { get; set; }
        public Nullable<long> ProductId { get; set; }
        public Nullable<decimal> OpeningQty { get; set; }
        public Nullable<System.DateTime> OpeningQtyDate { get; set; }
        public Nullable<decimal> InwardQty { get; set; }
        public Nullable<decimal> InwardPrice { get; set; }
        public Nullable<decimal> OutwardQty { get; set; }
        public Nullable<decimal> OutwardPrice { get; set; }
        public Nullable<long> DocumentNo { get; set; }
        public string TransactionType { get; set; }
        public bool IsDelete { get; set; }
        public string BatchNo { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<decimal> InwardAmount { get; set; }
        public Nullable<decimal> OutwardAmount { get; set; }
        public Nullable<bool> IsOpeningStockEntry { get; set; }
    }
}
