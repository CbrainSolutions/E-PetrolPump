using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    

    public class CustomerModel
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
        public long? SubledgerId { get; set; }
        public string CustomerType { get; set; }

        public int? AccountTypeId { get; set; }

        //public decimal? OpeningBalance { get; set; }

        //public string BalType { get; set; }
    }

    public class CustomerTypeResponse : Error
    {
        public IQueryable<tblCustomeType> CustomerTypeList { get; set; }
    }

    public class CustomerResponse : Error
    {
        public IQueryable<CustomerModel> CustomerList { get; set; }
    }

    public class CustomerModelBL
    {
        public static CustomerModelBL _userBl = null;
        PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private CustomerModelBL()
        {

        }

        public static CustomerModelBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new CustomerModelBL();
                }
                return _userBl;
            }
        }

        public CustomerTypeResponse GetCustomerTypes()
        {
            CustomerTypeResponse response = new CustomerTypeResponse();
            response.CustomerTypeList = _db.tblCustomeTypes;
            return response;
        }

        public CustomerResponse GetAllCustomers()
        {
            CustomerResponse response = new CustomerResponse() { Status = false };
            response.CustomerList = from tbl in _db.tbl_CustomerInfo
                                    join tblctype in _db.tblCustomeTypes
                                    on tbl.CustomerTypeId equals tblctype.CustomerTypeId
                                    join tblledger in _db.tblLedgers
                                    on tbl.LedgerId equals tblledger.LedgerId
                                    
                                    where tbl.IsDelete==false
                                    select
                                new CustomerModel()
                                {
                                    CustomerId = tbl.CustomerId,
                                    CustomerFirstName = tbl.CustomerFirstName,
                                    CustomerMName = tbl.CustomerMName,
                                    CustomeLName = tbl.CustomeLName,
                                    Address = tbl.Address,
                                    City = tbl.City,
                                    Pin = tbl.Pin,
                                    State = tbl.State,
                                    Country = tbl.Country,
                                    MobileNo = tbl.MobileNo,
                                    EMailId = tbl.EMailId,
                                    CustomerTypeId = tbl.CustomerTypeId,
                                    DuplicateBill = tbl.DuplicateBill,
                                    SummaryofBills = tbl.SummaryofBills,
                                    ContactPerson = tbl.ContactPerson,
                                    VehicleWiseBill = tbl.VehicleWiseBill,
                                    BillingCycle = tbl.BillingCycle,
                                    CreditLimit = tbl.CreditLimit,
                                    ChargesPercent = tbl.ChargesPercent,
                                    SecurityDeposit = tbl.SecurityDeposit,
                                    NoofCreditDays = tbl.NoofCreditDays,
                                    IsSeperateBill = tbl.IsSeperateBill,
                                    IsRoundOff = tbl.IsRoundOff,
                                    IsDelete = tbl.IsDelete,
                                    LedgerId = tbl.LedgerId,
                                    
                                    CustomerType = tblctype.CustomerType,
                                    AccountTypeId=tblledger.AcTypeId,
                                    SubledgerId=tblledger.SubLedgerId,
                                    //BalType = tblopening.CreditBal > 0 ? "CR" : (tblopening.DebitBal > 0 ? "DR" : ""),
                                    //OpeningBalance = tblopening.CreditBal > 0 ? tblopening.CreditBal : (tblopening.DebitBal > 0 ? tblopening.DebitBal : 0)

                                };
            if (response.CustomerList.Count() > 0)
            {
                response.Status = true;
            }
            return response;
        }

        public CustomerResponse SaveCustomer(CustomerModel model)
        {
            CustomerResponse response = new CustomerResponse() { Status = false };
            //tblSubLedger tbl = _db.tbl_CustomerInfo.Where(p => p..Trim().ToUpper() == model.SubLedgerName.Trim().ToUpper()).FirstOrDefault();

            tblLedger ledger = new tblLedger()
            {
                AccOpenDate = DateTime.Now.Date,
                AcTypeId = model.AccountTypeId,
                SubLedgerId=model.SubledgerId,
                FinancialYearId = 1
            };
            _db.tblLedgers.Add(ledger);
            if (_db.SaveChanges() > 0)
            {
                tbl_CustomerInfo tbl = new tbl_CustomerInfo()
                {
                    CustomerId = model.CustomerId,
                    CustomerFirstName = model.CustomerFirstName,
                    CustomerMName = model.CustomerMName,
                    CustomeLName = model.CustomeLName,
                    Address = model.Address,
                    City = model.City,
                    Pin = model.Pin,
                    State = model.State,
                    Country = model.Country,
                    MobileNo = model.MobileNo,
                    EMailId = model.EMailId,
                    CustomerTypeId = model.CustomerTypeId,
                    DuplicateBill = model.DuplicateBill,
                    SummaryofBills = model.SummaryofBills,
                    ContactPerson = model.ContactPerson,
                    VehicleWiseBill = model.VehicleWiseBill,
                    BillingCycle = model.BillingCycle,
                    CreditLimit = model.CreditLimit,
                    ChargesPercent = model.ChargesPercent,
                    SecurityDeposit = model.SecurityDeposit,
                    NoofCreditDays = model.NoofCreditDays,
                    IsSeperateBill = model.IsSeperateBill,
                    IsRoundOff = model.IsRoundOff,
                    IsDelete = false,
                    LedgerId = ledger.LedgerId,

                };
                _db.tbl_CustomerInfo.Add(tbl);
                tblOpeningBalance opening = new tblOpeningBalance()
                {
                    CreatedDate = DateTime.Now.Date,
                    CreditBal = 0,// ? model.OpeningBalance : 0,
                    DebitBal = 0,//model.BalType == "DR" ? model.OpeningBalance : 0,
                    FinancialYearId = 1,
                    IsDelete = false,
                    LedgerId = ledger.LedgerId,
                    OpeningBalnceEffectFrom = DateTime.Now.Date,
                };
                _db.tblOpeningBalances.Add(opening);
                if (_db.SaveChanges() > 0)
                {
                    response.Status = true;
                    response.Id = tbl.CustomerId;
                }
                else
                {
                    response.Message = "Record not saved.";
                }
            }
            else
            {
                response.Message = "Record not saved.";
            }
            return response;
        }

        public CustomerResponse UpdateCustomer(CustomerModel model)
        {
            CustomerResponse response = new CustomerResponse() { Status = false };
            tbl_CustomerInfo tbl = _db.tbl_CustomerInfo.Where(p => p.CustomerId == model.CustomerId).FirstOrDefault();
            if (tbl != null)
            {
                //tbl.CustomerId = model.CustomerId;
                tbl.CustomerFirstName = model.CustomerFirstName;
                tbl.CustomerMName = model.CustomerMName;
                tbl.CustomeLName = model.CustomeLName;
                tbl.Address = model.Address;
                tbl.City = model.City;
                tbl.Pin = model.Pin;
                tbl.State = model.State;
                tbl.Country = model.Country;
                tbl.MobileNo = model.MobileNo;
                tbl.EMailId = model.EMailId;
                tbl.CustomerTypeId = model.CustomerTypeId;
                tbl.DuplicateBill = model.DuplicateBill;
                tbl.SummaryofBills = model.SummaryofBills;
                tbl.ContactPerson = model.ContactPerson;
                tbl.VehicleWiseBill = model.VehicleWiseBill;
                tbl.BillingCycle = model.BillingCycle;
                tbl.CreditLimit = model.CreditLimit;
                tbl.ChargesPercent = model.ChargesPercent;
                tbl.SecurityDeposit = model.SecurityDeposit;
                tbl.NoofCreditDays = model.NoofCreditDays;
                tbl.IsSeperateBill = model.IsSeperateBill;
                tbl.IsRoundOff = model.IsRoundOff;
                //tbl.IsDelete = model.IsDelete;
                //tbl.LedgerId = model.LedgerId;
                if (_db.SaveChanges() > 0)
                {
                    response.Status = true;
                }
                else
                {
                    response.Message = "Record not updated.";
                }
            }
            else
            {
                response.Message = "Record not found.";
            }
            return response;
        }
    }
}