using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetrolPumpERP.Models.DataModels;
using PetrolPumpERP.Models.DataEntities;
using PetrolPumpERP.Models;

namespace PetrolPumpERP.Controllers
{
    public class OtherAccountController : Controller
    {
        OtherAccountModelBL objbank = OtherAccountModelBL.Instance;
        AccountTypeBL objAccounttype = AccountTypeBL.Instance;
        SubledgerBL objsubledger = SubledgerBL.Instance;
        
        // GET: OtherAccount
        [MyAuthorize]
        public ActionResult Index()
        {
            ViewBag.AccontTypeList = objAccounttype.GetAccountTypes();
            ViewBag.SubledgerList = objAccounttype.GetAccountTypesDetails(null);
            return View(objbank.GetOtherAccountDetails());
        }

        [HttpPost]
        public ActionResult Save(OtherAccountModel model)
        {
            return Json(objbank.Save(model));
        }

        [HttpPost]
        public ActionResult Update(OtherAccountModel model)
        {
            return Json(objbank.Update(model));
        }
    }
}