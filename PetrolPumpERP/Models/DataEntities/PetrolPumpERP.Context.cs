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
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PetrolPumpERPEntities : DbContext
    {
        public PetrolPumpERPEntities()
            : base("name=PetrolPumpERPEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tbl_CustomerInfo> tbl_CustomerInfo { get; set; }
        public virtual DbSet<tbl_ReceiptDetails> tbl_ReceiptDetails { get; set; }
        public virtual DbSet<tblAccountType> tblAccountTypes { get; set; }
        public virtual DbSet<tblAccountTypeDetail> tblAccountTypeDetails { get; set; }
        public virtual DbSet<tblAllowanceDeduction> tblAllowanceDeductions { get; set; }
        public virtual DbSet<tblCustomeType> tblCustomeTypes { get; set; }
        public virtual DbSet<tblDebitNote> tblDebitNotes { get; set; }
        public virtual DbSet<tblDesignationMaster> tblDesignationMasters { get; set; }
        public virtual DbSet<tblFinancialMaster> tblFinancialMasters { get; set; }
        public virtual DbSet<tblLedger> tblLedgers { get; set; }
        public virtual DbSet<tblMainLedger> tblMainLedgers { get; set; }
        public virtual DbSet<tblProductType> tblProductTypes { get; set; }
        public virtual DbSet<tblPurchaseOrder> tblPurchaseOrders { get; set; }
        public virtual DbSet<tblPurchaseOrderDetail> tblPurchaseOrderDetails { get; set; }
        public virtual DbSet<tblSalary> tblSalaries { get; set; }
        public virtual DbSet<tblSalaryDetail> tblSalaryDetails { get; set; }
        public virtual DbSet<tblSubLedger> tblSubLedgers { get; set; }
        public virtual DbSet<tblSupplierMaster> tblSupplierMasters { get; set; }
        public virtual DbSet<tblUnitMaster> tblUnitMasters { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<tblUserType> tblUserTypes { get; set; }
        public virtual DbSet<tblOpeningBalance> tblOpeningBalances { get; set; }
        public virtual DbSet<tblProductMaster> tblProductMasters { get; set; }
        public virtual DbSet<tblWareHouse> tblWareHouses { get; set; }
        public virtual DbSet<tblBankMaster> tblBankMasters { get; set; }
        public virtual DbSet<tblPurchaseInvoice> tblPurchaseInvoices { get; set; }
        public virtual DbSet<tbl_InvoiceDetail> tbl_InvoiceDetail { get; set; }
        public virtual DbSet<tblDebitNoteDetail> tblDebitNoteDetails { get; set; }
        public virtual DbSet<tblPurchaseInvoiceDetail> tblPurchaseInvoiceDetails { get; set; }
        public virtual DbSet<tblStockDetail> tblStockDetails { get; set; }
        public virtual DbSet<tblOtherAccount> tblOtherAccounts { get; set; }
        public virtual DbSet<tblDepartment> tblDepartments { get; set; }
        public virtual DbSet<tblSwipeMachine> tblSwipeMachines { get; set; }
        public virtual DbSet<tblEmployee> tblEmployees { get; set; }
        public virtual DbSet<tblAttendance> tblAttendances { get; set; }
        public virtual DbSet<tblTransactionType> tblTransactionTypes { get; set; }
        public virtual DbSet<tblTransaction> tblTransactions { get; set; }
        public virtual DbSet<tbl_Invoice> tbl_Invoice { get; set; }
    
        public virtual ObjectResult<STP_GetAllOpeningBalLedgers_Result> STP_GetAllOpeningBalLedgers()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<STP_GetAllOpeningBalLedgers_Result>("STP_GetAllOpeningBalLedgers");
        }
    
        public virtual int STP_InsertLedger(string ledgerName, string address, string accountNo, string panNo, Nullable<System.DateTime> accOpenDate, Nullable<long> subLedgerId, string emailID, string phoneNo, string mobileNo, Nullable<long> financialYearId, string miscCode, string iFSCCode, string bankBranchName, Nullable<bool> uploaded, string city, string area, string pinCode, string acType, Nullable<int> acTypeId)
        {
            var ledgerNameParameter = ledgerName != null ?
                new ObjectParameter("LedgerName", ledgerName) :
                new ObjectParameter("LedgerName", typeof(string));
    
            var addressParameter = address != null ?
                new ObjectParameter("Address", address) :
                new ObjectParameter("Address", typeof(string));
    
            var accountNoParameter = accountNo != null ?
                new ObjectParameter("AccountNo", accountNo) :
                new ObjectParameter("AccountNo", typeof(string));
    
            var panNoParameter = panNo != null ?
                new ObjectParameter("PanNo", panNo) :
                new ObjectParameter("PanNo", typeof(string));
    
            var accOpenDateParameter = accOpenDate.HasValue ?
                new ObjectParameter("AccOpenDate", accOpenDate) :
                new ObjectParameter("AccOpenDate", typeof(System.DateTime));
    
            var subLedgerIdParameter = subLedgerId.HasValue ?
                new ObjectParameter("SubLedgerId", subLedgerId) :
                new ObjectParameter("SubLedgerId", typeof(long));
    
            var emailIDParameter = emailID != null ?
                new ObjectParameter("EmailID", emailID) :
                new ObjectParameter("EmailID", typeof(string));
    
            var phoneNoParameter = phoneNo != null ?
                new ObjectParameter("PhoneNo", phoneNo) :
                new ObjectParameter("PhoneNo", typeof(string));
    
            var mobileNoParameter = mobileNo != null ?
                new ObjectParameter("MobileNo", mobileNo) :
                new ObjectParameter("MobileNo", typeof(string));
    
            var financialYearIdParameter = financialYearId.HasValue ?
                new ObjectParameter("FinancialYearId", financialYearId) :
                new ObjectParameter("FinancialYearId", typeof(long));
    
            var miscCodeParameter = miscCode != null ?
                new ObjectParameter("MiscCode", miscCode) :
                new ObjectParameter("MiscCode", typeof(string));
    
            var iFSCCodeParameter = iFSCCode != null ?
                new ObjectParameter("IFSCCode", iFSCCode) :
                new ObjectParameter("IFSCCode", typeof(string));
    
            var bankBranchNameParameter = bankBranchName != null ?
                new ObjectParameter("BankBranchName", bankBranchName) :
                new ObjectParameter("BankBranchName", typeof(string));
    
            var uploadedParameter = uploaded.HasValue ?
                new ObjectParameter("Uploaded", uploaded) :
                new ObjectParameter("Uploaded", typeof(bool));
    
            var cityParameter = city != null ?
                new ObjectParameter("City", city) :
                new ObjectParameter("City", typeof(string));
    
            var areaParameter = area != null ?
                new ObjectParameter("Area", area) :
                new ObjectParameter("Area", typeof(string));
    
            var pinCodeParameter = pinCode != null ?
                new ObjectParameter("PinCode", pinCode) :
                new ObjectParameter("PinCode", typeof(string));
    
            var acTypeParameter = acType != null ?
                new ObjectParameter("AcType", acType) :
                new ObjectParameter("AcType", typeof(string));
    
            var acTypeIdParameter = acTypeId.HasValue ?
                new ObjectParameter("AcTypeId", acTypeId) :
                new ObjectParameter("AcTypeId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("STP_InsertLedger", ledgerNameParameter, addressParameter, accountNoParameter, panNoParameter, accOpenDateParameter, subLedgerIdParameter, emailIDParameter, phoneNoParameter, mobileNoParameter, financialYearIdParameter, miscCodeParameter, iFSCCodeParameter, bankBranchNameParameter, uploadedParameter, cityParameter, areaParameter, pinCodeParameter, acTypeParameter, acTypeIdParameter);
        }
    }
}
