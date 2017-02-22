using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetrolPumpERP.Controllers
{
    public class AccessariesSalesController : Controller
    {
        SalesModelBL sales = SalesModelBL.Instance;


        [MyAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [MyAuthorize]
        [HttpGet]
        public ActionResult GetSalesInvoices(int ProductTypeId = 0)
        {
            return Json(sales.GetSalesInvoices(ProductTypeId), JsonRequestBehavior.AllowGet);
        }


        [MyAuthorize]
        [HttpPost]
        public ActionResult Save(SalesInvoiceModels model)
        {
            decimal? round = null;
            if (model.RoundOff != null && Convert.ToBoolean(model.RoundOff))
            {
                decimal roundnet = Math.Round(model.NetAmount, 0);
                decimal? temp = 0;
                if (model.NetAmount < roundnet)
                {
                    temp = roundnet - model.NetAmount;
                    round = temp;
                }
                else
                {
                    temp = model.NetAmount - roundnet;
                    round = temp * (-(1));
                }
                model.RoundValue = round;
            }
            return Json(sales.Save(model));
        }

        [MyAuthorize]
        [HttpPost]
        public ActionResult Update(SalesInvoiceModels model)
        {
            decimal? round = null;
            if (model.RoundOff != null && Convert.ToBoolean(model.RoundOff))
            {
                decimal roundnet = Math.Round(model.NetAmount, 0);
                decimal? temp = 0;
                if (model.NetAmount < roundnet)
                {
                    temp = roundnet - model.NetAmount;
                    round = temp;
                }
                else
                {
                    temp = model.NetAmount - roundnet;
                    round = temp * (-(1));
                }
                model.RoundValue = round;
            }
            return Json(sales.Update(model));
        }
    }
}