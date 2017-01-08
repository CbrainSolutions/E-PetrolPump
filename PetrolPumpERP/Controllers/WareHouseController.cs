using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataEntities;
using PetrolPumpERP.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetrolPumpERP.Controllers
{
    public class WareHouseController : Controller
    {
        // GET: WareHouse
        WareHouseBL objOpeningBal = WareHouseBL.Instance;
        // GET: OpenignBalance
        [MyAuthorizeAttribute]
        public ActionResult Index()
        {
            return View(objOpeningBal.GetWareHouseList());
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Save(tblWareHouse model)
        {
            return Json(objOpeningBal.Save(model));
        }

        [MyAuthorizeAttribute]
        [HttpPost]
        public ActionResult Update(tblWareHouse model)
        {
            return Json(objOpeningBal.Update(model));
        }
    }
}