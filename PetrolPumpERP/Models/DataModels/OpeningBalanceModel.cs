using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    public class OpeningBalanceModel
    {
        public long OpeningBalanceId { get; set; }
        public Nullable<long> LedgerId { get; set; }
        public Nullable<System.DateTime> OpeningBalnceEffectFrom { get; set; }
        public Nullable<decimal> CreditBal { get; set; }
        public Nullable<decimal> DebitBal { get; set; }
        public Nullable<long> FinancialYearId { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public string Name { get; set; }

        public string BalType { get; set; }

        public Nullable<decimal> OpeningBal { get; set; }


        public DateTime? AccOpenDate { get; set; }

    }

    public class OpeningBalanceResponse : Error
    {
        public IQueryable<OpeningBalanceModel> OpeningBalanceList { get; set; }
    }

    public class OpeningBalanceBL
    {
        public static OpeningBalanceBL _userBl = null;
        PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private OpeningBalanceBL()
        {
            
        }

        public static OpeningBalanceBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new OpeningBalanceBL();
                }
                return _userBl;
            }
        }

        public OpeningBalanceResponse GetAllOpeningBalLedgers()
        {
            var response= new OpeningBalanceResponse() {Status=true };
            var arrcust = from temp in _db.tbl_CustomerInfo
                          select temp.LedgerId;
            var arrsup = from temp in _db.tblSupplierMasters
                         select temp.LedgerId;
            var arrbank = from temp in _db.tblBankMasters
                         select temp.LedgerId;

            var arrother = from temp in _db.tblOtherAccounts
                          select temp.LedgerId;

            long?[] longarr = { 1,2 };

            var arrsubledger = from temp in _db.tblSubLedgers
                               where longarr.Contains(temp.MainLedgerId)
                               select temp.SubLedgerId;

            var result = (from tbl in _db.tblOpeningBalances
                          join tblledger in _db.tblLedgers
                          on tbl.LedgerId equals tblledger.LedgerId
                          join tblc in _db.tbl_CustomerInfo
                          on tbl.LedgerId equals tblc.LedgerId
                          select new OpeningBalanceModel
                          {
                              AccOpenDate=tblledger.AccOpenDate,
                              CreatedDate=tbl.CreatedDate,
                              CreditBal=tbl.CreditBal,
                              DebitBal=tbl.DebitBal,
                              FinancialYearId=tbl.FinancialYearId,
                              IsDelete=tbl.IsDelete,
                              Name=tblc.CustomerFirstName + " " + tblc.CustomeLName,
                              LedgerId=tbl.LedgerId,
                              OpeningBalanceId=tbl.OpeningBalanceId,
                              OpeningBalnceEffectFrom=tbl.OpeningBalnceEffectFrom,
                              BalType=tbl.CreditBal==0 && tbl.DebitBal==0?"-":(tbl.CreditBal>0?"CR":"DR"),
                              OpeningBal = tbl.CreditBal == 0 && tbl.DebitBal == 0 ? 0 : (tbl.CreditBal > 0 ? tbl.CreditBal : tbl.DebitBal),
                          }).Union(from tbl in _db.tblOpeningBalances
                                   join tblledger in _db.tblLedgers
                                   on tbl.LedgerId equals tblledger.LedgerId
                                   join tblc in _db.tblBankMasters
                                   on tbl.LedgerId equals tblc.LedgerId
                                   select new OpeningBalanceModel
                                   {
                                       AccOpenDate = tblledger.AccOpenDate,
                                       CreatedDate = tbl.CreatedDate,
                                       CreditBal = tbl.CreditBal,
                                       DebitBal = tbl.DebitBal,
                                       FinancialYearId = tbl.FinancialYearId,
                                       IsDelete = tbl.IsDelete,
                                       Name = tblc.BankName + tblc.AccountNo,
                                       LedgerId = tbl.LedgerId,
                                       OpeningBalanceId = tbl.OpeningBalanceId,
                                       OpeningBalnceEffectFrom = tbl.OpeningBalnceEffectFrom,
                                       BalType = tbl.CreditBal == 0 && tbl.DebitBal == 0 ? "-" : (tbl.CreditBal > 0 ? "CR" : "DR"),
                                       OpeningBal = tbl.CreditBal == 0 && tbl.DebitBal == 0 ? 0 : (tbl.CreditBal > 0 ? tbl.CreditBal : tbl.DebitBal),
                                   }).Union(from tbl in _db.tblOpeningBalances
                                            join tblledger in _db.tblLedgers
                                            on tbl.LedgerId equals tblledger.LedgerId
                                            join tblc in _db.tblOtherAccounts
                                            on tbl.LedgerId equals tblc.LedgerId
                                            select new OpeningBalanceModel
                                            {
                                                AccOpenDate = tblledger.AccOpenDate,
                                                CreatedDate = tbl.CreatedDate,
                                                CreditBal = tbl.CreditBal,
                                                DebitBal = tbl.DebitBal,
                                                FinancialYearId = tbl.FinancialYearId,
                                                IsDelete = tbl.IsDelete,
                                                Name = tblc.AccountName,
                                                LedgerId = tbl.LedgerId,
                                                OpeningBalanceId = tbl.OpeningBalanceId,
                                                OpeningBalnceEffectFrom = tbl.OpeningBalnceEffectFrom,
                                                BalType = tbl.CreditBal == 0 && tbl.DebitBal == 0 ? "-" : (tbl.CreditBal > 0 ? "CR" : "DR"),
                                                OpeningBal = tbl.CreditBal == 0 && tbl.DebitBal == 0 ? 0 : (tbl.CreditBal > 0 ? tbl.CreditBal : tbl.DebitBal),
                                            }).Union(from tbl in _db.tblOpeningBalances
                                   join tblledger in _db.tblLedgers
                                   on tbl.LedgerId equals tblledger.LedgerId
                                   join tblc in _db.tblSupplierMasters
                                   on tbl.LedgerId equals tblc.LedgerId
                                   select new OpeningBalanceModel
                                   {
                                       AccOpenDate = tblledger.AccOpenDate,
                                       CreatedDate = tbl.CreatedDate,
                                       CreditBal = tbl.CreditBal,
                                       DebitBal = tbl.DebitBal,
                                       FinancialYearId = tbl.FinancialYearId,
                                       IsDelete = tbl.IsDelete,
                                       Name = tblc.SupplierName,
                                       LedgerId = tbl.LedgerId,
                                       OpeningBalanceId = tbl.OpeningBalanceId,
                                       OpeningBalnceEffectFrom = tbl.OpeningBalnceEffectFrom,
                                       BalType = tbl.CreditBal == 0 && tbl.DebitBal == 0 ? "-" : (tbl.CreditBal > 0 ? "CR" : "DR"),
                                       OpeningBal = tbl.CreditBal == 0 && tbl.DebitBal == 0 ? 0 : (tbl.CreditBal > 0 ? tbl.CreditBal : tbl.DebitBal),
                                   }).Union(from tbl in _db.tblOpeningBalances
                                            join tblledger in _db.tblLedgers
                                            on tbl.LedgerId equals tblledger.LedgerId
                                            where !(arrcust).Contains(tbl.LedgerId)
                                                    && !(arrsup).Contains(tbl.LedgerId)
                                                    && !(arrbank).Contains(tbl.LedgerId)
                                                    && !(arrother).Contains(tbl.LedgerId)
                                                    //&& (arrsubledger).Contains(tblledger.SubLedgerId.Value)
                                            select new OpeningBalanceModel
                                            {
                                                AccOpenDate = tblledger.AccOpenDate,
                                                CreatedDate = tbl.CreatedDate,
                                                CreditBal = tbl.CreditBal,
                                                DebitBal = tbl.DebitBal,
                                                FinancialYearId = tbl.FinancialYearId,
                                                IsDelete = tbl.IsDelete,
                                                Name = tblledger.LedgerName,
                                                LedgerId = tbl.LedgerId,
                                                OpeningBalanceId = tbl.OpeningBalanceId,
                                                OpeningBalnceEffectFrom = tbl.OpeningBalnceEffectFrom,
                                                BalType = tbl.CreditBal == 0 && tbl.DebitBal == 0 ? "-" : (tbl.CreditBal > 0 ? "CR" : "DR"),
                                                OpeningBal = tbl.CreditBal == 0 && tbl.DebitBal == 0 ? 0 : (tbl.CreditBal > 0 ? tbl.CreditBal : tbl.DebitBal),
                                            });
            response.OpeningBalanceList = result;
            return response;
        }

        public OpeningBalanceResponse Update(OpeningBalanceModel model)
        {
            OpeningBalanceResponse response = new OpeningBalanceResponse() { Status=false};
            tblOpeningBalance bal = _db.tblOpeningBalances.Where(p => p.OpeningBalanceId == model.OpeningBalanceId).FirstOrDefault();
            if (bal!=null)
            {
                if (model.BalType.ToUpper()=="CR")
                {
                    bal.CreditBal = model.OpeningBal;
                }
                else
                {
                    bal.DebitBal = model.OpeningBal;
                }
                _db.SaveChanges();
                response.Status = true;
            }
            return response;
        }
    }
}