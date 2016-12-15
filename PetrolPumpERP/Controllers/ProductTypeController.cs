using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetrolPumpERP.Controllers
{
    public class ProductTypeController : Controller
    {
        ProductTypeBL objProductType = ProductTypeBL.Instance;
        // GET: ProductType
        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            return View(objProductType.GetProductTypeList());
        }


        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(ProductTypeModel model)
        {
            return Json(objProductType.Save(model));
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Update(ProductTypeModel model)
        {
            return Json(objProductType.Update(model));
        }
    }
}