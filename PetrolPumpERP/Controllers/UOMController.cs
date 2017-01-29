using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetrolPumpERP.Controllers
{
    public class UOMController : Controller
    {
        UOMBL objuom = UOMBL.Instance;
        // GET: UOM
        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            return View();
        }

        [MyAuthorizeAttribute]
        [HttpGet]
        public ActionResult GetUOMList()
        {
            return Json(objuom.GetUOMList(),JsonRequestBehavior.AllowGet);
        }


        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(UOMModel model)
        {
            return Json(objuom.Save(model), JsonRequestBehavior.AllowGet);
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Update(UOMModel model)
        {
            return Json(objuom.Update(model), JsonRequestBehavior.AllowGet);
        }

    }
}