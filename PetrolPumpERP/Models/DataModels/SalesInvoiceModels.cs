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
        public decimal BalanceAmount { get; set; }
        public decimal Discount { get; set; }

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
                        _db.SaveChanges();
                    }
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
                    DrLedger = settings.CashLedger.LedgerId,
                    DrAmount = Math.Round(model.NetAmount, 0),
                    BillNo = inv.InvoiceNo,
                    TransactionId = trasid,
                    FinancialId = 1,
                    IsDelete = false,
                    TransactionType = Convert.ToString(TransactionTypes.PetroliumSales),
                    UserId = SessionManager.Instance.ActiveUser.UserId,
                };
                if (!model.ISCASH)
                {
                    transaction.DrLedger = model.BankLedgerId;
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
            return response;
        }


    }
}