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
    
    public partial class tblFinancialMaster
    {
        public long FinancialYearId { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> YearStartDate { get; set; }
        public Nullable<System.DateTime> YearEndDate { get; set; }
        public string FirmName { get; set; }
        public string Address { get; set; }
    }
}