using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetrolPumpERP.Controllers
{
    public class OpenignBalanceController : Controller
    {
        OpeningBalanceBL objOpeningBal = OpeningBalanceBL.Instance;
        // GET: OpenignBalance
        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            return View();
        }

        [MyAuthorizeAttribute]
        [HttpGet]
        public ActionResult GetAllOpeningBalLedgers()
        {
            return Json(objOpeningBal.GetAllOpeningBalLedgers(),JsonRequestBehavior.AllowGet);
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(OpeningBalanceModel model)
        {
            return Json(objOpeningBal.Update(model));
        }
    }
}