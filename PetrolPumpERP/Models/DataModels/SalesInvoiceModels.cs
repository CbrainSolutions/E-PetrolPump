using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetrolPumpERP.Models.DataEntities;
using PetrolPumpERP.Models;

namespace PetrolPumpERP.Models.DataModels
{
    public class SalesInvoiceModels
    {
        public long InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long CustomerLedgerId { get; set; }
        public bool ISCASH { get; set; }
        public string CustName { get; set; }
        public string CustAddress { get; set; }
        public string CustContactNo { get; set; }
        public decimal NetVAT { get; set; }
        public decimal NetInvoiceAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }

        public bool? IsDelete { get; set; }

        public IQueryable<SalesDetailModel> SalesDetails { get; set; }
    }

    public class SalesDetailModel
    {
        public long SRNO { get; set; }
        public Nullable<long> InvoiceNo { get; set; }
        public Nullable<long> ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public string BatchNo { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public decimal Rate { get; set; }
        public decimal ProductAmount { get; set; }
        public Nullable<decimal> VAT { get; set; }
        public Nullable<decimal> VATAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }

        public string ProductName { get; set; }

        public int UOMId { get; set; }

        public bool? IsDelete { get; set; }

        public string UOMName { get; set; }
    }

    public class SalesResponse : Error
    {
        public IQueryable<SalesInvoiceModels> SalesList { get; set; }
    }

    public class SalesModelBL
    {
        public static SalesModelBL _userBl = null;
        public static OtherAccountModel _SalesLedger = null;
        public static tblLedger _CashLedger = null;
        PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private SalesModelBL()
        {

        }

        public static SalesModelBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new SalesModelBL();
                }
                return _userBl;
            }
        }

        public SalesResponse GetSalesInvoices()
        {
            SalesResponse response = new SalesResponse();
            response.SalesList = from tbl in _db.tbl_Invoice
                                   join tblcustomer in _db.tbl_CustomerInfo
                                   on tbl.CustomerLedgerId equals tblcustomer.LedgerId
                                   where tbl.IsDelete == false
                                   select new SalesInvoiceModels
                                   {
                                       BalanceAmount=tbl.BalanceAmount,
                                       CustAddress=tbl.ISCASH?tbl.CustAddress: tblcustomer.Address,
                                       CustContactNo=tblcustomer.MobileNo,
                                       CustName= tbl.ISCASH?tbl.CustName: tblcustomer.CustomerFirstName,
                                       CustomerLedgerId=tbl.CustomerLedgerId,
                                       Discount=tbl.Discount,
                                       InvoiceDate=tbl.InvoiceDate,
                                       InvoiceNo=tbl.InvoiceNo,
                                       ISCASH=tbl.ISCASH,
                                       NetAmount=tbl.NetAmount,
                                       NetInvoiceAmount=tbl.NetInvoiceAmount,
                                       NetVAT=tbl.NetVAT,
                                       SalesDetails=(from tblsd in _db.tbl_InvoiceDetail
                                                     join tblprod in _db.tblProductMasters
                                                     on tblsd.ItemCode equals tblprod.ProductId
                                                     join tbluom in _db.tblUnitMasters
                                                     on tblprod.UOMId equals tbluom.UnitCode
                                                     where tblsd.IsDelete==false
                                                     select new SalesDetailModel
                                                     {
                                                         BatchNo=tblsd.BatchNo,
                                                         ExpiryDate=tblsd.ExpiryDate,
                                                         InvoiceNo=tblsd.InvoiceNo,
                                                         ItemCode=tblsd.ItemCode,
                                                         ProductAmount=tblsd.ProductAmount,
                                                         Quantity=tblsd.Quantity,
                                                         Rate=tblsd.Rate,
                                                         SRNO=tblsd.SRNO,
                                                         TotalAmount=tblsd.TotalAmount,
                                                         VAT=tblsd.VAT,
                                                         VATAmount=tblsd.VATAmount,
                                                         ProductName=tblprod.ProductName,
                                                         UOMId=tbluom.UnitCode,
                                                         UOMName=tbluom.UnitDesc,
                                                     })
                                   };
            return response;
        }


        public SalesResponse Save(SalesInvoiceModels model)
        {
            SalesResponse response = new SalesResponse();
            tbl_Invoice inv = new tbl_Invoice()
            {
                BalanceAmount=model.BalanceAmount,
                CustAddress=model.CustAddress,
                CustContactNo=model.CustContactNo,
                CustName=model.CustName,
                CustomerLedgerId=model.CustomerLedgerId,
                Discount=model.Discount,
                InvoiceDate=model.InvoiceDate,
                ISCASH=model.ISCASH,
                IsDelete=false,
                NetAmount=model.NetAmount,
                NetInvoiceAmount=model.NetInvoiceAmount,
                NetVAT=model.NetVAT,
            };

            _db.tbl_Invoice.Add(inv);
            _db.SaveChanges();
            foreach (var item in model.SalesDetails)
            {
                tbl_InvoiceDetail invdtl = new tbl_InvoiceDetail()
                {
                    BatchNo=item.BatchNo,
                    ExpiryDate=item.ExpiryDate,
                    InvoiceNo=inv.InvoiceNo,
                    IsDelete=false,
                    ItemCode=item.ItemCode,
                    ProductAmount=item.ProductAmount,
                    Quantity=item.Quantity,
                    Rate=item.Rate,
                    TotalAmount=item.TotalAmount,
                    VAT=item.VAT,
                    VATAmount=item.VATAmount,  
                };

                _db.tbl_InvoiceDetail.Add(invdtl);
                

                tblStockDetail stock = new tblStockDetail()
                {
                    BatchNo=item.BatchNo,
                    DocumentNo=item.InvoiceNo,
                    ExpiryDate=item.ExpiryDate,
                    OutwardAmount=item.ProductAmount,
                    OutwardPrice=item.Rate,
                    OutwardQty=item.Quantity,
                    ProductId=item.ItemCode,
                    TransactionType="Sales",
                };
                _db.tblStockDetails.Add(stock);
                _db.SaveChanges();
            }
            tblTransaction transaction = new tblTransaction()
            {

            };
            return response;
        }

        public SalesResponse Update(SalesInvoiceModels model)
        {
            SalesResponse response = new SalesResponse();
            return response;
        }


    }
}