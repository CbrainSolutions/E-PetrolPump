using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetrolPumpERP.Controllers
{
    public class DesignationController : Controller
    {
        DesignationBL objSubledger = DesignationBL.Instance;
        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            return View();
        }

        [MyAuthorizeAttribute]
        [HttpGet]
        public ActionResult GetAllDesignations()
        {
            return Json(objSubledger.GetAllDesignations(),JsonRequestBehavior.AllowGet);
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(DesignationModel model)
        {
            return Json(objSubledger.Save(model));
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Update(DesignationModel model)
        {
            return Json(objSubledger.Update(model));
        }
    }
}