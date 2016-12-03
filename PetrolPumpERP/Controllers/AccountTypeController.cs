using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetrolPumpERP.Controllers
{
    public class AccountTypeController : Controller
    {
        // GET: AccountType
        AccountTypeBL objAccounttype = AccountTypeBL.Instance;
        SubledgerBL objSubledger = SubledgerBL.Instance;

        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            ViewBag.SubledgerList = objSubledger.GetAllSubledgers();
            return View(objAccounttype.GetAccountTypes());
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(AccountTypeModel model)
        {
            return Json(objAccounttype.SaveAccountType(model));
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Update(AccountTypeModel model)
        {
            return Json(objAccounttype.UpdateAccountType(model));
        }

    }
}