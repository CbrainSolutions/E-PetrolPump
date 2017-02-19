using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PetrolPumpERP.Models.DataEntities;
using PetrolPumpERP.Models;
using System.Data.Entity.Validation;

namespace PetrolPumpERP.Models.DataModels
{
    public class SalesInvoiceModels
    {
        public SalesInvoiceModels()
        {
            TaxList = new List<OtherAccountModel>();
            SalesDetails = new List<SalesDetailModel>();
        }
        public long InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long CustomerLedgerId { get; set; }
        public bool ISCASH { get; set; }
        public string CustName { get; set; }
        public string CustAddress { get; set; }
        public string CustContactNo { get; set; }
        public decimal NetVAT { get; set; }
        public decimal NetInvoiceAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
        public decimal? Discount { get; set; }

        public long? SwipeMachineId { get; set; }

        public decimal? RoundValue { get; set; }

        public long BankLedgerId { get; set; }

        public decimal NetAmount { get; set; }

        public bool? IsDelete { get; set; }

        public List<OtherAccountModel> TaxList { get; set; }

        public List<SalesDetailModel> SalesDetails { get; set; }

        public bool? RoundOff { get; set; }
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

        public long ProductTypeId { get; set; }

        public string ProductType { get; set; }

        public string UOMName { get; set; }
    }

    public class SalesResponse : Error
    {
        public SalesResponse()
        {
            BankList = new BankModelResponse();
            Customers = new CustomerResponse();
            ProductList = new ProductResponse();
            SwipeMachines = new SwipeMachineResponse();
        }
        public BankModelResponse BankList { get; set; }
        public CustomerResponse Customers { get; set; }
        public ProductResponse ProductList { get; set; }

        public SwipeMachineResponse SwipeMachines { get; set; }
        public IQueryable<OtherAccountModel> OtherAccounts { get; set; }
        
        public IQueryable<SalesInvoiceModels> SalesList { get; set; }
        public IQueryable<OtherAccountModel> TaxList { get; set; }
    }

    public class SalesModelBL
    {
        public static SalesModelBL _userBl = null;
        public static OtherAccountModel _SalesLedger = null;
        public static tblLedger _CashLedger = null;
        SubledgerBL subledger = SubledgerBL.Instance;
        OtherAccountModelBL otherac = OtherAccountModelBL.Instance;
        BankModelBL bankbl = BankModelBL.Instance;
        CustomerModelBL customer = CustomerModelBL.Instance;
        ProductModelBL objproduct = ProductModelBL.Instance;
        SwipeMachineBL objSwipe = SwipeMachineBL.Instance;
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
                                   on tbl.CustomerLedgerId equals tblcustomer.LedgerId into gj
                                   from x in gj.DefaultIfEmpty()
                                   where tbl.IsDelete == false
                                   select new SalesInvoiceModels
                                   {
                                       BalanceAmount=tbl.BalanceAmount,
                                       CustAddress=tbl.ISCASH?tbl.CustAddress:(x==null?string.Empty:x.Address),
                                       CustContactNo= x == null ? string.Empty : x.Address,
                                       CustName= tbl.ISCASH?tbl.CustName: (x == null ? string.Empty : x.CustomerFirstName + " " + x.CustomeLName),
                                       CustomerLedgerId=tbl.CustomerLedgerId,
                                       Discount=tbl.Discount,
                                       InvoiceDate=tbl.InvoiceDate,
                                       InvoiceNo=tbl.InvoiceNo,
                                       ISCASH=tbl.ISCASH,
                                       NetAmount=tbl.NetAmount,
                                       NetInvoiceAmount=tbl.NetInvoiceAmount,
                                       IsDelete=tbl.IsDelete,
                                       NetVAT=tbl.NetVAT,
                                       RoundOff=tbl.IsRoundOff,
                                       SwipeMachineId=tbl.SwipeMachineId,
                                       SalesDetails=(from tblsd in _db.tbl_InvoiceDetail
                                                     join tblprod in _db.tblProductMasters
                                                     on tblsd.ItemCode equals tblprod.ProductId
                                                     join tbluom in _db.tblUnitMasters
                                                     on tblprod.UOMId equals tbluom.UnitCode
                                                     where tblsd.IsDelete==false
                                                     && tblsd.InvoiceNo==tbl.InvoiceNo
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
                                                     }).ToList(),
                                       TaxList=(from tax in _db.tblTransactions
                                                join tblotherac in _db.tblOtherAccounts
                                                on tax.CrLedgerId equals tblotherac.LedgerId
                                                where tblotherac.IsPercent==true &&
                                                tax.BillNo==tbl.InvoiceNo
                                                select new OtherAccountModel
                                                {
                                                    AccountName=tblotherac.AccountName,
                                                    PercentOrFixedAmount=tblotherac.PercentOrFixedAmount,
                                                    IsPercent=tblotherac.IsPercent,
                                                    RoundOff=tblotherac.RoundOff,
                                                    LedgerId=tblotherac.LedgerId,
                                                    OtherAccountId=tblotherac.OtherAccountId,
                                                    IsDelete=tblotherac.IsDelete,
                                                }).ToList()

                                   };

