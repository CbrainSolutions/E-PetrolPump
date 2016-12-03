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
        public Nullable<int> LedgerId { get; set; }

        public string CustomerType { get; set; }
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
            response.CustomerList = _db.tbl_CustomerInfo.Join(_db.tblCustomeTypes,
                                 customertype => customertype.CustomerTypeId,
                                 customer => customer.CustomerTypeId,
                                 (customertype, customer) => new { entitycustomer = customertype, entitycustomertype = customer })
                                 .Where(p=>p.entitycustomer.IsDelete==false).Select(p =>
                                new CustomerModel()
                                {
                                   CustomerId=p.entitycustomer.CustomerId,
                                    CustomerFirstName = p.entitycustomer.CustomerFirstName,
                                     CustomerMName = p.entitycustomer.CustomerMName,
                                     CustomeLName = p.entitycustomer.CustomeLName,
                                     Address = p.entitycustomer.Address,
                                     City = p.entitycustomer.City,
                                     Pin = p.entitycustomer.Pin,
                                     State = p.entitycustomer.State,
                                     Country = p.entitycustomer.Country,
                                     MobileNo = p.entitycustomer.MobileNo,
                                     EMailId = p.entitycustomer.EMailId,
                                    CustomerTypeId = p.entitycustomer.CustomerTypeId,
                                     DuplicateBill = p.entitycustomer.DuplicateBill,
                                     SummaryofBills = p.entitycustomer.SummaryofBills,
                                     ContactPerson = p.entitycustomer.ContactPerson,
                                     VehicleWiseBill = p.entitycustomer.VehicleWiseBill,
                                     BillingCycle = p.entitycustomer.BillingCycle,
                                    CreditLimit = p.entitycustomer.CreditLimit,
                                    ChargesPercent = p.entitycustomer.ChargesPercent,
                                    SecurityDeposit = p.entitycustomer.SecurityDeposit,
                                    NoofCreditDays = p.entitycustomer.NoofCreditDays,
                                     IsSeperateBill = p.entitycustomer.IsSeperateBill,
                                     IsRoundOff = p.entitycustomer.IsRoundOff,
                                     IsDelete = p.entitycustomer.IsDelete,
                                    LedgerId = p.entitycustomer.LedgerId,

                                     CustomerType = p.entitycustomertype.CustomerType
                                });
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
                IsDelete = model.IsDelete,
                LedgerId = model.LedgerId,

            };
            _db.tbl_CustomerInfo.Add(tbl);
            if (_db.SaveChanges() > 0)
            {
                response.Status = true;
                response.Id = tbl.CustomerId;
            }
            else
            {
                response.Message = "Record not saved.";
            }
            return response;
        }

        public CustomerResponse UpdateSubledger(CustomerModel model)
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