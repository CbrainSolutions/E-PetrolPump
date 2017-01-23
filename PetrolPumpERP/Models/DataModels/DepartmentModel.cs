using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    public class DepartmentModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }

    public class DepartmentResponse : Error
    {
        public IQueryable<DepartmentModel> DepartmentList { get; set; }
    }

    public class DepartmentBL
    {
        public static DepartmentBL _userBl = null;
        PetrolPumpERP.Models.DataEntities.PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private DepartmentBL()
        {

        }

        public static DepartmentBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new DepartmentBL();
                }
                return _userBl;
            }
        }

        

        public DepartmentResponse GetAllDepartments()
        {
            DepartmentResponse response = new DepartmentResponse() { Status = false };
            response.DepartmentList = from tbl in _db.tblDepartments
                                      select
                                new DepartmentModel()
                                {
                                    DepartmentId=tbl.DepartmentId,
                                    DepartmentName=tbl.DepartmentName,
                                };
            if (response.DepartmentList.Count() > 0)
            {
                response.Status = true;
            }
            return response;
        }

        public DepartmentResponse Save(DepartmentModel model)
        {
            DepartmentResponse response = new DepartmentResponse() { Status = false };
            tblDepartment tbl = _db.tblDepartments.Where(p => p.DepartmentName.Trim().ToUpper() == model.DepartmentName.Trim().ToUpper()).FirstOrDefault();
            if (tbl == null)
            {
                tbl = new tblDepartment()
                {
                    DepartmentName=model.DepartmentName,
                };
                _db.tblDepartments.Add(tbl);
                if (_db.SaveChanges() > 0)
                {
                    response.Status = true;
                    response.Id = tbl.DepartmentId;
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

        public DepartmentResponse Update(DepartmentModel model)
        {
            DepartmentResponse response = new DepartmentResponse() { Status = false };
            tblDepartment tbl = _db.tblDepartments.Where(p => p.DepartmentName.Trim().ToUpper() == model.DepartmentName.Trim().ToUpper() && p.DepartmentId == model.DepartmentId).FirstOrDefault();
            if (tbl != null)
            {
                tbl.DepartmentName = model.DepartmentName;
                tbl.DepartmentId = model.DepartmentId;
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
                tbl = _db.tblDepartments.Where(p => p.DepartmentName.Trim().ToUpper() == model.DepartmentName.Trim().ToUpper()).FirstOrDefault();
                if (tbl == null)
                {
                    tbl = _db.tblDepartments.Where(p => p.DepartmentId == model.DepartmentId).FirstOrDefault();
                    if (tbl != null)
                    {
                        tbl.DepartmentName = model.DepartmentName;
                        tbl.DepartmentId = model.DepartmentId;
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