            response.BankList = bankbl.GetBankDetails();
            response.Customers = customer.GetAllCustomers();
            response.TaxList = otherac.GetOtherAccountDetails().OtherAccountList.Where(p => p.SubledgerId == 12 || p.OtherAccountId == 2 || p.OtherAccountId == 3);
            response.OtherAccounts = otherac.GetOtherAccountDetails().OtherAccountList.Where(p => p.SubledgerId == 8 || p.SubledgerId == 9);
            response.ProductList = objproduct.GetProducts();
            response.SwipeMachines = objSwipe.GetAllSwipeMachines();
            return response;
        }

        

        private OtherAccountModel GetRoundOffLedger()
        {
            return otherac.GetOtherAccountDetails().OtherAccountList.Where(p => p.OtherAccountId == 3).FirstOrDefault();
        }


        public SalesResponse Save(SalesInvoiceModels model)
        {
            SalesResponse response = new SalesResponse();
            response.Status = false;
            try
            {
                AppSettingManager settings = AppSettingManager.Instance;
                tbl_Invoice inv = new tbl_Invoice()
                {
                    BalanceAmount = model.BalanceAmount,
                    CustAddress = model.CustAddress,
                    CustContactNo = model.CustContactNo,
                    CustName = model.ISCASH?model.CustName:"",
                    CustomerLedgerId = model.CustomerLedgerId,
                    Discount = model.Discount,
                    InvoiceDate = model.InvoiceDate,
                    ISCASH = model.ISCASH,
                    IsDelete = false,
                    NetAmount = Math.Round(model.NetAmount, 0),
                    NetInvoiceAmount = model.NetInvoiceAmount,
                    NetVAT = model.NetVAT,
                    IsRoundOff=model.RoundOff,
                    SwipeMachineId=model.SwipeMachineId,
                };

                _db.tbl_Invoice.Add(inv);
                _db.SaveChanges();
                foreach (var item in model.SalesDetails)
                {
                    tbl_InvoiceDetail invdtl = new tbl_InvoiceDetail()
                    {
                        BatchNo = item.BatchNo,
                        ExpiryDate = item.ExpiryDate,
                        InvoiceNo = inv.InvoiceNo,
                        IsDelete = false,
                        ItemCode = item.ItemCode,
                        ProductAmount = item.ProductAmount,
                        Quantity = item.Quantity,
                        Rate = item.Rate,
                        TotalAmount = item.TotalAmount,
                        VAT = item.VAT,
                        VATAmount = item.VATAmount,
                    };

                    _db.tbl_InvoiceDetail.Add(invdtl);

                    tblProductMaster producttype = _db.tblProductMasters.Where(p => p.ProductId == item.ItemCode).FirstOrDefault();
                    if (producttype.ProductTypeId != 3)
                    {
                        tblStockDetail stock = new tblStockDetail()
                        {
                            BatchNo = item.BatchNo,
                            DocumentNo = item.InvoiceNo,
                            ExpiryDate = item.ExpiryDate,
                            OutwardAmount = item.ProductAmount,
                            OutwardPrice = item.Rate,
                            OutwardQty = item.Quantity,
                            ProductId = item.ItemCode,
                            TransactionType = "Sales",
                        };
                        _db.tblStockDetails.Add(stock);
                    }
                    _db.SaveChanges();
                }

                long? trasid = _db.tblTransactions.Max(p => p.TransactionId);
                if (trasid==null)
                {
                    trasid = 1;
                }

                tblTransaction transaction = new tblTransaction()
                {
                    CustomerId = 0,
                    TransactionDate = model.InvoiceDate,
                    DrLedger = model.ISCASH?settings.CashLedger.LedgerId:model.CustomerLedgerId,
                    DrAmount = Math.Round(model.NetAmount, 0),
                    BillNo = inv.InvoiceNo,
                    TransactionId = trasid,
                    FinancialId = 1,
                    IsDelete = false,
                    TransactionType = Convert.ToString(TransactionTypes.PetroliumSales),
                    UserId = SessionManager.Instance.ActiveUser.UserId,
                };
                if (model.ISCASH)
                {
                    if (model.BankLedgerId>0)
                    {
                        transaction.DrLedger = model.BankLedgerId;
                    }
                }
                _db.tblTransactions.Add(transaction);
                if (model.RoundOff != null && Convert.ToBoolean(model.RoundOff))
                {
                    transaction = new tblTransaction()
                    {
                        CustomerId = 0,
                        TransactionDate = model.InvoiceDate,
                        CrLedgerId = GetRoundOffLedger().LedgerId,
                        CrAmount = model.RoundValue,
                        BillNo = inv.InvoiceNo,
                        TransactionId = trasid,
                        FinancialId = 1,
                        IsDelete = false,
                        TransactionType = Convert.ToString(TransactionTypes.PetroliumSales),
                        UserId = SessionManager.Instance.ActiveUser.UserId,
                    };
                    _db.tblTransactions.Add(transaction);
                }

                transaction = new tblTransaction()
                {
                    CustomerId = 0,
                    TransactionDate = model.InvoiceDate,
                    CrLedgerId = settings.SalesLedger.LedgerId,
                    CrAmount = model.NetInvoiceAmount,
                    BillNo = inv.InvoiceNo,
                    TransactionId = trasid,
                    FinancialId = 1,
                    IsDelete = false,
                    TransactionType = Convert.ToString(TransactionTypes.PetroliumSales),
                    UserId = SessionManager.Instance.ActiveUser.UserId,
                };
                _db.tblTransactions.Add(transaction);
                foreach (var item in model.TaxList)
                {
                    transaction = new tblTransaction()
                    {
                        CustomerId = 0,
                        TransactionDate = model.InvoiceDate,
                        CrLedgerId = item.LedgerId,
                        CrAmount = item.Amount,
                        BillNo = inv.InvoiceNo,
                        TransactionId = trasid,
                        FinancialId = 1,
                        IsDelete = false,
                        TransactionType = Convert.ToString(TransactionTypes.PetroliumSales),
                        UserId = SessionManager.Instance.ActiveUser.UserId,
                    };
                    _db.tblTransactions.Add(transaction);
                }
                _db.SaveChanges();
                response.Id = inv.InvoiceNo;
                response.Status = true;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                    }
                }
            }
            
            return response;
        }

        public SalesResponse Update(SalesInvoiceModels model)
        {
            SalesResponse response = new SalesResponse();
            tblTransaction transaction = _db.tblTransactions.Where(p => p.BillNo == model.InvoiceNo && p.TransactionType.ToUpper() == TransactionTypes.PetroliumSales.ToString().ToUpper()).FirstOrDefault();
            try
            {
                tbl_Invoice inv = _db.tbl_Invoice.Where(p => p.InvoiceNo == model.InvoiceNo).FirstOrDefault();
                if (inv!=null)
                {
                    inv.BalanceAmount = model.BalanceAmount;
                    inv.CustAddress = model.CustAddress;
                    inv.CustContactNo = model.CustContactNo;
                    inv.CustName = model.ISCASH ? model.CustName : "";
                    inv.CustomerLedgerId = model.CustomerLedgerId;
                    inv.Discount = model.Discount;
                    inv.InvoiceDate = model.InvoiceDate;
                    inv.ISCASH = model.ISCASH;
                    inv.NetAmount = Math.Round(model.NetAmount, 0);
                    inv.NetInvoiceAmount = model.NetInvoiceAmount;
                    inv.NetVAT = model.NetVAT;
                    inv.IsRoundOff = model.RoundOff;
                    inv.SwipeMachineId = model.SwipeMachineId;

                    IQueryable<tbl_InvoiceDetail> salesdetail = _db.tbl_InvoiceDetail.Where(p => p.InvoiceNo == model.InvoiceNo && p.IsDelete==false);
                    // Find to delete removed products from db
                    foreach (var item in salesdetail)
                    {
                        var test= model.SalesDetails.Where(p => p.ItemCode == item.ItemCode).FirstOrDefault();
                        if (test==null)
                        {
                            item.IsDelete = true;
                        }
                        
                        if (test==null)
                        {
                            tblProductMaster producttype = _db.tblProductMasters.Where(p => p.ProductId == item.ItemCode).FirstOrDefault();
                            if (producttype.ProductTypeId != 3)
                            {
                                tblStockDetail stock = _db.tblStockDetails.Where(p => p.DocumentNo == item.InvoiceNo && p.ProductId == item.ItemCode && p.TransactionType == "Sales").FirstOrDefault();
                                if (stock!=null)
                                {
                                    stock.IsDelete = true;
                                }
                            }
                        }
                        else
                        {
                            tblProductMaster producttype = _db.tblProductMasters.Where(p => p.ProductId == test.ItemCode).FirstOrDefault();
                            if (producttype.ProductTypeId == 3)
                            {
                                // Product Category was change  while update it 
                                tblStockDetail stock = _db.tblStockDetails.Where(p => p.DocumentNo == item.InvoiceNo && p.ProductId == test.ItemCode && p.TransactionType == "Sales").FirstOrDefault();
                                if (stock != null)
                                {
                                    stock.IsDelete = true;
                                }
                            }
                        }
                        
                        _db.SaveChanges();
                    }
                    salesdetail = _db.tbl_InvoiceDetail.Where(p => p.InvoiceNo == model.InvoiceNo && p.IsDelete == false);
                    foreach (var item in salesdetail)
                    {
                        var test = model.SalesDetails.Where(p => p.ItemCode == item.ItemCode).FirstOrDefault();
                        if (test != null)
                        {
                            item.BatchNo = test.BatchNo;
                            item.ExpiryDate = test.ExpiryDate;
                            item.ItemCode = test.ItemCode;
                            item.ProductAmount = test.ProductAmount;
                            item.Quantity = test.Quantity;
                            item.Rate = test.Rate;
                            item.TotalAmount = test.TotalAmount;
                            item.VAT = test.VAT;
                            item.VATAmount = test.VATAmount;

                            tblProductMaster producttype = _db.tblProductMasters.Where(p => p.ProductId == test.ItemCode).FirstOrDefault();
                            if (producttype.ProductTypeId != 3)
                            {
                                tblStockDetail stock = _db.tblStockDetails.Where(p => p.DocumentNo == item.InvoiceNo && p.ProductId == test.ItemCode && p.TransactionType == "Sales").FirstOrDefault();
                                if (stock != null)
                                {
                                    stock.BatchNo = test.BatchNo;
                                    stock.DocumentNo = test.InvoiceNo;
                                    stock.ExpiryDate = test.ExpiryDate;
                                    stock.OutwardAmount = test.ProductAmount;
                                    stock.OutwardPrice = test.Rate;
                                    stock.OutwardQty = test.Quantity;
                                    stock.ProductId = test.ItemCode;
                                    stock.TransactionType = "Sales";
                                }
                            }
                        }
                    }

                    foreach (var item in model.SalesDetails)
                    {
                        var test = _db.tbl_InvoiceDetail.Where(p => p.ItemCode == item.ItemCode && p.InvoiceNo==model.InvoiceNo).FirstOrDefault();
                        if (test == null)
                        {
                            tbl_InvoiceDetail invdtl = new tbl_InvoiceDetail()
                            {
                                BatchNo = item.BatchNo,
                                ExpiryDate = item.ExpiryDate,
                                InvoiceNo = inv.InvoiceNo,
                                IsDelete = false,
                                ItemCode = item.ItemCode,
                                ProductAmount = item.ProductAmount,
                                Quantity = item.Quantity,
                                Rate = item.Rate,
                                TotalAmount = item.TotalAmount,
                                VAT = item.VAT,
                                VATAmount = item.VATAmount,
                            };

                            _db.tbl_InvoiceDetail.Add(invdtl);

                            tblProductMaster producttype = _db.tblProductMasters.Where(p => p.ProductId == item.ItemCode).FirstOrDefault();
                            if (producttype.ProductTypeId != 3)
                            {
                                tblStockDetail stock = new tblStockDetail()
                                {
                                    BatchNo = item.BatchNo,
                                    DocumentNo = item.InvoiceNo,
                                    ExpiryDate = item.ExpiryDate,
                                    OutwardAmount = item.ProductAmount,
                                    OutwardPrice = item.Rate,
                                    OutwardQty = item.Quantity,
                                    ProductId = item.ItemCode,
                                    TransactionType = "Sales",
                                };
                                _db.tblStockDetails.Add(stock);
                            }
                            _db.SaveChanges();
                        }
                    }
                }
                var taxlist= otherac.GetOtherAccountDetails().OtherAccountList.Where(p => p.SubledgerId == 12 || p.OtherAccountId == 2 || p.OtherAccountId == 3);
                IQueryable<tblTransaction> lstTrans = _db.tblTransactions.Where(p => p.BillNo == model.InvoiceNo && p.TransactionType.ToUpper() == TransactionTypes.PetroliumSales.ToString().ToUpper());
                foreach (var item in lstTrans)
                {
                    var checktax = taxlist.Where(p => p.LedgerId == item.CrLedgerId).FirstOrDefault();
                    if (checktax!=null)
                    {
                        var test = model.TaxList.Where(p => p.LedgerId == item.CrLedgerId).FirstOrDefault();
                        if (test == null)
                        {
                            item.IsDelete = true;
                        }
                    }
                }
                foreach (var item in lstTrans)
                {
                    var checktax = taxlist.Where(p => p.LedgerId == item.CrLedgerId).FirstOrDefault();
                    if (checktax != null)
                    {
                        var test = model.TaxList.Where(p => p.LedgerId == item.CrLedgerId).FirstOrDefault();
                        if (test != null)
                        {
                            item.CustomerId = 0;
                            item.TransactionDate = model.InvoiceDate;
                            item.CrLedgerId = test.LedgerId;
                            item.CrAmount = test.Amount;
                            item.BillNo = inv.InvoiceNo;
                            item.FinancialId = 1;
                        }
                        
                    }
                }
                _db.SaveChanges();
                foreach (var item in model.TaxList)
                {
                    tblTransaction transactionexist = _db.tblTransactions.Where(p => p.CrLedgerId == item.LedgerId && p.BillNo == model.InvoiceNo && p.TransactionType.ToUpper() == TransactionTypes.PetroliumSales.ToString().ToUpper()).FirstOrDefault();
                    if (transactionexist == null)
                    {
                        if (transaction!=null)
                        {
                            tblTransaction transaction1 = new tblTransaction()
                            {
                                CustomerId = 0,
                                TransactionDate = model.InvoiceDate,
                                CrLedgerId = item.LedgerId,
                                CrAmount = item.Amount,
                                BillNo = inv.InvoiceNo,
                                TransactionId = transaction.TransactionId,
                                FinancialId = 1,
                                IsDelete = false,
                                TransactionType = Convert.ToString(TransactionTypes.PetroliumSales),
                                UserId = SessionManager.Instance.ActiveUser.UserId,
                            };
                            _db.tblTransactions.Add(transaction1);
                            _db.SaveChanges();
                        }
                    }
                }


                // Update Round off ledger
                var roundoffledger = GetRoundOffLedger();
                var checkexist = lstTrans.Where(p => p.CrLedgerId == roundoffledger.LedgerId && p.BillNo == model.InvoiceNo && p.TransactionType.ToUpper() == TransactionTypes.PetroliumSales.ToString().ToUpper()).FirstOrDefault();
                if (model.RoundOff!=null && Convert.ToBoolean(model.RoundOff))
                {
                    if (checkexist!=null)
                    {
                        checkexist.CrAmount = model.RoundValue;
                    }
                    else
                    {
                        tblTransaction transactionround = new tblTransaction()
                        {
                            CustomerId = 0,
                            TransactionDate = model.InvoiceDate,
                            CrLedgerId = GetRoundOffLedger().LedgerId,
                            CrAmount = model.RoundValue,
                            BillNo = inv.InvoiceNo,
                            TransactionId = transaction.TransactionId,
                            FinancialId = 1,
                            IsDelete = false,
                            TransactionType = Convert.ToString(TransactionTypes.PetroliumSales),
                            UserId = SessionManager.Instance.ActiveUser.UserId,
                        };
                        _db.tblTransactions.Add(transactionround);
                    }
                }
                else
                {
                    if (checkexist!=null)
                    {
                        checkexist.IsDelete = true;
                    }
                }

                var checkexistsales = lstTrans.Where(p => p.CrLedgerId == AppSettingManager.Instance.SalesLedger.LedgerId && p.BillNo == model.InvoiceNo && p.TransactionType.ToUpper() == TransactionTypes.PetroliumSales.ToString().ToUpper()).FirstOrDefault();
                if (checkexistsales != null)
                {
                    checkexistsales.CrAmount = model.NetInvoiceAmount;
                }

                var checkexistdr = lstTrans.Where(p => p.DrLedger >0 && p.BillNo == model.InvoiceNo && p.TransactionType.ToUpper() == TransactionTypes.PetroliumSales.ToString().ToUpper()).FirstOrDefault();
                if (checkexistdr != null)
                {
                    checkexistdr.DrAmount = model.NetAmount;
                    checkexistdr.DrLedger = model.ISCASH ? (model.BankLedgerId>0?model.BankLedgerId:AppSettingManager.Instance.CashLedger.LedgerId) : model.CustomerLedgerId;
                }

                _db.SaveChanges();

                // Update Sales Ledger


            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                    }
                }
            }
            return response;
        }


    }
}