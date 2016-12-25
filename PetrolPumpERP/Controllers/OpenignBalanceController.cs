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
            return View(objOpeningBal.GetAllOpeningBalLedgers());
        }


    }
}