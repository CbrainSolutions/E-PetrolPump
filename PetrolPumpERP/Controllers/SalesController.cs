using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataModels;

namespace PetrolPumpERP.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        SalesModelBL sales = SalesModelBL.Instance;
        SubledgerBL subledger = SubledgerBL.Instance;
        OtherAccountModelBL otherac = OtherAccountModelBL.Instance;
        BankModelBL bankbl = BankModelBL.Instance;
        CustomerModelBL customer = CustomerModelBL.Instance;
        ProductModelBL objproduct = ProductModelBL.Instance;
        //PetrolPumpERP.Models.DataEntities.PetrolPumpERPEntities _db = new PetrolPumpERP.Models.DataEntities.PetrolPumpERPEntities();

        [MyAuthorize]
        public ActionResult Index()
        {
            ViewBag.BankList = bankbl.GetBankDetails();
            ViewBag.Customers = customer.GetAllCustomers();
            ViewBag.TaxList=otherac.GetOtherAccountDetails().OtherAccountList.Where(p => p.SubledgerId == 12 || p.OtherAccountId==2 || p.OtherAccountId==3);
            ViewBag.OtherAccounts = otherac.GetOtherAccountDetails().OtherAccountList.Where(p => p.SubledgerId == 8 || p.SubledgerId==9);
            ViewBag.ProductList = objproduct.GetProducts();
            
            return View(sales.GetSalesInvoices());
        }

        public ActionResult Save(SalesInvoiceModels model)
        {
            return Json(sales.Save(model));
        }

        public ActionResult Update(SalesInvoiceModels model)
        {
            return View(sales.Update(model));
        }
    }
}