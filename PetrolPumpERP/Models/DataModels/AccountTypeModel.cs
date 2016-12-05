using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    

    public class AccountTypeModel
    {
        public int AccountTypeId { get; set; }
        public string AccountType { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<long> SubledgerId { get; set; }

        public int SRNo { get; set; }

        public string SubLedgerName { get; set; }
    }


    //public class AccountTypeDetails
    //{
    //    public int SRNo { get; set; }
    //    public Nullable<long> AccountTypeId { get; set; }
    //    public Nullable<bool> IsDelete { get; set; }
    //    public Nullable<long> SubledgerId { get; set; }

    //    public string SubLedgerName { get; set; }
    //}

    public class AccountTypeResponse : Error
    {
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

            response.AccountTypeList = from tbl in _db.tblAccountTypes
                                       where tbl.IsDelete == false
                                       select new AccountTypeModel
                                       {
                                           AccountType=tbl.AccountType,
                                           AccountTypeId=tbl.AccountTypeId,
                                           IsDelete=tbl.IsDelete,
                                       };


            if (response.AccountTypeList.Count() > 0)
            {
                response.Status = true;
            }
            return response;
        }

        public IQueryable<AccountTypeModel> GetAccountTypesDetails(int? AcId)
        {
            return (from tbl in _db.tblAccountTypeDetails
                    join tblsubledger in _db.tblSubLedgers
                    on tbl.SubledgerId equals tblsubledger.SubLedgerId
                    where tbl.IsDelete == false
                    && tbl.AccountTypeId==AcId
                    select new AccountTypeModel
                    {
                        AccountTypeId = Convert.ToInt32(tbl.AccountTypeId),
                        IsDelete = tbl.IsDelete,
                        SRNo = tbl.SRNo,
                        SubledgerId = tblsubledger.SubLedgerId,
                        SubLedgerName = tblsubledger.SubLedgerName,
                    });
        }

        public AccountTypeResponse SaveAccountType(List<AccountTypeModel> model)
        {
            AccountTypeResponse response = new AccountTypeResponse() { Status = false };
            tblAccountType tbl = new tblAccountType()
            {
                IsDelete = false,
                AccountType = model[0].AccountType,
                //SubledgerId = model[0].SubledgerId
            };
            _db.tblAccountTypes.Add(tbl);
            if (_db.SaveChanges() > 0)
            {
                
                foreach (var item in model)
                {
                    tblAccountTypeDetail tbldetails = new tblAccountTypeDetail()
                    {
                        IsDelete = false,
                        AccountTypeId = response.Id,
                        SubledgerId = item.SubledgerId
                    };
                    _db.tblAccountTypeDetails.Add(tbldetails);
                }

                _db.SaveChanges();
                response.Status = true;
                response.Id = tbl.AccountTypeId;
            }
            else
            {
                response.Message = "Record not saved.";
            }
            return response;
        }

        //public AccountTypeResponse UpdateAccountType(AccountTypeModel model)
        //{
        //    AccountTypeResponse response = new AccountTypeResponse() { Status = false };
        //    tblAccountType tbl = _db.tblAccountTypes.Where(p => p.AccountTypeId == model.AccountTypeId).FirstOrDefault();
        //    if (tbl!=null)
        //    {
        //        tbl.AccountType = model.AccountType;
        //        tbl.SubledgerId = model.SubledgerId;
        //        if (_db.SaveChanges() > 0)
        //        {
        //            response.Status = true;
        //        }
        //        else
        //        {
        //            response.Message = "Record not updated.";
        //        }
        //    }
        //    else
        //    {
        //        response.Message = "Record not found.";
        //    }
        //    return response;
        //}
    }
}