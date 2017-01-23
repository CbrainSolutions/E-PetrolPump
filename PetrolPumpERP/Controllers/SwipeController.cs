using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataModels;

namespace PetrolPumpERP.Controllers
{
    public class SwipeController : Controller
    {
        SwipeMachineBL objSwipe = SwipeMachineBL.Instance;
        [MyAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        //[MyAuthorize]
        [HttpGet]
        public ActionResult GetData()
        {
            return Json(objSwipe.GetAllSwipeMachines(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(SwipeMachineModel model)
        {
            return Json(objSwipe.Save(model));
        }

        public ActionResult Update(SwipeMachineModel model)
        {
            return Json(objSwipe.Update(model));
        }


    }
}