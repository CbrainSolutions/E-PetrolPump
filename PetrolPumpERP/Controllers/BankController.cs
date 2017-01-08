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
        AccountTypeBL objAccounttype = AccountTypeBL.Instance;
        SubledgerBL objsubledger = SubledgerBL.Instance;
        // GET: Bank
        [MyAuthorize]
        public ActionResult Index()
        {
            ViewBag.AccontTypeList = objAccounttype.GetAccountTypes();
            ViewBag.SubledgerList = objAccounttype.GetAccountTypesDetails(null);
            return View(objbank.GetBankDetails());
        }

        [HttpPost]
        public ActionResult Save(BankModel model)
        {
            return Json(objbank.Save(model));
        }

        [HttpPost]
        public ActionResult Update(BankModel model)
        {
            return Json(objbank.Update(model));
        }
    }
}