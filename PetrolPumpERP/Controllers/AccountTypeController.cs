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
        

        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            //ViewBag.SubledgerList = 
            return View();
        }

        [MyAuthorizeAttribute]
        public ActionResult GetAccountTypes()
        {
            return Json(objAccounttype.GetAccountTypes());
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(List<AccountTypeModel> model)
        {
            return Json(objAccounttype.SaveAccountType(model),JsonRequestBehavior.AllowGet);
        }

        [MyAuthorizeAttribute]
        public ActionResult GetAccountTypeDetails(int? AcId)
        {
            return Json(objAccounttype.GetAccountTypesDetails(AcId),JsonRequestBehavior.AllowGet);
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Update(List<AccountTypeModel> model)
        {
            return Json(objAccounttype.UpdateAccountType(model));
        }

    }
}