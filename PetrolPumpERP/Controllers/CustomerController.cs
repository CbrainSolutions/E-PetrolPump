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
        AccountTypeBL objAcType = AccountTypeBL.Instance;


        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            ViewBag.CustomerTypeList = objCustomer.GetCustomerTypes().CustomerTypeList;
            ViewBag.AccontTypeList = objAcType.GetAccountTypes();
            ViewBag.SubledgerList = objAcType.GetAccountTypesDetails(null);
            return View(objCustomer.GetAllCustomers());
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