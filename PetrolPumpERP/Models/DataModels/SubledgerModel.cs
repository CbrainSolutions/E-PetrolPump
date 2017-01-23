using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    public class SubledgerModel
    {
        public long SubLedgerId { get; set; }
        public string SubLedgerName { get; set; }
        public Nullable<System.DateTime> SubLedgerCreationDate { get; set; }
        public Nullable<long> MainLedgerId { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> IsModify { get; set; }

        public string MainLedgerName { get; set; }
    }

    public class MainledgerResponse : Error
    {

        public IQueryable<tblMainLedger> MainledgerList { get; set; }

        
    }

    public class SubledgerResponse : Error
    {
        public IQueryable<SubledgerModel> SubledgerList { get; set; }
    }

    public class SubledgerBL
    {
        public static SubledgerBL _userBl = null;
        PetrolPumpERP.Models.DataEntities.PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private SubledgerBL()
        {

        }

        public static SubledgerBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new SubledgerBL();
                }
                return _userBl;
            }
        }

        public MainledgerResponse GetMainledgers()
        {
            MainledgerResponse response = new MainledgerResponse();
            response.MainledgerList = _db.tblMainLedgers;
            return response;
        }

        public SubledgerResponse GetAllSubledgers()
        {
            SubledgerResponse response = new SubledgerResponse() { Status = false };
            response.SubledgerList = _db.tblSubLedgers.Join(_db.tblMainLedgers,
                                 subledger => subledger.MainLedgerId,
                                 mainledger => mainledger.MainLedgerId,
                                 (subledger, mainledger) => new { entitysubledger = subledger, entitymainledger = mainledger }).Select(p =>
                                new SubledgerModel()
                                {
                                    IsDelete = p.entitysubledger.IsDelete,
                                    SubLedgerId = p.entitysubledger.SubLedgerId,
                                    MainLedgerId  = p.entitysubledger.MainLedgerId,
                                    IsModify = p.entitysubledger.IsModify,
                                    MainLedgerName = p.entitymainledger.MainLedgerName,
                                    SubLedgerName = p.entitysubledger.SubLedgerName,
                                });
            if (response.SubledgerList.Count() > 0)
            {
                response.Status = true;
            }
            return response;
        }

        public SubledgerResponse SaveSubledger(SubledgerModel model)
        {
            SubledgerResponse response = new SubledgerResponse() { Status = false };
            tblSubLedger tbl = _db.tblSubLedgers.Where(p => p.SubLedgerName.Trim().ToUpper() == model.SubLedgerName.Trim().ToUpper()).FirstOrDefault();
            if (tbl==null)
            {
                tbl = new tblSubLedger()
                {
                    IsDelete = false,
                    MainLedgerId = model.MainLedgerId,
                    IsModify = false,
                    SubLedgerName = model.SubLedgerName
                };
                _db.tblSubLedgers.Add(tbl);
                if (_db.SaveChanges() > 0)
                {
                    response.Status = true;
                    response.Id = tbl.SubLedgerId;
                }
                else
                {
                    response.Message = "Record not saved.";
                }
            }
            else
            {
                response.Message = "Same name already exist.";
            }
            return response;
        }

        public SubledgerResponse UpdateSubledger(SubledgerModel model)
        {
            SubledgerResponse response = new SubledgerResponse() { Status = false };
            tblSubLedger tbl = _db.tblSubLedgers.Where(p => p.SubLedgerName.Trim().ToUpper() == model.SubLedgerName.Trim().ToUpper() && p.SubLedgerId==model.SubLedgerId).FirstOrDefault();
            if (tbl!=null)
            {
                tbl.SubLedgerName = model.SubLedgerName;
                tbl.MainLedgerId = model.MainLedgerId;
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
                tbl = _db.tblSubLedgers.Where(p => p.SubLedgerName.Trim().ToUpper() == model.SubLedgerName.Trim().ToUpper()).FirstOrDefault();
                if (tbl==null)
                {
                    tbl = _db.tblSubLedgers.Where(p => p.SubLedgerId == model.SubLedgerId).FirstOrDefault();
                    if (tbl!=null)
                    {
                        tbl.SubLedgerName = model.SubLedgerName;
                        tbl.MainLedgerId = model.MainLedgerId;
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
                else
                {
                    response.Message = "Record already exist.";
                }
            }
            return response;
        }
    }
}