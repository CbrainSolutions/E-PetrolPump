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

        [MyAuthorize]
        public ActionResult Index()
        {

            return View();
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