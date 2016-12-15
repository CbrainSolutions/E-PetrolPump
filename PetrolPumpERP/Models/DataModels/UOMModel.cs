using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    public class UOMModel
    {
        public int UnitCode { get; set; }
        public string UnitDesc { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }

    public class UOMResponse : Error
    {
        public IQueryable<UOMModel> UOMList { get; set; }
    }

    public class UOMBL
    {
        public static UOMBL _userBl = null;
        PetrolPumpERP.Models.DataEntities.PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private UOMBL()
        {

        }

        public static UOMBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new UOMBL();
                }
                return _userBl;
            }
        }

        

        public UOMResponse GetUOMList()
        {
            UOMResponse response = new UOMResponse() { Status = false };
            response.UOMList = from tbl in _db.tblUnitMasters
                               where tbl.IsDelete==false
                               select new UOMModel
                               {
                                   IsDelete=tbl.IsDelete,
                                   UnitCode=tbl.UnitCode,
                                   UnitDesc=tbl.UnitDesc,
                               };
            if (response.UOMList.Count() > 0)
            {
                response.Status = true;
            }
            return response;
        }

        public UOMResponse Save(UOMModel model)
        {
            UOMResponse response = new UOMResponse() { Status = false };
            tblUnitMaster tbl = _db.tblUnitMasters.Where(p => p.UnitDesc.Trim().ToUpper() == model.UnitDesc.Trim().ToUpper()).FirstOrDefault();
            if (tbl == null)
            {
                tbl = new tblUnitMaster()
                {
                    IsDelete = false,
                    UnitDesc=model.UnitDesc,
                };
                _db.tblUnitMasters.Add(tbl);
                if (_db.SaveChanges() > 0)
                {
                    response.Status = true;
                    response.Id = tbl.UnitCode;
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

        public UOMResponse Update(UOMModel model)
        {
            UOMResponse response = new UOMResponse() { Status = false };
            tblUnitMaster tbl = _db.tblUnitMasters.Where(p => p.UnitDesc.Trim().ToLower()==model.UnitDesc.ToLower() && p.UnitCode==model.UnitCode).FirstOrDefault();
            if (tbl != null)
            {
                tbl.UnitDesc = model.UnitDesc;
                _db.SaveChanges();
                response.Status = true;
            }
            else
            {
                tbl = _db.tblUnitMasters.Where(p => p.UnitDesc.Trim().ToUpper() == model.UnitDesc.Trim().ToUpper()).FirstOrDefault();
                if (tbl == null)
                {
                    tbl = _db.tblUnitMasters.Where(p => p.UnitCode == model.UnitCode).FirstOrDefault();
                    if (tbl != null)
                    {
                        tbl.UnitDesc = model.UnitDesc;
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