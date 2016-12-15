using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    public class ProductTypeModel
    {
        public long ProductTypeId { get; set; }
        public string ProductType { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }

    public class ProductTypeResponse : Error
    {
        public IQueryable<ProductTypeModel> ProductTypeList { get; set; }
    }

    public class ProductTypeBL
    {
        public static ProductTypeBL _userBl = null;
        PetrolPumpERP.Models.DataEntities.PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private ProductTypeBL()
        {

        }

        public static ProductTypeBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new ProductTypeBL();
                }
                return _userBl;
            }
        }



        public ProductTypeResponse GetProductTypeList()
        {
            ProductTypeResponse response = new ProductTypeResponse() { Status = false };
            response.ProductTypeList = from tbl in _db.tblProductTypes
                                       where tbl.IsDelete == false
                                       select new ProductTypeModel
                                       {
                                           IsDelete = tbl.IsDelete,
                                           ProductType = tbl.ProductType,
                                           ProductTypeId = tbl.ProductTypeId,
                                       };
            if (response.ProductTypeList.Count() > 0)
            {
                response.Status = true;
            }
            return response;
        }

        public ProductTypeResponse Save(ProductTypeModel model)
        {
            ProductTypeResponse response = new ProductTypeResponse() { Status = false };
            tblProductType tbl = _db.tblProductTypes.Where(p => p.ProductType.Trim().ToUpper() == model.ProductType.Trim().ToUpper()).FirstOrDefault();
            if (tbl == null)
            {
                tbl = new tblProductType()
                {
                    IsDelete = false,
                    ProductType = model.ProductType,
                };
                _db.tblProductTypes.Add(tbl);
                if (_db.SaveChanges() > 0)
                {
                    response.Status = true;
                    response.Id = tbl.ProductTypeId;
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

        public ProductTypeResponse Update(ProductTypeModel model)
        {
            ProductTypeResponse response = new ProductTypeResponse() { Status = false };
            tblProductType tbl = _db.tblProductTypes.Where(p => p.ProductType.Trim().ToLower() == model.ProductType.ToLower() && p.ProductTypeId == model.ProductTypeId).FirstOrDefault();
            if (tbl != null)
            {
                tbl.ProductType = model.ProductType;
                _db.SaveChanges();
                response.Status = true;
            }
            else
            {
                tbl = _db.tblProductTypes.Where(p => p.ProductType.Trim().ToUpper() == model.ProductType.Trim().ToUpper()).FirstOrDefault();
                if (tbl == null)
                {
                    tbl = _db.tblProductTypes.Where(p => p.ProductTypeId == model.ProductTypeId).FirstOrDefault();
                    if (tbl != null)
                    {
                        tbl.ProductType = model.ProductType;
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