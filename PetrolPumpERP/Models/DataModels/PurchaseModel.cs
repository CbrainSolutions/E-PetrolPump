using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetrolPumpERP.Models.DataEntities;
using PetrolPumpERP.Models;

namespace PetrolPumpERP.Models.DataModels
{
    public class PurchaseModel
    {
        public int PINo { get; set; }
        public Nullable<System.DateTime> PIDate { get; set; }
        public Nullable<int> PONo { get; set; }
        public Nullable<long> SupplierLedgerId { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> Tax1 { get; set; }
        public Nullable<int> Tax2 { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<bool> ISCASH { get; set; }
        public string SupplierName { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }

        public string RefDocNo { get; set; }

        public IQueryable<PurchaseDetailModel> PurchaseDetails { get; set; }

    }

    public class PurchaseDetailModel
    {
        public long SRNO { get; set; }
        public Nullable<long> PINo { get; set; }
        public Nullable<long> ItemCode { get; set; }
        public decimal? Quantity { get; set; }
        public string BatchNo { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public decimal? Rate { get; set; }
        public decimal? ProductAmount { get; set; }
        public Nullable<decimal> VAT { get; set; }
        public Nullable<decimal> VATAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }

        public string ProductName { get; set; }

        public int UOMId { get; set; }

        public bool? IsDelete { get; set; }

        public string UOMName { get; set; }
    }

    public class PurchaseResponse : Error
    {
        public IQueryable<PurchaseModel> PurchaseList { get; set; }
    }

    public class PurchaseModelBL
    {
        public static PurchaseModelBL _userBl = null;
        PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private PurchaseModelBL()
        {

        }

        public static PurchaseModelBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new PurchaseModelBL();
                }
                return _userBl;
            }
        }

        public PurchaseResponse GetPurchaseInvoices()
        {
            PurchaseResponse response = new PurchaseResponse();
            response.PurchaseList = from tbl in _db.tblPurchaseInvoices
                                 join tblcustomer in _db.tblSupplierMasters
                                 on tbl.SupplierLedgerId equals tblcustomer.LedgerId
                                 where tbl.IsDelete == false
                                 select new PurchaseModel
                                 {
                                     Address =tbl.ISCASH.Value?tbl.Address: tblcustomer.Address,
                                     ContactNo = tblcustomer.MobileNo,
                                     SupplierName = tbl.ISCASH.Value ? tbl.SupplierName : tblcustomer.SupplierName,
                                     SupplierLedgerId = tbl.SupplierLedgerId,
                                     Discount = tbl.Discount,
                                     PIDate = tbl.PIDate,
                                     PONo = tbl.PONo,
                                     
                                     ISCASH = tbl.ISCASH,
                                     Amount = tbl.Amount,
                                     IsDelete = false,
                                     RefDocNo=tbl.RefDocNo,
                                     PINo = tbl.PINo,
                                     Tax1=tbl.Tax1,
                                     Tax2=tbl.Tax2,
                                  PurchaseDetails = (from tblsd in _db.tblPurchaseInvoiceDetails
                                                     join tblprod in _db.tblProductMasters
                                                     on tblsd.ProductCode equals tblprod.ProductId
                                                     join tbluom in _db.tblUnitMasters
                                                     on tblprod.UOMId equals tbluom.UnitCode
                                                     where tblsd.IsDelete == false
                                                     select new PurchaseDetailModel
                                                     {
                                                         BatchNo = tblsd.BatchNo,
                                                         ExpiryDate = tblsd.ExpiryDate,
                                                         PINo=tblsd.PINo,
                                                         ItemCode = tblsd.ProductCode,
                                                         ProductAmount = tblsd.InvoicePrice,
                                                         Quantity = tblsd.InvoiceQty,
                                                         Rate = tblsd.InvoicePrice,
                                                         SRNO = tblsd.PINoSrNo,
                                                         ProductName = tblprod.ProductName,
                                                         UOMId = tbluom.UnitCode,
                                                         UOMName = tbluom.UnitDesc,
                                                     })
                                 };
            return response;
        }


        public PurchaseResponse Save(PurchaseModel model)
        {
            PurchaseResponse response = new PurchaseResponse();
            tblPurchaseInvoice inv = new tblPurchaseInvoice()
            {
                Discount = model.Discount,
                ISCASH = model.ISCASH,
                IsDelete = false,
                Address=model.Address,
                Amount=model.Amount,
                ContactNo=model.ContactNo,
                PIDate=model.PIDate,
                PONo=model.PONo,
                RefDocNo=model.RefDocNo,
                SupplierLedgerId=model.SupplierLedgerId,
                SupplierName=model.SupplierName,
                Tax1=model.Tax1,
                Tax2=model.Tax2
            };

            _db.tblPurchaseInvoices.Add(inv);
            _db.SaveChanges();
            foreach (var item in model.PurchaseDetails)
            {
                tblPurchaseInvoiceDetail invdtl = new tblPurchaseInvoiceDetail()
                {
                    BatchNo = item.BatchNo,
                    ExpiryDate = item.ExpiryDate,
                    IsDelete = false,
                    ProductCode = item.ItemCode,
                    Amount = item.ProductAmount,
                    InvoiceQty = item.Quantity,
                    InvoicePrice = item.Rate,
                    PINo=model.PINo,
                };

                _db.tblPurchaseInvoiceDetails.Add(invdtl);


                tblStockDetail stock = new tblStockDetail()
                {
                    BatchNo = item.BatchNo,
                    DocumentNo = inv.PINo,
                    ExpiryDate = item.ExpiryDate,
                    InwardAmount = item.ProductAmount,
                    InwardPrice = item.Rate,
                    InwardQty = item.Quantity,
                    ProductId = item.ItemCode,
                    TransactionType = "Purchase",
                };
                _db.tblStockDetails.Add(stock);
                _db.SaveChanges();
            }
            tblTransaction transaction = new tblTransaction()
            {

            };
            return response;
        }

        public PurchaseResponse Update(PurchaseModel model)
        {
            PurchaseResponse response = new PurchaseResponse();
            return response;
        }


    }
}