using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    public class OtherAccountModel
    {
        public long OtherAccountId { get; set; }
        public string AccountName { get; set; }
        public Nullable<long> LedgerId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string AccountType { get; set; }
        public long? SubledgerId { get; set; }
        public int? AccountTypeId { get; set; }
    }

    

    public class OtherAccountModelResponse : Error
    {
        public IQueryable<OtherAccountModel> OtherAccountList { get; set; }
    }

    public class OtherAccountModelBL
    {
        public static OtherAccountModelBL _userBl = null;
        PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private OtherAccountModelBL()
        {

        }

        public static OtherAccountModelBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new OtherAccountModelBL();
                }
                return _userBl;
            }
        }

        public OtherAccountModelResponse GetOtherAccountDetails()
        {
            OtherAccountModelResponse response = new OtherAccountModelResponse();
            response.OtherAccountList = from tbl in _db.tblOtherAccounts
                                join tblledger in _db.tblLedgers
                                on tbl.LedgerId equals tblledger.LedgerId
                                join tblopening in _db.tblOpeningBalances
                                on tblledger.LedgerId equals tblopening.LedgerId
                                where tbl.IsDelete == false
                                select new OtherAccountModel
                                {
                                    AccountName=tbl.AccountName,
                                    //CreatedDate=DateTime.Now.Date,
                                    IsDelete=false,
                                    LedgerId=tbl.LedgerId,
                                    OtherAccountId=tbl.OtherAccountId,
                                };
            return response;
        }



        public OtherAccountModelResponse Save(OtherAccountModel model)
        {
            OtherAccountModelResponse response = new OtherAccountModelResponse() { Status = false };
            //tblSubLedger tbl = _db.tbl_CustomerInfo.Where(p => p..Trim().ToUpper() == model.SubLedgerName.Trim().ToUpper()).FirstOrDefault();
            tblOtherAccount tbl = _db.tblOtherAccounts.Where(p => p.AccountName.Trim().ToLower() == model.AccountName.Trim().ToLower()).FirstOrDefault();
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
                        tbl = new tblOtherAccount()
                        {
                            AccountName=model.AccountName,
                            CreatedDate=DateTime. Now.Date,
                            IsDelete=false,
                            LedgerId=ledger.LedgerId,
                        };
                        _db.tblOtherAccounts.Add(tbl);

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
                            response.Id = tbl.OtherAccountId;
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

        public OtherAccountModelResponse Update(OtherAccountModel model)
        {
            OtherAccountModelResponse response = new OtherAccountModelResponse() { Status = false };

            tblOtherAccount tbl = _db.tblOtherAccounts.Where(p => p.OtherAccountId == model.OtherAccountId && p.AccountName.Trim().ToLower() == model.AccountName.Trim().ToLower()).FirstOrDefault();
            if (tbl != null)
            {
                //tbl.CustomerId = model.CustomerId;
                tbl.AccountName = model.AccountName;

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
                tbl = _db.tblOtherAccounts.Where(p => p.AccountName.Trim().ToLower() == model.AccountName.Trim().ToLower()).FirstOrDefault();
                if (tbl != null)
                {
                    response.Message = "Another Bank registered with same name.";
                }
                else
                {
                    tbl = _db.tblOtherAccounts.Where(p => p.OtherAccountId == model.OtherAccountId).FirstOrDefault();
                    if (tbl != null)
                    {
                        tbl.AccountName = model.AccountName;
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