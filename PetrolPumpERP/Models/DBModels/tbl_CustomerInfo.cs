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
    
    public partial class tbl_CustomerInfo
    {
        public int CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string VATNo { get; set; }
        public string CSTNo { get; set; }
        public Nullable<System.DateTime> WEFDate { get; set; }
        public Nullable<System.DateTime> ChangeDate { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal TotalReceivedAmount { get; set; }
        public decimal TotalInvoiceAmount { get; set; }
    }
}
