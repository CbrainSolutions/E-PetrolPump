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
    
    public partial class tblSalaryDetail
    {
        public int SalDetail_Id { get; set; }
        public int SalId { get; set; }
        public System.DateTime SalDate { get; set; }
        public int AllowanceDeduction_Id { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaymentDone { get; set; }
        public bool IsDelete { get; set; }
    }
}