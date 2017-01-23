using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    public class SwipeMachineModel
    {
        public long MachineId { get; set; }
        public string MachineNo { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<long> BankId { get; set; }
        public string CardType { get; set; }

        public string BankName { get; set; }
        public string AccountNo { get; set; }
    }

    

    public class SwipeMachineResponse : Error
    {
        public SwipeMachineResponse()
        {
            BankDetails = new BankModelResponse();
        }
        public IQueryable<SwipeMachineModel> SwipeMachineList { get; set; }

        public BankModelResponse BankDetails { get; set; }
    }

    public class SwipeMachineBL
    {
        private static SwipeMachineBL _userBl = null;
        PetrolPumpERP.Models.DataEntities.PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private SwipeMachineBL()
        {

        }

        public static SwipeMachineBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new SwipeMachineBL();
                }
                return _userBl;
            }
        }

        

        public SwipeMachineResponse GetAllSwipeMachines()
        {
            SwipeMachineResponse response = new SwipeMachineResponse() { Status = false };
            response.SwipeMachineList = from p in _db.tblSwipeMachines
                                        join bank in _db.tblBankMasters
                                        on p.BankId equals bank.BankId
                                        where p.IsDelete == false
                                        select
                                        new SwipeMachineModel()
                                        {
                                            IsDelete = p.IsDelete,
                                            BankId = p.BankId,
                                            AccountNo = bank.AccountNo,
                                            BankName = bank.BankName,
                                            CardType = p.CardType,
                                            MachineId = p.MachineId,
                                            MachineNo = p.MachineNo,
                                        };
            BankModelBL objbank = BankModelBL.Instance;
            response.BankDetails = objbank.GetBankDetails();
            if (response.SwipeMachineList.Count() > 0)
            {
                response.Status = true;
            }
            return response;
        }

        public SwipeMachineResponse Save(SwipeMachineModel model)
        {
            SwipeMachineResponse response = new SwipeMachineResponse() { Status = false };
            tblSwipeMachine tbl = _db.tblSwipeMachines.Where(p => p.MachineNo.Trim().ToUpper() == model.MachineNo.Trim().ToUpper()).FirstOrDefault();
            if (tbl == null)
            {
                tbl = new tblSwipeMachine()
                {
                    IsDelete = false,
                    BankId = model.BankId,
                    CardType = model.CardType,
                    MachineId = model.MachineId,
                    MachineNo = model.MachineNo,
                };
                _db.tblSwipeMachines.Add(tbl);
                if (_db.SaveChanges() > 0)
                {
                    response.Status = true;
                    response.Id = tbl.MachineId;
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

        public SwipeMachineResponse Update(SwipeMachineModel model)
        {
            SwipeMachineResponse response = new SwipeMachineResponse() { Status = false };
            tblSwipeMachine tbl = _db.tblSwipeMachines.Where(p => p.MachineNo.Trim().ToUpper() == model.MachineNo.Trim().ToUpper() && p.MachineId == model.MachineId).FirstOrDefault();
            if (tbl != null)
            {
                tbl.MachineNo = model.MachineNo;
                tbl.BankId = model.BankId;
                tbl.CardType = model.CardType;
                _db.SaveChanges();
                response.Status = true;
            }
            else
            {
                tbl = _db.tblSwipeMachines.Where(p => p.MachineNo.Trim().ToUpper() == model.MachineNo.Trim().ToUpper()).FirstOrDefault();
                if (tbl == null)
                {
                    tbl = _db.tblSwipeMachines.Where(p => p.MachineId == model.MachineId).FirstOrDefault();
                    if (tbl != null)
                    {
                        tbl.MachineNo = model.MachineNo;
                        tbl.BankId = model.BankId;
                        tbl.CardType = model.CardType;
                        _db.SaveChanges();
                        response.Status = true;
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