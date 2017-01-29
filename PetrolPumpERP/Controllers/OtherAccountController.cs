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
        
        // GET: OtherAccount
        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            return View();
        }


        [MyAuthorizeAttribute]
        [HttpGet]
        public ActionResult GetOtherAccountDetails()
        {
            return Json(objbank.GetOtherAccountDetails(),JsonRequestBehavior.AllowGet);
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(OtherAccountModel model)
        {
            return Json(objbank.Save(model));
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Update(OtherAccountModel model)
        {
            return Json(objbank.Update(model));
        }
    }
}