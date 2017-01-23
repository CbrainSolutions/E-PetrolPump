using PetrolPumpERP.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetrolPumpERP.Controllers
{
    public class EmployeeController : Controller
    {
        
        EmployeeBL oEmployeeBL = EmployeeBL.Instance;
        public ActionResult Index()
        {
            return View(oEmployeeBL.GetAllEmployees());
        }

        [HttpPost]
        public ActionResult Save(Employee model)
        {
            return Json(oEmployeeBL.Save(model));
        }


        public ActionResult Update(Employee model)
        {
            return Json(oEmployeeBL.Update(model));
        }
    }
}