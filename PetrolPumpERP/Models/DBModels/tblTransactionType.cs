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
    
    public partial class tblTransactionType
    {
        public long TransactionTypeId { get; set; }
        public string TransactionType { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> IsModify { get; set; }
    }
}
