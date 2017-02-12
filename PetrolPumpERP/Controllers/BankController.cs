using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetrolPumpERP.Models.DataModels;
using PetrolPumpERP.Models;

namespace PetrolPumpERP.Controllers
{
    public class BankController : Controller
    {
        BankModelBL objbank = BankModelBL.Instance;
        
        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            return View();
        }

        [MyAuthorizeAttribute]
        [HttpGet]
        public ActionResult GetBankDetails()
        {
            return Json(objbank.GetBankDetails(),JsonRequestBehavior.AllowGet);
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(BankModel model)
        {
            return Json(objbank.Save(model));
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Update(BankModel model)
        {
            return Json(objbank.Update(model));
        }
    }
}