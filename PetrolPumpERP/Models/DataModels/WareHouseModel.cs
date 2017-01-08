using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    public class WareHouseResponse:Error
    {
        public IQueryable<tblWareHouse> WareHouseList { get; set; }
    }

    public class WareHouseBL
    {
        public static WareHouseBL _userBl = null;
        PetrolPumpERP.Models.DataEntities.PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private WareHouseBL()
        {

        }

        public static WareHouseBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new WareHouseBL();
                }
                return _userBl;
            }
        }



        public WareHouseResponse GetWareHouseList()
        {
            WareHouseResponse response = new WareHouseResponse() { Status = false };
            response.WareHouseList = from tbl in _db.tblWareHouses
                                       where tbl.IsDelete == false
                                       select tbl;
            if (response.WareHouseList.Count() > 0)
            {
                response.Status = true;
            }
            return response;
        }

        public WareHouseResponse Save(tblWareHouse model)
        {
            WareHouseResponse response = new WareHouseResponse() { Status = false };
            tblWareHouse tbl = _db.tblWareHouses.Where(p => p.WareHouseName.Trim().ToUpper() == model.WareHouseName.Trim().ToUpper()).FirstOrDefault();
            if (tbl == null)
            {
                tbl = new tblWareHouse()
                {
                    IsDelete = false,
                    WareHouseName = model.WareHouseName,
                    Address=model.Address
                };
                _db.tblWareHouses.Add(tbl);
                if (_db.SaveChanges() > 0)
                {
                    response.Status = true;
                    response.Id = tbl.WareHouseNo;
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

        public WareHouseResponse Update(tblWareHouse model)
        {
            WareHouseResponse response = new WareHouseResponse() { Status = false };
            tblWareHouse tbl = _db.tblWareHouses.Where(p => p.WareHouseName.Trim().ToLower() == model.WareHouseName.ToLower() && p.WareHouseNo == model.WareHouseNo).FirstOrDefault();
            if (tbl != null)
            {
                tbl.WareHouseName = model.WareHouseName;
                tbl.Address = model.Address;
                _db.SaveChanges();
                response.Status = true;
            }
            else
            {
                tbl = _db.tblWareHouses.Where(p => p.WareHouseName.Trim().ToUpper() == model.WareHouseName.Trim().ToUpper()).FirstOrDefault();
                if (tbl == null)
                {
                    tbl = _db.tblWareHouses.Where(p => p.WareHouseNo == model.WareHouseNo).FirstOrDefault();
                    if (tbl != null)
                    {
                        tbl.WareHouseName = model.WareHouseName;
                        tbl.Address = model.Address;
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