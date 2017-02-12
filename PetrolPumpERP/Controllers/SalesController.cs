using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetrolPumpERP.Models;
using PetrolPumpERP.Models.DataModels;

namespace PetrolPumpERP.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        SalesModelBL sales = SalesModelBL.Instance;
        

        [MyAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [MyAuthorize]
        [HttpGet]
        public ActionResult GetSalesInvoices()
        {
            return Json(sales.GetSalesInvoices(),JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public ActionResult Save(SalesInvoiceModels model)
        {
            decimal? round = null;
            if (model.RoundOff != null && Convert.ToBoolean(model.RoundOff))
            {
                decimal roundnet =Math.Round(model.NetAmount, 0);
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
                
                //if (temp < Convert.ToDecimal(0.5))
                //{
                //    round = temp;// * (-(1));
                //}
                //else
                //{
                //    round = temp * (-(1));
                //}
                model.RoundValue = round;
            }
            return Json(sales.Save(model));
        }

        [HttpPost]
        public ActionResult Update(SalesInvoiceModels model)
        {
            return View(sales.Update(model));
        }
    }
}