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
            ViewBag.CustomerTypeList = objCustomer.GetCustomerTypes().CustomerTypeList;
            return View(objCustomer.GetAllCustomers());
        }


    }
}