using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetrolPumpERP.Controllers
{
    public class VendorController : Controller
    {
        AccountTypeBL objAcType = AccountTypeBL.Instance;
        VendorModelBL objVendor = VendorModelBL.Instance;
        // GET: Vendor
        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            ViewBag.AccontTypeList = objAcType.GetAccountTypes();
            ViewBag.SubledgerList = objAcType.GetAccountTypesDetails(null);
            return View(objVendor.GetVendors());
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(VendorModel model)
        {
            return Json(objVendor.SaveVendor(model));
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Update(VendorModel model)
        {
            return Json(objVendor.UpdateVendor(model));
        }
    }
}