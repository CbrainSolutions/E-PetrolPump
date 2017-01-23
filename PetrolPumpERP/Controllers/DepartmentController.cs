using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetrolPumpERP.Controllers
{
    public class DepartmentController : Controller
    {

        DepartmentBL objSubledger = DepartmentBL.Instance;
        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            return View(objSubledger.GetAllDepartments());
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(DepartmentModel model)
        {
            return Json(objSubledger.Save(model));
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Update(DepartmentModel model)
        {
            return Json(objSubledger.Update(model));
        }
    }
}