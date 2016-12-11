﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PetrolPumpERPEntities : DbContext
    {
        public PetrolPumpERPEntities()
            : base("name=PetrolPumpERPEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_Invoice> tbl_Invoice { get; set; }
        public virtual DbSet<tbl_InvoiceDetail> tbl_InvoiceDetail { get; set; }
        public virtual DbSet<tbl_ReceiptDetails> tbl_ReceiptDetails { get; set; }
        public virtual DbSet<tblAllowanceDeduction> tblAllowanceDeductions { get; set; }
        public virtual DbSet<tblDebitNote> tblDebitNotes { get; set; }
        public virtual DbSet<tblDebitNoteDetail> tblDebitNoteDetails { get; set; }
        public virtual DbSet<tblDesignationMaster> tblDesignationMasters { get; set; }
        public virtual DbSet<tblEmployee> tblEmployees { get; set; }
        public virtual DbSet<tblFinancialMaster> tblFinancialMasters { get; set; }
        public virtual DbSet<tblMainLedger> tblMainLedgers { get; set; }
        public virtual DbSet<tblProductMaster> tblProductMasters { get; set; }
        public virtual DbSet<tblPurchaseInvoice> tblPurchaseInvoices { get; set; }
        public virtual DbSet<tblPurchaseInvoiceDetail> tblPurchaseInvoiceDetails { get; set; }
        public virtual DbSet<tblPurchaseOrder> tblPurchaseOrders { get; set; }
        public virtual DbSet<tblPurchaseOrderDetail> tblPurchaseOrderDetails { get; set; }
        public virtual DbSet<tblSalary> tblSalaries { get; set; }
        public virtual DbSet<tblSalaryDetail> tblSalaryDetails { get; set; }
        public virtual DbSet<tblStockDetail> tblStockDetails { get; set; }
        public virtual DbSet<tblSubLedger> tblSubLedgers { get; set; }
        public virtual DbSet<tblTransaction> tblTransactions { get; set; }
        public virtual DbSet<tblTransactionType> tblTransactionTypes { get; set; }
        public virtual DbSet<tblUnitMaster> tblUnitMasters { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<tblUserType> tblUserTypes { get; set; }
        public virtual DbSet<tblCustomeType> tblCustomeTypes { get; set; }
        public virtual DbSet<tblAccountType> tblAccountTypes { get; set; }
        public virtual DbSet<tblAccountTypeDetail> tblAccountTypeDetails { get; set; }
        public virtual DbSet<tblLedger> tblLedgers { get; set; }
        public virtual DbSet<tbl_CustomerInfo> tbl_CustomerInfo { get; set; }
        public virtual DbSet<tblSupplierMaster> tblSupplierMasters { get; set; }
    }
}
