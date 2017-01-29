using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    public class VendorModel
    {
        public int SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string VATCSTNo { get; set; }
        public string ExciseNo { get; set; }
        public string Email { get; set; }
        public string ServiceTaxNo { get; set; }
        public decimal TotalInwardAmnt { get; set; }
        public decimal TotCumInwardAmnt { get; set; }
        public string EntryBy { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public string ChangeBy { get; set; }
        public Nullable<System.DateTime> ChangeDate { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<long> LedgerId { get; set; }
        public int? AccountTypeId { get; set; }

        public string AccountType { get; set; }
        public long? SubledgerId { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pin { get; set; }

        //public decimal? OpeningBalance { get; set; }

        //public string BalType { get; set; }
    }

    

    public class VendorResponse : Error
    {
        public VendorResponse()
        {
            AccontTypeList = new AccountTypeResponse();
        }
        public AccountTypeResponse AccontTypeList { get; set; }

        public IQueryable<VendorModel> VendorList { get; set; }

        public IQueryable<AccountTypeModel> SubledgerList { get; set; }
    }

    public class VendorModelBL
    {
        public static VendorModelBL _userBl = null;
        AccountTypeBL objAcType = AccountTypeBL.Instance;
        PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private VendorModelBL()
        {

        }

        public static VendorModelBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new VendorModelBL();
                }
                return _userBl;
            }
        }

        public VendorResponse GetVendors()
        {
            VendorResponse response = new VendorResponse();
            response.VendorList = from tbl in _db.tblSupplierMasters
                                  join tblledger in _db.tblLedgers
                                  on tbl.LedgerId equals tblledger.LedgerId
                                  join tblopening in _db.tblOpeningBalances
                                  on tblledger.LedgerId equals tblopening.LedgerId
                                  where tbl.IsDelete==false
                                  select new VendorModel
                                  {
                                      Address = tbl.Address,
                                      ChangeDate = tbl.ChangeDate,
                                      Email = tbl.Email,
                                      EntryDate = tbl.EntryDate,
                                      ExciseNo = tbl.ExciseNo,
                                      MobileNo = tbl.MobileNo,
                                      ServiceTaxNo = tbl.ServiceTaxNo,
                                      SupplierName = tbl.SupplierName,
                                      TotalInwardAmnt = 0,
                                      VATCSTNo = tbl.VATCSTNo,
                                      PhoneNo = tbl.PhoneNo,
                                      IsDelete = false,
                                      TotCumInwardAmnt = 0,
                                      City=tbl.City,
                                      State=tbl.State,
                                      Country=tbl.Country,
                                      Pin=tbl.Pin,
                                      SupplierCode=tbl.SupplierCode,
                                      AccountTypeId=tblledger.AcTypeId,
                                      SubledgerId=tblledger.SubLedgerId,
                                  };
            response.AccontTypeList = objAcType.GetAccountTypes();
            response.SubledgerList = objAcType.GetAccountTypesDetails(null);
            return response;
        }

        

        public VendorResponse SaveVendor(VendorModel model)
        {
            VendorResponse response = new VendorResponse() { Status = false };
            tblSupplierMaster tbl=_db.tblSupplierMasters.Where(p=>p.SupplierName.Trim().ToLower()==model.SupplierName.Trim().ToLower()).FirstOrDefault();
            if (tbl==null)
            {
                tblLedger ledger = new tblLedger()
                {
                    AccOpenDate = DateTime.Now.Date,
                    AcTypeId = model.AccountTypeId,
                    SubLedgerId = model.SubledgerId,
                    FinancialYearId = 1,
                    LedgerName = "",
                };
                _db.tblLedgers.Add(ledger);
                try
                {
                    if (_db.SaveChanges() > 0)
                    {
                        tbl = new tblSupplierMaster()
                        {
                            Address = model.Address,
                            ChangeDate = DateTime.Now.Date,
                            Email = model.Email,
                            EntryDate = DateTime.Now.Date,
                            ExciseNo = string.IsNullOrEmpty(model.ExciseNo)?"":model.ExciseNo,
                            MobileNo = model.MobileNo,
                            ServiceTaxNo = "",
                            SupplierName = model.SupplierName,
                            TotalInwardAmnt = 0,
                            VATCSTNo = model.VATCSTNo,
                            PhoneNo = model.PhoneNo,
                            IsDelete = false,
                            TotCumInwardAmnt = 0,
                            LedgerId = ledger.LedgerId,
                            City = model.City,
                            State = model.State,
                            Country = model.Country,
                            Pin = model.Pin,
                            ChangeBy = "",
                            EntryBy = "",
                        };
                        _db.tblSupplierMasters.Add(tbl);

                        tblOpeningBalance opening = new tblOpeningBalance()
                        {
                            CreatedDate=DateTime.Now.Date,
                            CreditBal=0,//model.BalType=="CR"?model.OpeningBalance:0,
                            DebitBal= 0,//model.BalType == "DR" ? model.OpeningBalance : 0,
                            FinancialYearId=1,
                            IsDelete=false,
                            LedgerId=ledger.LedgerId,
                            OpeningBalnceEffectFrom=DateTime.Now.Date,
                        };
                        _db.tblOpeningBalances.Add(opening);
                        var state = _db.SaveChanges();
                        if (state > 0)
                        {
                            response.Status = true;
                            response.Id = tbl.SupplierCode;
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
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            //Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
            }
            else
            {
                response.Message = "Another vendor registered with same name.";
            }
            return response;
        }

        public VendorResponse UpdateVendor(VendorModel model)
        {
            VendorResponse response = new VendorResponse() { Status = false };

            tblSupplierMaster tbl = _db.tblSupplierMasters.Where(p => p.SupplierCode == model.SupplierCode && p.SupplierName.Trim().ToLower()==model.SupplierName.Trim().ToLower()).FirstOrDefault();
            if (tbl != null)
            {
                //tbl.CustomerId = model.CustomerId;
                tbl.SupplierName = model.SupplierName;
                tbl.Address = model.Address;
                tbl.Address = model.Address;
                tbl.Email = model.Email;
                tbl.ExciseNo = model.ExciseNo;
                tbl.MobileNo = model.MobileNo;
                tbl.PhoneNo = model.PhoneNo;
                tbl.MobileNo = model.MobileNo;
                tbl.ServiceTaxNo = model.ServiceTaxNo;
                tbl.VATCSTNo = model.VATCSTNo;
                tbl.City = model.City;
                tbl.State = model.State;
                tbl.Country = model.Country;
                tbl.Pin = model.Pin;
                tbl.ServiceTaxNo = "";
                try
                {
                    if (_db.SaveChanges() > 0)
                    {
                        response.Status = true;
                    }
                    else
                    {
                        response.Message = "Record not updated.";
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            //Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
            }
            else
            {
                tbl = _db.tblSupplierMasters.Where(p => p.SupplierName.Trim().ToLower() == model.SupplierName.Trim().ToLower()).FirstOrDefault();
                if (tbl!=null)
                {
                    response.Message = "Another vendor registered with same name.";
                }
                else
                {
                    tbl = _db.tblSupplierMasters.Where(p => p.SupplierCode==model.SupplierCode).FirstOrDefault();
                    if (tbl!=null)
                    {
                        tbl.SupplierName = model.SupplierName;
                        tbl.Address = model.Address;
                        tbl.Address = model.Address;
                        tbl.Email = model.Email;
                        tbl.ExciseNo = model.ExciseNo;
                        tbl.MobileNo = model.MobileNo;
                        tbl.PhoneNo = model.PhoneNo;
                        tbl.MobileNo = model.MobileNo;
                        tbl.ServiceTaxNo = model.ServiceTaxNo;
                        tbl.VATCSTNo = model.VATCSTNo;
                        tbl.City = model.City;
                        tbl.State = model.State;
                        tbl.Country = model.Country;
                        tbl.Pin = model.Pin;
                        tbl.ServiceTaxNo = "";
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
                }
            }
            return response;
        }
    }
}