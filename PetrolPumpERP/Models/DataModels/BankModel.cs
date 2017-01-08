using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    public class BankModel
    {
        public long BankId { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string IFSC { get; set; }
        public string MICR { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<long> LedgerId { get; set; }
        public string AccountNo { get; set; }
        public Nullable<bool> IsLoan { get; set; }

        public string AccountType { get; set; }
        public long? SubledgerId { get; set; }
        public int? AccountTypeId { get; set; }
    }

    public class BankModelResponse : Error
    {
        public IQueryable<BankModel> BankList { get; set; }
    }

    public class BankModelBL
    {
        public static BankModelBL _userBl = null;
        PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private BankModelBL()
        {

        }

        public static BankModelBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new BankModelBL();
                }
                return _userBl;
            }
        }

        public BankModelResponse GetBankDetails()
        {
            BankModelResponse response = new BankModelResponse();
            response.BankList = from tbl in _db.tblBankMasters
                                  join tblledger in _db.tblLedgers
                                  on tbl.LedgerId equals tblledger.LedgerId
                                  join tblopening in _db.tblOpeningBalances
                                  on tblledger.LedgerId equals tblopening.LedgerId
                                  where tbl.IsDelete == false
                                  select new BankModel
                                  {
                                      Address = tbl.Address,
                                      PhoneNo = tbl.PhoneNo,
                                      //IsDelete = false,
                                      AccountTypeId = tblledger.AcTypeId,
                                      SubledgerId = tblledger.SubLedgerId,
                                      AccountNo=tbl.AccountNo,
                                      BankName=tbl.BankName,
                                      BankId=tbl.BankId,
                                      BranchName=tbl.BranchName,
                                      ContactPerson=tbl.ContactPerson,
                                      IFSC=tbl.IFSC,
                                      IsLoan=tbl.IsLoan,
                                      LedgerId=tblledger.LedgerId,
                                      MICR=tbl.MICR,
                                      MobileNo=tbl.MobileNo,
                                      //BalType=tblopening.CreditBal>0?"CR": (tblopening.DebitBal > 0?"DR":""),
                                      //OpeningBalance= tblopening.CreditBal > 0 ? tblopening.CreditBal : (tblopening.DebitBal > 0 ?tblopening.DebitBal : 0)
                                  };
            return response;
        }



        public BankModelResponse Save(BankModel model)
        {
            BankModelResponse response = new BankModelResponse() { Status = false };
            //tblSubLedger tbl = _db.tbl_CustomerInfo.Where(p => p..Trim().ToUpper() == model.SubLedgerName.Trim().ToUpper()).FirstOrDefault();
            tblBankMaster tbl = _db.tblBankMasters.Where(p => p.BankName.Trim().ToLower() == model.BankName.Trim().ToLower()).FirstOrDefault();
            if (tbl == null)
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
                        tbl = new tblBankMaster()
                        {
                            Address = model.Address,
                            MobileNo = model.MobileNo,
                            BankName = model.BankName,
                            PhoneNo = model.PhoneNo,
                            IsDelete = false,
                            LedgerId = ledger.LedgerId,
                            AccountNo = model.AccountNo,
                            BankId = model.BankId,
                            BranchName = model.BranchName,
                            ContactPerson = model.ContactPerson,
                            IFSC = model.IFSC,
                            IsLoan = false,
                            MICR = model.MICR,
                        };
                        _db.tblBankMasters.Add(tbl);

                        tblOpeningBalance opening = new tblOpeningBalance()
                        {
                            CreatedDate = DateTime.Now.Date,
                            CreditBal = 0,//model.BalType=="CR"?model.OpeningBalance:0,
                            DebitBal = 0,//model.BalType == "DR" ? model.OpeningBalance : 0,
                            FinancialYearId = 1,
                            IsDelete = false,
                            LedgerId = ledger.LedgerId,
                            OpeningBalnceEffectFrom = DateTime.Now.Date,
                        };
                        _db.tblOpeningBalances.Add(opening);
                        var state = _db.SaveChanges();
                        if (state > 0)
                        {
                            response.Status = true;
                            response.Id = tbl.BankId;
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
                response.Message = "Another bank registered with same name.";
            }
            return response;
        }

        public BankModelResponse Update(BankModel model)
        {
            BankModelResponse response = new BankModelResponse() { Status = false };

            tblBankMaster tbl = _db.tblBankMasters.Where(p => p.BankId == model.BankId && p.AccountNo.Trim().ToLower() == model.AccountNo.Trim().ToLower()).FirstOrDefault();
            if (tbl != null)
            {
                //tbl.CustomerId = model.CustomerId;
                tbl.BankName = model.BankName;
                tbl.Address = model.Address;
                tbl.MobileNo = model.MobileNo;
                tbl.PhoneNo = model.PhoneNo;
                tbl.AccountNo = model.AccountNo;
                tbl.BranchName = model.BranchName;
                tbl.ContactPerson = model.ContactPerson;
                tbl.IFSC = model.IFSC;
                tbl.MICR = model.MICR;
                tbl.IsLoan = model.IsLoan;

                tblLedger ledger = _db.tblLedgers.Where(p => p.LedgerId == tbl.LedgerId).FirstOrDefault();
                if (ledger != null)
                {
                    ledger.AcTypeId = model.AccountTypeId;
                    ledger.SubLedgerId = model.SubledgerId;
                }
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
                tbl = _db.tblBankMasters.Where(p => p.AccountNo.Trim().ToLower() == model.AccountNo.Trim().ToLower()).FirstOrDefault();
                if (tbl != null)
                {
                    response.Message = "Another Bank registered with same name.";
                }
                else
                {
                    tbl = _db.tblBankMasters.Where(p => p.BankId == model.BankId).FirstOrDefault();
                    if (tbl != null)
                    {
                        tbl.BankName = model.BankName;
                        tbl.Address = model.Address;
                        tbl.PhoneNo = model.PhoneNo;
                        tbl.MobileNo = model.MobileNo;
                        tbl.AccountNo = model.AccountNo;
                        tbl.BranchName = model.BranchName;
                        tbl.ContactPerson = model.ContactPerson;
                        tbl.IFSC = model.IFSC;
                        tbl.MICR = model.MICR;
                        tbl.IsLoan = model.IsLoan;
                        tblLedger ledger = _db.tblLedgers.Where(p => p.LedgerId == tbl.LedgerId).FirstOrDefault();
                        if (ledger != null)
                        {
                            ledger.AcTypeId = model.AccountTypeId;
                            ledger.SubLedgerId = model.SubledgerId;
                        }
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