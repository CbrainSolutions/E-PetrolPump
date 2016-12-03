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

        //public string SubLedgerName { get; set; }
    }

    public class AccountTypeDetails
    {

    }

    public class AccountTypeResponse : Error
    {
        public IQueryable<AccountTypeModel> AccountTypeList { get; set; }
    }

    public class AccountTypeBL
    {
        public static AccountTypeBL _userBl = null;
        PetrolPumpERP.Models.DataEntities.PetrolPumpERPEntities _db = new PetrolPumpERPEntities1();
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
            
            response.AccountTypeList = _db.tblAccountTypes.Join(_db.tblSubLedgers,
                                     accounttype => accounttype.SubledgerId,
                                     subledgre =>subledgre.SubLedgerId,
                                     (accountype, subledgre) => new { entityaccounttype= accountype,entitysubledger=subledgre}).Select(p => 
                                     new AccountTypeModel {
                                         AccountType=p.entityaccounttype.AccountType,
                                         AccountTypeId=p.entityaccounttype.AccountTypeId,
                                         IsDelete=p.entityaccounttype.IsDelete,
                                         SubledgerId=p.entityaccounttype.SubledgerId,
                                         SubLedgerName=p.entitysubledger.SubLedgerName,
                                     });


            if (response.AccountTypeList.Count() > 0)
            {
                response.Status = true;
            }
            return response;
        }

        public AccountTypeResponse SaveAccountType(AccountTypeModel model)
        {
            AccountTypeResponse response = new AccountTypeResponse() { Status = false };
            tblAccountType tbl = new tblAccountType()
            {
                IsDelete = false,
                AccountType = model.AccountType,
                SubledgerId = model.SubledgerId
            };
            _db.tblAccountTypes.Add(tbl);
            if (_db.SaveChanges() > 0)
            {
                response.Status = true;
                response.Id = tbl.AccountTypeId;
            }
            else
            {
                response.Message = "Record not saved.";
            }
            return response;
        }

        public AccountTypeResponse UpdateAccountType(AccountTypeModel model)
        {
            AccountTypeResponse response = new AccountTypeResponse() { Status = false };
            tblAccountType tbl = _db.tblAccountTypes.Where(p => p.AccountTypeId == model.AccountTypeId).FirstOrDefault();
            if (tbl!=null)
            {
                tbl.AccountType = model.AccountType;
                tbl.SubledgerId = model.SubledgerId;
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