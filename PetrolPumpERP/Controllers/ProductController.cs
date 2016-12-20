using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetrolPumpERP.Controllers
{
    public class ProductController : Controller
    {

        //AccountTypeBL objAcType = AccountTypeBL.Instance;
        ProductTypeBL objproduttype = ProductTypeBL.Instance;
        UOMBL objuombl = UOMBL.Instance;
        ProductModelBL objVendor = ProductModelBL.Instance;
        // GET: Vendor
        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            ViewBag.ProductTypeList = objproduttype.GetProductTypeList();
            ViewBag.UOMList = objuombl.GetUOMList();
            return View(objVendor.GetProducts());
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(ProductModel model)
        {
            return Json(objVendor.Save(model));
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Update(ProductModel model)
        {
            return Json(objVendor.Update(model));
        }
    }
}