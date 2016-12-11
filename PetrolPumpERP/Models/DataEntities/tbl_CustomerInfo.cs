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
    
    public partial class tbl_CustomerInfo
    {
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerMName { get; set; }
        public string CustomeLName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Pin { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string MobileNo { get; set; }
        public string EMailId { get; set; }
        public Nullable<int> CustomerTypeId { get; set; }
        public Nullable<bool> DuplicateBill { get; set; }
        public Nullable<bool> SummaryofBills { get; set; }
        public string ContactPerson { get; set; }
        public Nullable<bool> VehicleWiseBill { get; set; }
        public string BillingCycle { get; set; }
        public Nullable<decimal> CreditLimit { get; set; }
        public Nullable<int> ChargesPercent { get; set; }
        public Nullable<decimal> SecurityDeposit { get; set; }
        public Nullable<int> NoofCreditDays { get; set; }
        public Nullable<bool> IsSeperateBill { get; set; }
        public Nullable<bool> IsRoundOff { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<long> LedgerId { get; set; }
    }
}
