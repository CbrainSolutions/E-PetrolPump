using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetrolPumpERP.Models.DataEntities;
using System.Data.Entity.Validation;

namespace PetrolPumpERP.Models.DataModels
{
    public class ProductModel
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<long> ProductTypeId { get; set; }
        public Nullable<int> UOMId { get; set; }
        public Nullable<int> SubUOMId { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string ProductDescription { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public IQueryable<StockModel> StockDetail { get; set; }

        public string ProductType { get; set; }

        public string UOM { get; set; }

        public string SubUOM { get; set; }
    }

    public class StockModel
    {
        public int StockId { get; set; }
        public long ProductId { get; set; }
        public Nullable<int> OpeningQty { get; set; }
        public Nullable<System.DateTime> OpeningQtyDate { get; set; }
        public Nullable<int> InwardQty { get; set; }
        public Nullable<decimal> InwardPrice { get; set; }
        public Nullable<int> OutwardQty { get; set; }
        public Nullable<decimal> OutwardPrice { get; set; }
        public Nullable<int> DocumentNo { get; set; }
        public string TransactionType { get; set; }
        public bool IsDelete { get; set; }
        public string BatchNo { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<decimal> InwardAmount { get; set; }
        public Nullable<decimal> OutwardAmount { get; set; }
    }


    public class ProductResponse : Error
    {
        public IQueryable<ProductModel> ProductList { get; set; }
    }

    public class ProductModelBL
    {
        public static ProductModelBL _userBl = null;
        PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private ProductModelBL()
        {

        }

        public static ProductModelBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new ProductModelBL();
                }
                return _userBl;
            }
        }

        public ProductResponse GetProducts()
        {
            ProductResponse response = new ProductResponse();
            response.ProductList = from tbl in _db.tblProductMasters
                                   join tblproducttype in _db.tblProductTypes
                                   on tbl.ProductTypeId equals tblproducttype.ProductTypeId
                                   join tbluom in _db.tblUnitMasters
                                   on tbl.UOMId equals tbluom.UnitCode
                                   join tblsubuom in _db.tblUnitMasters
                                   on tbl.SubUOMId equals tblsubuom.UnitCode
                                   where tbl.IsDelete == false
                                   select new ProductModel
                                   {
                                       ProductId=tbl.ProductId,
                                       CreatedDate=tbl.CreatedDate,
                                       IsDelete=tbl.IsDelete,
                                       Price=tbl.Price,
                                       ProductDescription=tbl.ProductDescription,
                                       ProductName=tbl.ProductName,
                                       ProductType=tblproducttype.ProductType,
                                       ProductTypeId=tbl.ProductTypeId,
                                       SubUOM=tbluom.UnitDesc,
                                       SubUOMId=tbl.SubUOMId,
                                       UOM=tbluom.UnitDesc,
                                       UOMId=tbl.UOMId,
                                   };
            return response;
        }



        public ProductResponse Save(ProductModel model)
        {
            ProductResponse response = new ProductResponse() { Status = false };
            //tblSubLedger tbl = _db.tbl_CustomerInfo.Where(p => p..Trim().ToUpper() == model.SubLedgerName.Trim().ToUpper()).FirstOrDefault();
            tblProductMaster tbl = _db.tblProductMasters.Where(p => p.ProductName.Trim().ToLower() == model.ProductName.Trim().ToLower()).FirstOrDefault();
            if (tbl == null)
            {
                try
                {
                    tbl = new tblProductMaster()
                    {
                        CreatedDate=DateTime.Now.Date,
                        IsDelete=false,
                        Price=model.Price,
                        ProductDescription=string.IsNullOrEmpty(model.ProductDescription)?"":model.ProductDescription,
                        ProductName=model.ProductName,
                        ProductTypeId=model.ProductTypeId,
                        UOMId=model.UOMId,
                        SubUOMId=model.SubUOMId
                    };
                    _db.tblProductMasters.Add(tbl);

                    var state = _db.SaveChanges();
                    if (state > 0)
                    {
                        response.Status = true;
                        response.Id = tbl.ProductId;
                        tblStockDetail stock = new tblStockDetail()
                        {
                             InwardPrice=model.Price,
                             OpeningQty=0,
                             IsDelete=false,  
                        };
                        _db.tblStockDetails.Add(stock);
                        _db.SaveChanges();
                    }
                    else
                    {
                        response.Message = "Record not saved.";
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            //Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
            }
            else
            {
                response.Message = "Another product created with same name.";
            }
            return response;
        }

        public ProductResponse Update(ProductModel model)
        {
            ProductResponse response = new ProductResponse() { Status = false };

            tblProductMaster tbl = _db.tblProductMasters.Where(p => p.ProductId == model.ProductId && p.ProductName.Trim().ToLower() == model.ProductName.Trim().ToLower()).FirstOrDefault();
            if (tbl != null)
            {
                try
                {
                    tbl.CreatedDate = model.CreatedDate;
                    tbl.Price = model.Price;
                    tbl.ProductDescription = model.ProductDescription;
                    tbl.ProductName = model.ProductName;
                    tbl.ProductTypeId = model.ProductTypeId;
                    tbl.UOMId = model.UOMId;
                    tbl.SubUOMId = model.SubUOMId;
                    if (_db.SaveChanges() > 0)
                    {
                        response.Status = true;
                    }
                    else
                    {
                        response.Message = "Record not updated.";
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            //Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
            }
            else
            {
                tbl = _db.tblProductMasters.Where(p => p.ProductName.Trim().ToLower() == model.ProductName.Trim().ToLower()).FirstOrDefault();
                if (tbl != null)
                {
                    response.Message = "Another vendor registered with same name.";
                }
                else
                {
                    tbl = _db.tblProductMasters.Where(p => p.ProductId == model.ProductId).FirstOrDefault();
                    if (tbl != null)
                    {
                        tbl.CreatedDate = model.CreatedDate;
                        tbl.Price = model.Price;
                        tbl.ProductDescription = model.ProductDescription;
                        tbl.ProductName = model.ProductName;
                        tbl.ProductTypeId = model.ProductTypeId;
                        tbl.UOMId = model.UOMId;
                        tbl.SubUOMId = model.SubUOMId;
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
            }
            return response;
        }
    }
}