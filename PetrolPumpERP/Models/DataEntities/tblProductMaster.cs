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
    
    public partial class tblProductMaster
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string UOM { get; set; }
        public string SubUOM { get; set; }
        public decimal Price { get; set; }
        public bool IsReusable { get; set; }
        public bool IsDelete { get; set; }
    }
}
