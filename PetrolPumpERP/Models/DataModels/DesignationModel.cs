using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    public class DesignationModel
    {
        public int DesignationCode { get; set; }
        public string DesignationDesc { get; set; }
        public bool IsDelete { get; set; }
    }

    

    public class DesignationResponse : Error
    {
        public IQueryable<DesignationModel> DesignationList { get; set; }
    }

    public class DesignationBL
    {
        public static DesignationBL _userBl = null;
        PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private DesignationBL()
        {

        }

        public static DesignationBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new DesignationBL();
                }
                return _userBl;
            }
        }



        public DesignationResponse GetAllDesignations()
        {
            DesignationResponse response = new DesignationResponse() { Status = false };
            response.DesignationList = from tbl in _db.tblDesignationMasters
                                      select
                                new DesignationModel()
                                {
                                    DesignationCode = tbl.DesignationCode,
                                    DesignationDesc = tbl.DesignationDesc,
                                    IsDelete=tbl.IsDelete,
                                };
            if (response.DesignationList.Count() > 0)
            {
                response.Status = true;
            }
            return response;
        }

        public DesignationResponse Save(DesignationModel model)
        {
            DesignationResponse response = new DesignationResponse() { Status = false };
            tblDesignationMaster tbl = _db.tblDesignationMasters.Where(p => p.DesignationDesc.Trim().ToUpper() == model.DesignationDesc.Trim().ToUpper()).FirstOrDefault();
            if (tbl == null)
            {
                tbl = new tblDesignationMaster()
                {
                    DesignationDesc = model.DesignationDesc,
                };
                _db.tblDesignationMasters.Add(tbl);
                if (_db.SaveChanges() > 0)
                {
                    response.Status = true;
                    response.Id = tbl.DesignationCode;
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

        public DesignationResponse Update(DesignationModel model)
        {
            DesignationResponse response = new DesignationResponse() { Status = false };
            tblDesignationMaster tbl = _db.tblDesignationMasters.Where(p => p.DesignationDesc.Trim().ToUpper() == model.DesignationDesc.Trim().ToUpper() && p.DesignationCode == model.DesignationCode).FirstOrDefault();
            if (tbl != null)
            {
                tbl.DesignationDesc = model.DesignationDesc;
                tbl.DesignationCode = model.DesignationCode;
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
                tbl = _db.tblDesignationMasters.Where(p => p.DesignationDesc.Trim().ToUpper() == model.DesignationDesc.Trim().ToUpper()).FirstOrDefault();
                if (tbl == null)
                {
                    tbl = _db.tblDesignationMasters.Where(p => p.DesignationCode == model.DesignationCode).FirstOrDefault();
                    if (tbl != null)
                    {
                        tbl.DesignationDesc = model.DesignationDesc;
                        tbl.DesignationCode = model.DesignationCode;
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