using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetrolPumpERP.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        CustomerModelBL objCustomer = CustomerModelBL.Instance;

        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            return View();
        }

        [MyAuthorizeAttribute]
        [HttpGet]
        public ActionResult GetCustomer()
        {
            return Json(objCustomer.GetAllCustomers(),JsonRequestBehavior.AllowGet);
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(CustomerModel model)
        {
            return Json(objCustomer.SaveCustomer(model),JsonRequestBehavior.AllowGet);
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Update(CustomerModel model)
        {
            return Json(objCustomer.UpdateCustomer(model), JsonRequestBehavior.AllowGet);
        }


    }
}