using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    

    public class AccountTypeModel
    {
        public long? AccountTypeId { get; set; }
        public string AccountType { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<long> SubledgerId { get; set; }

        public int SRNo { get; set; }

        public string SubLedgerName { get; set; }
    }


    public class AccountTypeResponse : Error
    {
        public AccountTypeResponse()
        {
            Subledgers = new SubledgerResponse();
        }

        public SubledgerResponse Subledgers { get; set; }

        public IQueryable<AccountTypeModel> AccountTypeList { get; set; }
    }

    public class AccountTypeBL
    {
        public static AccountTypeBL _userBl = null;
        PetrolPumpERP.Models.DataEntities.PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private AccountTypeBL()
        {

        }

        public static AccountTypeBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new AccountTypeBL();
                }
                return _userBl;
            }
        }

        
        public AccountTypeResponse GetAccountTypes()
        {
            AccountTypeResponse response = new AccountTypeResponse() { Status = false };
            SubledgerBL objSubledger = SubledgerBL.Instance;
            response.AccountTypeList = from tbl in _db.tblAccountTypes
                                       where tbl.IsDelete == false
                                       select new AccountTypeModel
                                       {
                                           AccountType=tbl.AccountType,
                                           AccountTypeId=tbl.AccountTypeId,
                                           IsDelete=tbl.IsDelete,
                                       };

            response.Subledgers= objSubledger.GetAllSubledgers();
            if (response.AccountTypeList.Count() > 0)
            {
                response.Status = true;
            }
            return response;
        }

        public IQueryable<AccountTypeModel> GetAccountTypesDetails(int? AcId)
        {
            var data= (from tbl in _db.tblAccountTypeDetails
                    join tblsubledger in _db.tblSubLedgers
                    on tbl.SubledgerId equals tblsubledger.SubLedgerId
                    where tbl.IsDelete == false
                    //&& tbl.AccountTypeId==AcId
                    select new AccountTypeModel
                    {
                        AccountTypeId = tbl.AccountTypeId,
                        IsDelete = tbl.IsDelete,
                        SRNo = tbl.SRNo,
                        SubledgerId = tblsubledger.SubLedgerId,
                        SubLedgerName = tblsubledger.SubLedgerName,
                    });
            if (AcId!=null)
            {
                return data.Where(p => p.AccountTypeId == AcId);
            }
            return data;
        }

        public AccountTypeResponse SaveAccountType(List<AccountTypeModel> model)
        {
            AccountTypeResponse response = new AccountTypeResponse() { Status = false };
            string actype = model[0].AccountType.ToLower();
            tblAccountType ac = _db.tblAccountTypes.Where(p => p.AccountType.ToLower() == actype).FirstOrDefault();
            if (ac==null)
            {
                int cnt = 0;
                foreach (AccountTypeModel item in model)
                {
                    tblAccountTypeDetail detail = _db.tblAccountTypeDetails.Where(p => p.SubledgerId == item.SubledgerId && p.IsDelete==false).FirstOrDefault();
                    if (detail!=null)
                    {
                        cnt++;
                    }
                }
                if (cnt==model.Count)
                {
                    response.Message = "Another account type created with same subledgers.";
                }
                else
                {
                    ac = new tblAccountType()
                    {
                        IsDelete = false,
                        AccountType = model[0].AccountType,
                        SubledgerId = 0,// model[0].SubledgerId
                    };
                    _db.tblAccountTypes.Add(ac);
                    if (_db.SaveChanges() > 0)
                    {
                        foreach (var item in model)
                        {
                            tblAccountTypeDetail tbldetails = new tblAccountTypeDetail()
                            {
                                IsDelete = false,
                                AccountTypeId = ac.AccountTypeId,
                                SubledgerId = item.SubledgerId
                            };
                            _db.tblAccountTypeDetails.Add(tbldetails);
                        }

                        _db.SaveChanges();
                        response.Status = true;
                        response.Id = ac.AccountTypeId;
                    }
                    else
                    {
                        response.Message = "Record not saved.";
                    }
                }
            }
            else
            {
                response.Message = "Such account type already created.";
            }
            return response;
        }

        public AccountTypeResponse UpdateAccountType(List<AccountTypeModel> model)
        {
            AccountTypeResponse response = new AccountTypeResponse() { Status = false };
            string actype = model[0].AccountType.ToLower();
            long? actypeid= model[0].AccountTypeId;
            tblAccountType ac = _db.tblAccountTypes.Where(p => p.AccountType.ToLower() == actype  && p.AccountTypeId==actypeid).FirstOrDefault();
            if (ac != null)
            {
                ac = _db.tblAccountTypes.Where(p => p.AccountTypeId == actypeid).FirstOrDefault();
                if (ac!=null)
                {
                    ac.AccountType = model[0].AccountType;
                    // Firstly delete account types which are not in currunt request
                    var subtypes = _db.tblAccountTypeDetails.Where(p => p.AccountTypeId == actypeid && p.IsDelete==false);
                    foreach (var item in subtypes)
                    {
                        AccountTypeModel detail = model.Where(p => p.SubledgerId == item.SubledgerId).FirstOrDefault();
                        if (detail == null)
                        {
                            item.IsDelete = true;
                        }
                    }
                    // Firstly delete account types which are not in currunt request
                    foreach (var item in model)
                    {
                        tblAccountTypeDetail detail = _db.tblAccountTypeDetails.Where(p => p.SubledgerId == item.SubledgerId && p.AccountTypeId == item.AccountTypeId && p.IsDelete==false).FirstOrDefault();
                        if (detail == null)
                        {
                            detail = new tblAccountTypeDetail() { AccountTypeId = item.AccountTypeId, IsDelete = false, SubledgerId = item.SubledgerId };
                            _db.tblAccountTypeDetails.Add(detail);
                        }
                    }
                    _db.SaveChanges();
                    response.Message = "Record updated successfully.";
                    response.Status = true;
                }
                else
                {
                    response.Message = "Record not found.";
                }
            }
            else
            {
                ac = _db.tblAccountTypes.Where(p => p.AccountType.ToLower() == actype).FirstOrDefault();
                if (ac!=null)
                {
                    ac = _db.tblAccountTypes.Where(p => p.AccountTypeId == actypeid).FirstOrDefault();
                    if (ac != null)
                    {
                        ac.AccountType = model[0].AccountType;
                        // Firstly delete account types which are not in currunt request
                        var subtypes = _db.tblAccountTypeDetails.Where(p => p.AccountTypeId == actypeid && p.IsDelete==false);
                        foreach (var item in subtypes)
                        {
                            AccountTypeModel detail = model.Where(p => p.SubledgerId == item.SubledgerId).FirstOrDefault();
                            if (detail == null)
                            {
                                item.IsDelete = true;
                            }
                        }
                        foreach (var item in model)
                        {
                            tblAccountTypeDetail detail = _db.tblAccountTypeDetails.Where(p => p.SubledgerId == item.SubledgerId && p.AccountTypeId == item.AccountTypeId && p.IsDelete==false).FirstOrDefault();
                            if (detail == null)
                            {
                                detail = new tblAccountTypeDetail() { AccountTypeId = item.AccountTypeId, IsDelete = false, SubledgerId = item.SubledgerId };
                                _db.tblAccountTypeDetails.Add(detail);
                            }
                        }
                        _db.SaveChanges();
                        response.Message = "Record updated successfully.";
                        response.Status = true;
                    }
                    else
                    {
                        response.Message = "Record not found.";
                    }
                }
                else
                {
                    response.Message = "Such account type already exist.";
                }
            }
            return response;
        }
    }
}